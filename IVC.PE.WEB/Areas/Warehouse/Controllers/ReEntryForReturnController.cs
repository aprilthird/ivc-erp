using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.ReEntryForReturnViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/reingreso-por-devolucion")]
    public class ReEntryForReturnController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public ReEntryForReturnController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<ReEntryForReturnController> logger) 
            : base(context, userManager, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? workFrontId = null, Guid? sewerGroupId = null, 
            Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            var query = _context.ReEntryForReturns
                .Include(x => x.ProjectFormula)
                .Include(x => x.WorkFront)
                .Include(x => x.SupplyFamily)
                .Include(x => x.SewerGroup)
                .Where(x => x.ProjectFormula.ProjectId == GetProjectId());

            var items = await _context.ReEntryForReturnItems
                .Include(x => x.GoalBudgetInput.Supply)
                .ToListAsync();

            var users = await _context.Users.ToListAsync();

            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);

            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerGroupId == sewerGroupId);

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.SupplyFamilyId == supplyFamilyId);

            var reEntries = new List<ReEntryForReturnViewModel>();

            foreach (var reEntry in query)
            {
                var grupo = items.FirstOrDefault(x => x.GoalBudgetInput.Supply.SupplyGroupId == supplyGroupId
                    && x.ReEntryForReturnId == reEntry.Id);

                if (supplyGroupId.HasValue && grupo == null)
                    continue;
                else
                    reEntries.Add(new ReEntryForReturnViewModel
                    {
                        Id = reEntry.Id,
                        DocumentNumber = reEntry.DocumentNumber.ToString("D4"),
                        ReturnDate = reEntry.ReturnDate.ToDateString(),
                        ProjectFormula = new ProjectFormulaViewModel
                        {
                            Name = reEntry.ProjectFormula.Code + " - " + reEntry.ProjectFormula.Name
                        },
                        WorkFront = new WorkFrontViewModel
                        {
                            Code = reEntry.WorkFront.Code
                        },
                        SewerGroup = new SewerGroupViewModel
                        {
                            Code = reEntry.SewerGroup.Code
                        },
                        //Warehouse = new WarehouseViewModel
                        //{
                        //    Address = reEntry.Warehouse.Address
                        //},
                        SupplyFamily = new SupplyFamilyViewModel
                        {
                            Name = reEntry.SupplyFamily.Name
                        },
                        UserId = reEntry.UserId,
                        Status = reEntry.Status,
                        UserName = users.FirstOrDefault(x => x.Id == reEntry.UserId).FullName,
                        FileUrl = reEntry.FileUrl
                    });
            }

            return Ok(reEntries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var reEntry = await _context.ReEntryForReturns
                .Include(x => x.ProjectFormula)
                .Include(x => x.WorkFront)
                .Include(x => x.SupplyFamily)
                .Include(x => x.SewerGroup)
                .Select(x => new ReEntryForReturnViewModel
                {
                    Id = x.Id,
                    DocumentNumber = x.DocumentNumber.ToString("D4"),
                    ReturnDate = x.ReturnDate.ToDateString(),
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Name = x.ProjectFormula.Code + " - " + x.ProjectFormula.Name
                    },
                    WorkFrontId = x.WorkFrontId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    },
                    //Warehouse = new WarehouseViewModel
                    //{
                    //    Address = x.Warehouse.Address
                    //},
                    SupplyFamilyId = x.SupplyFamilyId,
                    SupplyFamily = new SupplyFamilyViewModel
                    {
                        Name = x.SupplyFamily.Name
                    },
                    UserId = x.UserId
                }).FirstOrDefaultAsync(x => x.Id == id);

            return Ok(reEntry);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ReEntryForReturnViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var count = 0;

            if (_context.ReEntryForReturns.Count() != 0)
                count = _context.FieldRequests.OrderBy(x => x.DocumentNumber).Last().DocumentNumber;

            var reEntry = new ReEntryForReturn {
                DocumentNumber = count + 1,
                ReturnDate = model.ReturnDate.ToDateTime(),
                WorkFrontId = model.WorkFrontId,
                SewerGroupId = model.SewerGroupId,
                SupplyFamilyId = model.SupplyFamilyId,
                ProjectFormulaId = model.ProjectFormulaId,
                UserId = GetUserId(),
                Status = ConstantHelpers.Warehouse.ReEntryForReturn.Status.INPROCESS
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                reEntry.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.WAREHOUSE, System.IO.Path.GetExtension(model.File.FileName), 
                    ConstantHelpers.Storage.Blobs.RE_ENTRY_FOR_RETURN, reEntry.DocumentNumber.ToString("D4"));
            }

            await _context.ReEntryForReturns.AddAsync(reEntry);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ReEntryForReturnViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reEntry = await _context.ReEntryForReturns.FirstOrDefaultAsync(x => x.Id == id);

            if (reEntry.Status == ConstantHelpers.Warehouse.ReEntryForReturn.Status.CONFIRMED)
                return BadRequest("El reingreso ya ha sido confirmado");

            reEntry.ReturnDate = model.ReturnDate.ToDateTime();
            reEntry.WorkFrontId = model.WorkFrontId;
            reEntry.SewerGroupId = model.SewerGroupId;
            reEntry.SupplyFamilyId = model.SupplyFamilyId;
            reEntry.ProjectFormulaId = model.ProjectFormulaId;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (reEntry.FileUrl != null)
                    await storage.TryDelete(reEntry.FileUrl, ConstantHelpers.Storage.Containers.WAREHOUSE);
                reEntry.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.WAREHOUSE, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.RE_ENTRY_FOR_RETURN, reEntry.DocumentNumber.ToString("D4"));
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var reEntry = await _context.ReEntryForReturns.FirstOrDefaultAsync(x => x.Id == id);

            if (reEntry == null)
                return BadRequest("No se ha encontrado el reingreso");

            if(reEntry.Status == ConstantHelpers.Warehouse.ReEntryForReturn.Status.CONFIRMED)
                return BadRequest("El reingreso ya ha sido confirmado");

            var items = await _context.ReEntryForReturnItems
                .Where(x => x.ReEntryForReturnId == id)
                .ToListAsync();

            if (reEntry.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete(reEntry.FileUrl, ConstantHelpers.Storage.Containers.WAREHOUSE);
            }

            _context.ReEntryForReturnItems.RemoveRange(items);
            _context.ReEntryForReturns.Remove(reEntry);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("confirmar/{id}")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var entry = await _context.ReEntryForReturns
                .Include(x => x.WorkFront)
                .Include(x => x.SewerGroup)
                .Include(x => x.ProjectFormula.Project)
                .FirstOrDefaultAsync(x => x.Id == id);

            var pId = GetProjectId();

            var correlative = await _context.RequestSummaries
               .Where(x => x.ProjectId == pId)
               .FirstOrDefaultAsync();

            if (entry.Status == ConstantHelpers.Warehouse.ReEntryForReturn.Status.CONFIRMED)
                return BadRequest("El ingreso ya ha sido confirmado");

            if (entry.FileUrl == null)
                return BadRequest("El Reingreso por devolución no cuenta con su archivo adjunto");

            entry.Status = ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED;

            var responsibles = await _context.WarehouseResponsibles
                .Where(x => x.UserType == ConstantHelpers.Warehouse.UserTypes.ThecnicalOfficeControl
                || x.UserType == ConstantHelpers.Warehouse.UserTypes.StoreKeepers)
                .Where(x => x.ProjectId == pId)
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();

            var items = await _context.ReEntryForReturnItems
                .Include(x => x.GoalBudgetInput)
                .Include(x => x.GoalBudgetInput.Supply)
                .Include(x => x.GoalBudgetInput.MeasurementUnit)
                .Include(x => x.GoalBudgetInput.Supply.SupplyFamily)
                .Include(x => x.GoalBudgetInput.Supply.SupplyGroup)
                .Where(x => x.ReEntryForReturnId == id)
                .ToListAsync();

            var stock = await _context.Stocks
                .Include(x => x.Supply)
                .ToListAsync();

            var newStock = new List<Stock>();

            var listMessage = "";

            foreach (var item in items)
            {
                var existe = stock.FirstOrDefault(x => x.SupplyId == item.GoalBudgetInput.SupplyId);

                if (existe == null)
                    return BadRequest("No se ha encontrado el insumo en stock");

                existe.Measure += item.Quantity;

                listMessage += item.GoalBudgetInput.Supply.FullCode + " | " + item.GoalBudgetInput.Supply.Description
                    + " | " + item.GoalBudgetInput.MeasurementUnit.Abbreviation
                    + " | " + item.Quantity.ToString(CultureInfo.InvariantCulture) + "<br />";
            }

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                Subject = $"{correlative.CodePrefix} - Aviso de confirmación del Reingreso por devolución {entry.DocumentNumber.ToString("D4")}"
            };

            WebClient webClient = new WebClient();
            Attachment data = new Attachment(webClient.OpenRead(entry.FileUrl),
                $"Reingreso-por-devolución-{entry.DocumentNumber.ToString("D4")}.pdf");
            mailMessage.Attachments.Add(data);
            
            foreach (var auth in responsibles)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == auth);

                mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
            }
            //mailMessage.To.Add(new MailAddress("henrrypaul_22@hotmail.com", "Henry"));
            //mailMessage.To.Add(new MailAddress("arian.cc@hotmail.com", "Arian"));

            mailMessage.Body =
                $"Hola, <br /><br /> " +
                $"Con fecha {entry.ReturnDate.ToString("dd/MM/yyyy")} ha reingresado por devolución en el centro de costo {entry.ProjectFormula.Project.Abbreviation} los siguientes insumos: <br />" +
                $"<br />" +
                //$"Ingreso de Material N° " + entry.DocumentNumber.ToString("D4") + "<br />" +
                $"Fórmula {entry.ProjectFormula.Code} - {entry.ProjectFormula.Name}<br />" +
                $"Frente {entry.WorkFront.Code}<br />" +
                $"Cuadrilla {entry.SewerGroup.Code}<br />" +
                $"<br />" +
                $"Codigo IVC | Insumo | UND | Cantidad | Observación <br />" +
                listMessage +
                $"<br />" +
                $"<br />" +
                $"Saludos <br />" +
                $"Sistema IVC <br />" +
                $"Control de Almacén";
            mailMessage.IsBodyHtml = true;

            //Mandar Correo
            using (var client = new SmtpClient("smtp.office365.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                client.EnableSsl = true;
                await client.SendMailAsync(mailMessage);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        #region ITEMS

        [HttpGet("items/listar")]
        public async Task<IActionResult> GetAllItems(Guid id)
        {
            var items = await _context.ReEntryForReturnItems
                .Include(x => x.GoalBudgetInput.Supply)
                .Include(x => x.GoalBudgetInput.Supply.SupplyFamily)
                .Include(x => x.GoalBudgetInput.Supply.SupplyGroup)
                .Include(x => x.GoalBudgetInput.Supply.MeasurementUnit)
                .Include(x => x.ProjectPhase)
                .Where(x => x.ReEntryForReturnId == id)
                .Select(x => new ReEntryForReturnItemViewModel
                {
                    Id = x.Id,
                    GoalBudgetInput = new GoalBudgetInputViewModel
                    {
                        SupplyId = x.GoalBudgetInput.SupplyId,
                        Supply = new SupplyViewModel
                        {
                            CorrelativeCode = x.GoalBudgetInput.Supply.CorrelativeCode,
                            SupplyFamily = new SupplyFamilyViewModel
                            {
                                Name = x.GoalBudgetInput.Supply.SupplyFamily.Name,
                                Code = x.GoalBudgetInput.Supply.SupplyFamily.Code
                            },
                            SupplyGroup = new SupplyGroupViewModel
                            {
                                Name = x.GoalBudgetInput.Supply.SupplyGroup.Name,
                                Code = x.GoalBudgetInput.Supply.SupplyGroup.Code
                            },
                            Description = x.GoalBudgetInput.Supply.Description,
                            MeasurementUnit = new MeasurementUnitViewModel
                            {
                                Abbreviation = x.GoalBudgetInput.Supply.MeasurementUnit.Abbreviation
                            }
                        }
                    },
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Code = x.ProjectPhase.Code + " - " + x.ProjectPhase.Description
                    },
                    Quantity = x.Quantity.ToString(CultureInfo.InvariantCulture),
                    Observations = x.Observations
                }).ToListAsync();

            return Ok(items);
        }

        [HttpGet("items/{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var folding = await _context.ReEntryForReturnItems
                .Include(x => x.GoalBudgetInput.Supply)
                .Select(x => new ReEntryForReturnItemViewModel
                {
                    Id = x.Id,
                    ReEntryForReturnId = x.ReEntryForReturnId,
                    GoalBudgetInputId = x.GoalBudgetInputId,
                    GoalBudgetInput = new GoalBudgetInputViewModel
                    {
                        Supply = new SupplyViewModel
                        {
                            SupplyGroupId = x.GoalBudgetInput.Supply.SupplyGroupId
                        }
                    },
                    ProjectPhaseId = x.ProjectPhaseId,
                    Quantity = x.Quantity.ToString(CultureInfo.InvariantCulture),
                    Observations = x.Observations
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(folding);
        }

        [HttpPost("items/crear")]
        public async Task<IActionResult> CreateItem(ReEntryForReturnItemViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = new ReEntryForReturnItem
            {
                ReEntryForReturnId = model.ReEntryForReturnId,
                GoalBudgetInputId = model.GoalBudgetInputId,
                ProjectPhaseId = model.ProjectPhaseId,
                Quantity = model.Quantity.ToDoubleString(),
                Observations = model.Observations
            };

            await _context.ReEntryForReturnItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("items/editar/{id}")]
        public async Task<IActionResult> EditItem(Guid id, ReEntryForReturnItemViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _context.ReEntryForReturnItems
                .Include(x => x.ReEntryForReturn)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item.ReEntryForReturn.Status == ConstantHelpers.Warehouse.ReEntryForReturn.Status.CONFIRMED)
                return BadRequest("El reingreso ya ha sido confirmado");

            item.GoalBudgetInputId = model.GoalBudgetInputId;
            item.ProjectPhaseId = model.ProjectPhaseId;
            item.Quantity = model.Quantity.ToDoubleString();
            item.Observations = model.Observations;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("items/eliminar/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var item = await _context.ReEntryForReturnItems
                .Include(x => x.ReEntryForReturn)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return BadRequest("No se ha encontrado el item");

            if (item.ReEntryForReturn.Status == ConstantHelpers.Warehouse.ReEntryForReturn.Status.CONFIRMED)
                return BadRequest("El reingreso ya ha sido confirmado");

            _context.ReEntryForReturnItems.Remove(item);
            await _context.SaveChangesAsync();
            return Ok();
        }

        #endregion
    }
}
