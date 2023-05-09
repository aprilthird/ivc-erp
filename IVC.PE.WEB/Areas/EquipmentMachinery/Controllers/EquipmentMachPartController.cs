using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachPartViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.MachineryPhaseViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/parte-equipo-maquinaria")]
    public class EquipmentMachPartController : BaseController
    {

        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachPartController(IvcDbContext context,
        ILogger<EquipmentMachPartController> logger,
        IOptions<CloudStorageCredentials> storageCredentials) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null, int? month = null, int? year = null, int? week = null, Guid? providerId = null, Guid? typeId = null)
        {
            var pId = GetProjectId();

            SqlParameter param1 = new SqlParameter("@Week", System.Data.SqlDbType.Int);
            param1.Value = (object)week ?? DBNull.Value;

            var data = await _context.Set<UspMachineryPart>().FromSqlRaw("execute EquipmentMachinery_uspMachineryPart @Week"
                ,param1)
         .IgnoreQueryFilters()
         .ToListAsync();

            data = data.Where(x => x.ProjectId == pId).OrderBy(x => x.TradeName).ToList();

            if (month == 1)
                data = data.Where(x => x.Month == 1 ).ToList();
            if (month == 2)
                data = data.Where(x => x.Month== 2  ).ToList();
            if (month == 3)
                data = data.Where(x => x.Month== 3 ).ToList();
            if (month == 4)
                data = data.Where(x => x.Month== 4 ).ToList();
            if (month == 5)
                data = data.Where(x => x.Month == 5 ).ToList();
            if (month == 6)
                data = data.Where(x => x.Month == 6  ).ToList();
            if (month == 7)
                data = data.Where(x => x.Month  == 7  ).ToList();
            if (month == 8)
                data = data.Where(x => x.Month  == 8  ).ToList();
            if (month == 9)
                data = data.Where(x => x.Month  == 9  ).ToList();
            if (month == 10)
                data = data.Where(x => x.Month  == 10  ).ToList();
            if (month == 11)
                data = data.Where(x => x.Month  == 11  ).ToList();
            if (month == 12)
                data = data.Where(x => x.Month  == 12  ).ToList();

            if (year.HasValue)
                data = data.Where(x => x.YearPartDate == year.Value && x.FoldingId != null).ToList();

            if (providerId.HasValue)
            {
                data = data.Where(x => x.EquipmentProviderId == providerId.Value).ToList();
            }
            if(typeId.HasValue)
            {
                data = data.Where(x => x.TypeFilter == typeId.Value).ToList();
            }

            if(week.HasValue)
            {
                data = data.Where(x => x.WeekDateFilter == week.Value).ToList();
            }
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentMachParts
                .Include(x => x.EquipmentProvider.Provider)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachPartViewModel
                {
                    Id = x.Id,
                    EquipmentProviderId = x.EquipmentProviderId,

                    EquipmentMachineryTypeTypeId = x.EquipmentMachineryTypeTypeId,

                    EquipmentMachId = x.EquipmentMachId,
                    Year = x.Year,
                    Month = x.Month,

                }).FirstOrDefaultAsync();


            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var softpart = new EquipmentMachPart
            {
                ProjectId = GetProjectId(),
                //PartNumber = model.PartNumber,
                //  = model. .ToDateTime(),
                EquipmentMachineryTypeTypeId = model.EquipmentMachineryTypeTypeId,
                EquipmentProviderId = model.EquipmentProviderId,
                EquipmentMachId = model.EquipmentMachId,
                Month = model.Month,
                Year = model.Year
                //EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId,
                //UserId = model.UserId,
                //UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname,

            };

            await _context.EquipmentMachParts.AddAsync(softpart);
            await _context.SaveChangesAsync();
            return Ok(softpart.Id);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var softpart = await _context.EquipmentMachParts
                .FirstOrDefaultAsync(x => x.Id == id);
            softpart.EquipmentMachineryTypeTypeId = model.EquipmentMachineryTypeTypeId;
            softpart.EquipmentProviderId = model.EquipmentProviderId;
            softpart.EquipmentMachId = model.EquipmentMachId;
            softpart.Year = model.Year;
            softpart.Month = model.Month;
            //softpart.UserId = model.UserId;
            //softpart.UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bondAdd = await _context.EquipmentMachParts.FirstOrDefaultAsync(x => x.Id == id);


            _context.EquipmentMachParts.Remove(bondAdd);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("detalles/listar")]
        public async Task<IActionResult> GetAllDetails(Guid? softPartId = null, int? week= null)
        {
            if (!softPartId.HasValue)
                return Ok(new List<EquipmentMachPartViewModel>());

            var query = await _context.EquipmentMachPartFoldings
                            .Include(x => x.EquipmentMachPart)
                            .Include(x => x.EquipmentMachineryOperator)
                            .Include(x => x.EquipmentMachineryOperator.Worker)
                            .Include(x => x.EquipmentMachineryTypeTypeActivity)
                            .Include(x => x.SewerGroup)
                            .Include(x => x.MachineryPhase)
                            .Where(x => x.EquipmentMachPartId == softPartId)
                            .Select(x => new EquipmentMachPartFoldingViewModel
                            {
                                Id = x.Id,
                                EquipmentMachPartId = x.EquipmentMachPartId,
                                EquipmentMachPart = new EquipmentMachPartViewModel
                                {
                                    EquipmentMachineryTypeTypeId = x.EquipmentMachPart.EquipmentMachineryTypeTypeId,
                                    EquipmentProviderId = x.EquipmentMachPart.EquipmentProviderId,
                                    EquipmentProvider = new EquipmentProviderViewModel
                                    {
                                        ProviderId = x.EquipmentMachPart.EquipmentProvider.ProviderId,
                                    }

                                },

                                 PartDate = x.PartDate.Date.ToDateString(),
                                PartNumber = x.PartNumber,
                                EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,
                                EquipmentMachineryOperator = new EquipmentMachineryOperatorViewModel
                                {
                                    ActualName = x.EquipmentMachineryOperator.ActualName,
                                    WorkerId = x.EquipmentMachineryOperator.WorkerId,

                                    Worker = x.EquipmentMachineryOperator.WorkerId != null ? new WorkerViewModel
                                    {

                                        Name = x.EquipmentMachineryOperator.Worker.Name,
                                        MiddleName = x.EquipmentMachineryOperator.Worker.MiddleName,
                                        PaternalSurname = x.EquipmentMachineryOperator.Worker.PaternalSurname,
                                        MaternalSurname = x.EquipmentMachineryOperator.Worker.MaternalSurname,
                                        PhoneNumber = x.EquipmentMachineryOperator.Worker.PhoneNumber,
                                        Document = x.EquipmentMachineryOperator.Worker.Document,
                                        DocumentType = x.EquipmentMachineryOperator.Worker.DocumentType

                                    } : null,

                                },
                                InitHorometer = x.InitHorometer,
                                EndHorometer = x.EndHorometer,
                                Dif = x.Dif,
                                Specific = x.Specific,

                                UserId = x.UserId,
                                UserName = x.UserName,
                                WorkArea = x.WorkArea,

                                Order = x.Order,
                                EquipmentMachineryTypeTypeActivityId = x.EquipmentMachineryTypeTypeActivityId,
                                EquipmentMachineryTypeTypeActivity = new EquipmentMachineryTypeTypeActivityViewModel
                                {
                                    Description = x.EquipmentMachineryTypeTypeActivity.Description

                                },

                                SewerGroupId = x.SewerGroupId,
                                SewerGroup = new SewerGroupViewModel
                                {

                                    Name = x.SewerGroup.Code

                                },

                                MachineryPhaseId = x.MachineryPhaseId,
                                MachineryPhase = new MachineryPhaseViewModel
                                {
                                    ProjectPhaseId = x.MachineryPhase.ProjectPhaseId,
                                    ProjectPhase = new ProjectPhaseViewModel
                                    {
                                        Code = x.MachineryPhase.ProjectPhase.Code
                                    }
                                }

                            })
                            .OrderBy(x=>x.Order)
                            .ToListAsync();

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            if (week.HasValue)
            {
                query = query.Where(x => cal.GetWeekOfYear(x.PartDate.ToDateTime(), dfi.CalendarWeekRule, DayOfWeek.Monday) - 1 == week.Value).ToList();
            }

            return Ok(query);
        }

        [HttpPost("detalles/crear")]
        public async Task<IActionResult> CreateDetail(EquipmentMachPartFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var tempusers = new List<IVC.PE.ENTITIES.Models.General.ApplicationUser>
            {
                new IVC.PE.ENTITIES.Models.General.ApplicationUser
                {
                    Name = "",
                    PaternalSurname = "",
                    MaternalSurname = "",
                    WorkArea = 0

                }
            };

            if (users == null)
                users = tempusers.FirstOrDefault();
            var last = await _context.EquipmentMachPartFoldings.OrderByDescending(x => x.Order).FirstOrDefaultAsync();
            var equipmentMachineryTransportPart = await _context.EquipmentMachParts.Where(x => x.Id == model.EquipmentMachPartId).FirstOrDefaultAsync();

            equipmentMachineryTransportPart.FoldingNumber++;


            var newRenovation = new EquipmentMachPartFolding()
            {
                EquipmentMachPartId = model.EquipmentMachPartId,
                PartDate= model.PartDate.ToDateTime(),
                PartNumber = model.PartNumber,
                EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId,
                InitHorometer = model.InitHorometer,
                EndHorometer = model.EndHorometer,
                Dif = model.EndHorometer - model.InitHorometer,
                Specific = model.Specific,
                UserId = model.UserId ,

                UserName = model.UserId != null?users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname : null,
                WorkArea = model.UserId != null ? users.WorkArea : 0,

                EquipmentMachineryTypeTypeActivityId = model.EquipmentMachineryTypeTypeActivityId,
                SewerGroupId = model.SewerGroupId,
                MachineryPhaseId = model.MachineryPhaseId,
                Order = equipmentMachineryTransportPart.FoldingNumber
            };

            if (newRenovation.InitHorometer > newRenovation.EndHorometer)
                return BadRequest("El Horometro Inicial debe ser Mayor al Horometro Final");


            await _context.EquipmentMachPartFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();
            return Ok();
        }

        
    }
}
