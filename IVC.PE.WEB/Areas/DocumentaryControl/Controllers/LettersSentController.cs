using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.UspModels.DocumentaryControl;
using IVC.PE.WEB.Areas.Admin.ViewModels.InterestGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.IssuerTargetViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.DocumentaryControl.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.DOCUMENTARY_CONTROL)]
    [Route("control-documentario/cartas-enviadas")]
    public class LettersSentController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public LettersSentController(IvcDbContext context,
            ILogger<LettersSentController> logger,
            IOptions<CloudStorageCredentials> storageCredentials) 
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("receptores")]
        public async Task<IActionResult> GetReceivers(int? page = null, int rowsPerPage = 5)
        {
            var projectId = GetProjectId();

            var query = _context.IssuerTargets.AsQueryable();
            if (page.HasValue)
                query = query.Skip(page.Value * rowsPerPage).Take(rowsPerPage);
            var model = await query
                .Select(x => new IssuerTargetSummaryViewModel
                {
                    IssuerTargetId = x.Id,
                    Acronym = x.Acronym,
                    Name = x.Name,
                    TotalCount = x.LetterIssuerTargets
                        .Where(y => y.Letter.ProjectId == projectId && y.Letter.Type == ConstantHelpers.Letter.Type.SENT)
                        .Count()
                })
                .OrderByDescending(x => x.TotalCount)
                .AsNoTracking()
                .ToListAsync();
            model.Add(new IssuerTargetSummaryViewModel
            {
                IssuerTargetId = null,
                Acronym = "VARIOS",
                Name = "VARIOS",
                TotalCount = model.Where(x => x.TotalCount < 5).Sum(x => x.TotalCount),
                Other = true
            });
            return Ok(model.Where(x => x.TotalCount >= 5));
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? docCharac = null, Guid? ig = null, Guid? issuerTargetId = null, bool? hasAnswer = null)
        {
            SqlParameter letterTypeParam = new SqlParameter("@LetterType", ConstantHelpers.Letter.Type.SENT);
            SqlParameter projectIdParam = new SqlParameter("@ProjectId", GetProjectId());

            var letters = await _context.Set<UspLetter>().FromSqlRaw("execute DocumentaryControl_uspLetters @LetterType, @ProjectId"
                , letterTypeParam, projectIdParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            if (docCharac.HasValue)
                letters = letters.Where(x => x.DocCharacteristicGuids.Any(y => y == docCharac.Value)).ToList();
            if (ig.HasValue)
                letters = letters.Where(x => x.InterestGroupGuids.Any(y => y == ig.Value)).ToList();
            if (issuerTargetId.HasValue)
                letters = letters.Where(x => x.IssuerId == issuerTargetId.Value).ToList();
            if (hasAnswer.HasValue)
                if (hasAnswer.Value)
                    letters = letters.Where(x => x.HasAnswers > 0).ToList();
                else
                    letters = letters.Where(x => x.HasAnswers == 0).ToList();


            return Ok(letters);

            //var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            //var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            //var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);
            //var orderNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.ORDER_COLUMN]);
            //var orderDirection = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.ORDER_DIRECTION].ToString().ToUpper();

            //var query = _context.Letters
            //    .Where(x => x.Type == ConstantHelpers.Letter.Type.SENT)
            //    .OrderByDescending(x => x.Date)
            //    .AsNoTracking()
            //    .AsQueryable();

            //if (orderNumber == 0 && !string.IsNullOrEmpty(orderDirection))
            //    query = orderDirection == ConstantHelpers.Datatable.ServerSide.Default.ORDER_DIRECTION
            //        ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
            //if (orderNumber == 2 && !string.IsNullOrEmpty(orderDirection))
            //    query = orderDirection == ConstantHelpers.Datatable.ServerSide.Default.ORDER_DIRECTION
            //        ? query.OrderByDescending(x => x.Date) : query.OrderBy(x => x.Date);

            //var projectId = GetProjectId();

            //query = query.Where(x => x.ProjectId == projectId);

            //var totalRecords = await query.CountAsync();

            //if (status.HasValue)
            //    query = query.Where(x => x.LetterStatus.Any(l => l.Status == status.Value));
            //if (issuerTargetId.HasValue)
            //    query = query.Where(x => x.LetterIssuerTargets.Any(l => l.IssuerTargetId == issuerTargetId.Value));
            //if (interestGroupId.HasValue)
            //    query = query.Where(x => x.LetterInterestGroups.Any(l => l.InterestGroupId == interestGroupId.Value));
            //if (hasFile.HasValue)
            //    query = query.Where(x => hasFile.Value ? x.FileUrl != null : true);
            //if (hasAnswer.HasValue)
            //    query = query.Where(x => x.LetterStatus.Any(s => s.Status == ConstantHelpers.Letter.Status.QUERY) && hasAnswer.Value == x.ReferencedBy.Any(r => r.Letter.LetterStatus.Any(s => s.Status == ConstantHelpers.Letter.Status.ANSWER)));
            //if (other.HasValue && other.Value)
            //    query = query.Where(x => x.LetterIssuerTargets.Any(l => l.IssuerTarget.LetterIssuerTargets.Count() < 5));

            //if (!string.IsNullOrEmpty(search))
            //    query = query.Where(x => x.Name.Contains(search) ||
            //        x.Subject.Contains(search));

            //var data = await query
            //    .Skip(currentNumber)
            //    .Take(recordsPerPage)
            //    .Select(x => new LetterViewModel
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        Code = x.Code,
            //        Subject = x.Subject,
            //        Date = x.Date.ToDateString(),
            //        ReferenceIds = x.References.Select(r => r.LetterId).ToList(),
            //        ResponseTermDays = x.LetterStatus.Any(l => l.Status == ConstantHelpers.Letter.Status.QUERY)
            //            ? x.ReferencedBy.Any(r => r.Letter.Type == ConstantHelpers.Letter.Type.RECEIVED && r.Letter.LetterStatus.Any(l => l.Status == ConstantHelpers.Letter.Status.ANSWER))
            //            ? x.ReferencedBy.Where(r => r.Letter.Type == ConstantHelpers.Letter.Type.RECEIVED && r.Letter.LetterStatus.Any(l => l.Status == ConstantHelpers.Letter.Status.ANSWER))
            //                .Select(r => (int?)EF.Functions.DateDiffDay(x.Date, r.Letter.Date)).FirstOrDefault()
            //                : EF.Functions.DateDiffDay(x.Date, DateTime.UtcNow)
            //            : x.LetterStatus.Any(l => l.Status== ConstantHelpers.Letter.Status.ANSWER)
            //            ? x.References.Any(r => r.Letter.Type == ConstantHelpers.Letter.Type.RECEIVED && r.Letter.LetterStatus.Any(l => l.Status == ConstantHelpers.Letter.Status.QUERY_US || l.Status == ConstantHelpers.Letter.Status.OBSERVATION))
            //            ? x.References.Where(r => r.Letter.Type == ConstantHelpers.Letter.Type.RECEIVED && r.Letter.LetterStatus.Any(l => l.Status == ConstantHelpers.Letter.Status.QUERY_US || l.Status == ConstantHelpers.Letter.Status.OBSERVATION))
            //                .Select(r => (int?)EF.Functions.DateDiffDay(r.Letter.Date, x.Date)).FirstOrDefault()
            //            : null : null,
            //        References = x.References.Select(r => new LetterViewModel
            //        {
            //            Name = r.Reference.Name
            //        }),
            //        Status = x.LetterStatus.Select(l => l.LetterDocumentCharacteristicId).ToList(),
            //        InterestGroups = x.LetterInterestGroups.Select(l => new InterestGroupViewModel 
            //        {
            //            Name = l.InterestGroup.Name
            //        }).ToList(),
            //        IssuerTargets = x.LetterIssuerTargets.Select(l => new IssuerTargetViewModel
            //        {
            //            Name = l.IssuerTarget.Name
            //        }).ToList(),
            //        Employee = !string.IsNullOrEmpty(x.EmployeeId)
            //            ? new UserViewModel
            //            {
            //                Id = x.EmployeeId,
            //                Name = x.Employee.Name,
            //                PaternalSurname = x.Employee.PaternalSurname,
            //                MaternalSurname = x.Employee.MaternalSurname,
            //                MiddleName = x.Employee.MiddleName
            //            } : null,
            //        FileUrl = x.FileUrl
            //    }).ToListAsync();

            //return Ok(new
            //{
            //    draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
            //    recordsTotal = totalRecords,
            //    recordsFiltered = await query.CountAsync(),
            //    data
            //});
        }

        [HttpGet("listar-referencias")]
        public async Task<IActionResult> GetReferences(Guid? letterId = null)
        {
            if (letterId == null)
                return Ok(new List<UspLetterReference>());

            SqlParameter letterParam = new SqlParameter("@LetterId", letterId);

            var refs = await _context.Set<UspLetterReference>().FromSqlRaw("execute DocumentaryControl_uspLetterReferences @LetterId",
                letterParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(refs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLetter(Guid id)
        {
            var model = await _context.Letters
                .Where(x => x.Id == id)
                .Select(x => new LetterViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Subject = x.Subject,
                    Date = x.Date.ToDateString(),
                    ReferenceIds = x.References.Select(r => r.ReferenceId).ToList(),
                    ResponseTermDays = x.ResponseTermDays,
                    References = x.References.Select(r => new LetterViewModel
                    {
                        Name = r.Reference.Name
                    }),
                    Status = x.LetterStatus.Select(l => l.LetterDocumentCharacteristicId).ToList(),
                    InterestGroupIds = x.LetterInterestGroups.Select(l => l.InterestGroupId),
                    InterestGroups = x.LetterInterestGroups.Select(l => new InterestGroupViewModel
                    {
                        Name = l.InterestGroup.Name
                    }).ToList(),
                    IssuerTargetIds = x.LetterIssuerTargets.Select(l => l.IssuerTargetId),
                    IssuerTargets = x.LetterIssuerTargets.Select(l => new IssuerTargetViewModel
                    {
                        Name = l.IssuerTarget.Name
                    }).ToList(),
                    EmployeeId = x.EmployeeId,
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();
            return Ok(model);
        }

        [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.FULL_ACCESS)]
        [HttpPost("crear")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Create(LetterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var letter = new Letter
            {
                Type = ConstantHelpers.Letter.Type.SENT,
                Code = model.Code,
                Name = model.Name,
                ProjectId = GetProjectId(),
                Subject = model.Subject,
                Date = model.Date.ToDateTime(),
                EmployeeId = model.EmployeeId
            };
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                letter.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL, System.IO.Path.GetExtension(model.File.FileName), ConstantHelpers.Storage.Blobs.LETTERS_SENT, letter.Code);
            }
            if (model.Status != null)
                await _context.LetterStatus.AddRangeAsync(
                    model.Status.Select(x => new LetterStatus
                    {
                        Status = 0,
                        LetterDocumentCharacteristicId = x,
                        Letter = letter
                    }).ToList());
            if (model.IssuerTargetIds != null)
                await _context.LetterIssuerTargets.AddRangeAsync(
                    model.IssuerTargetIds.Select(x => new LetterIssuerTarget
                    {
                        IssuerTargetId = x,
                        Letter = letter
                    }).ToList());
            if (model.InterestGroupIds != null)
                await _context.LetterInterestGroups.AddRangeAsync(
                        model.InterestGroupIds.Select(x => new LetterInterestGroup
                        {
                            InterestGroupId = x,
                            Letter = letter
                        }).ToList());
            if (model.ReferenceIds != null)
                await _context.LetterLetters.AddRangeAsync(
                    model.ReferenceIds.Select(x => new LetterLetter
                    {
                        ReferenceId = x,
                        Letter = letter
                    }).ToList());

            await _context.Letters.AddAsync(letter);
            await _context.SaveChangesAsync();
            var employee = await _context.Users.FindAsync(model.EmployeeId);

            /*Código para el envió de correo con CC al grupo de ínteres*/
            var employeesCC = new Dictionary<string, (string, string)>();
            if (model.InterestGroupIds != null)
            {
                foreach (var interestGroupId in model.InterestGroupIds)
                {
                    var users = await _context.UserInterestGroups
                        .Include(x => x.User)
                        .Where(x => x.InterestGroupId.Equals(interestGroupId))
                        .ToListAsync();
                    foreach (var us in users)
                    {
                        if (!employeesCC.ContainsKey(us.UserId))
                        {
                            employeesCC[us.UserId] = (us.User.RawFullName, us.User.Email);
                        }
                    }
                } 
            }

            //if (employee != null)
            //{
            //    await SendConfirmationEmail(model.File, employee, letter, employeesCC); 
            //}
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Edit(Guid id, LetterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var letter = await _context.Letters
                .Include(x => x.LetterIssuerTargets)
                .Include(x => x.LetterInterestGroups)
                .Include(x => x.LetterStatus)
                .Include(x => x.References)
                .Where(x => x.Type == ConstantHelpers.Letter.Type.SENT)
                .FirstOrDefaultAsync(x => x.Id == id);
            letter.Code = model.Code;
            letter.Name = model.Name;
            letter.Subject = model.Subject;
            letter.Date = model.Date.ToDateTime();
            letter.EmployeeId = model.EmployeeId;
            
            if (letter.LetterIssuerTargets != null)
                _context.LetterIssuerTargets.RemoveRange(letter.LetterIssuerTargets);
            if (model.IssuerTargetIds != null)
                await _context.LetterIssuerTargets.AddRangeAsync(
                    model.IssuerTargetIds.Select(x => new LetterIssuerTarget
                    {
                        IssuerTargetId = x,
                        LetterId = letter.Id
                    }).ToList());

            if (letter.LetterInterestGroups != null)
                _context.LetterInterestGroups.RemoveRange(letter.LetterInterestGroups);
            if (model.InterestGroupIds != null)
                await _context.LetterInterestGroups.AddRangeAsync(
                        model.InterestGroupIds.Select(x => new LetterInterestGroup
                        {
                            InterestGroupId = x,
                            LetterId = letter.Id
                        }).ToList());

            if (letter.LetterStatus != null)
                _context.LetterStatus.RemoveRange(letter.LetterStatus);
            if (model.Status != null)
                await _context.LetterStatus.AddRangeAsync(
                model.Status.Select(x => new LetterStatus
                {
                    Status = 0,
                    LetterDocumentCharacteristicId = x,
                    LetterId = letter.Id
                }).ToList());

            if(letter.References != null)
                _context.LetterLetters.RemoveRange(letter.References);
            if (model.ReferenceIds != null)
                await _context.LetterLetters.AddRangeAsync(
                    model.ReferenceIds.Select(x => new LetterLetter
                    {
                        ReferenceId = x,
                        LetterId = letter.Id
                    }).ToList());

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (letter.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LETTERS_SENT}/{letter.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
                letter.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL, System.IO.Path.GetExtension(model.File.FileName), ConstantHelpers.Storage.Blobs.LETTERS_SENT, letter.Code);
            }
            
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var letter = await _context.Letters
                .Include(x => x.LetterInterestGroups)
                .Include(x => x.LetterIssuerTargets)
                .Include(x => x.LetterStatus)
                .Include(x => x.References)
                .Include(x => x.ReferencedBy)
                .Where(x => x.Type == ConstantHelpers.Letter.Type.SENT)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (letter == null)
                return BadRequest($"Carta Enviada con Id '{id}' no encontrada.");
            if (letter.LetterIssuerTargets != null)
                _context.LetterIssuerTargets.RemoveRange(letter.LetterIssuerTargets);
            if (letter.LetterInterestGroups != null)
                _context.LetterInterestGroups.RemoveRange(letter.LetterInterestGroups);
            if (letter.LetterStatus != null)
                _context.LetterStatus.RemoveRange(letter.LetterStatus);
            if (letter.References != null)
                _context.LetterLetters.RemoveRange(letter.References);
            if (letter.ReferencedBy != null)
                _context.LetterLetters.RemoveRange(letter.ReferencedBy);
            if (letter.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LETTERS_SENT}/{letter.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
            }
            _context.Letters.Remove(letter);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task SendConfirmationEmail(IFormFile file, ApplicationUser user, Letter letter, Dictionary<string, (string, string)> employeesCC)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaivctest@gmail.com", "Sistema IVC")
            };
            mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
            mailMessage.Subject = $"Documento emitido ({letter.Name})";
            foreach (var key in employeesCC.Keys)
            {
                mailMessage.CC.Add(new MailAddress(employeesCC[key].Item2, employeesCC[key].Item1));
            }
            //mailMessage.CC.Add(new MailAddress("recepcion@jicamarca.pe", "recepcion"));
            mailMessage.Body =
                $"Hola {user.RawFullName},<br/>Favor de hacer seguimiento a la respuesta a la carta emitida adjunta al correo, informar si en un plazo máximo de {letter.ResponseTermDays} días calendarios no hay respuesta.<br/><br/>Saludos<br/>Sistema IVC<br/>Control Documentario";
            mailMessage.IsBodyHtml = true;
            if(file != null && file?.Length < 1024 * 1024 * 50)
                mailMessage.Attachments.Add(new Attachment(file.OpenReadStream(), file.FileName));
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("sistemaivctest@gmail.com", "IVC.12345");
                client.EnableSsl = true;
                await client.SendMailAsync(mailMessage);
            }
        }

        [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.FULL_ACCESS)]
        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheets = workBook.Worksheets
                        .Where(x => x.Name == "ENVIADAS" || x.Name == "RECIBIDAS")
                        .ToList();
                    var counter = 2;
                    var project = await _context.Projects.FirstOrDefaultAsync();
                    var letters = new List<Letter>();
                    var issuerTargets = new List<IssuerTarget>();
                    var interestGroups = new List<InterestGroup>();
                    foreach(var workSheet in workSheets)
                    {
                        counter = 2;
                        while (!workSheet.Cell($"A{counter}").IsEmpty())
                        {
                            var letter = new Letter();
                            letter.Type = workSheet.Name == "ENVIADAS"
                                ? ConstantHelpers.Letter.Type.SENT : ConstantHelpers.Letter.Type.RECEIVED;
                            letter.Name = workSheet.Cell($"B{counter}").GetString();
                            letter.Code = workSheet.Cell($"A{counter}").CachedValue.ToString();
                            letter.Subject = workSheet.Cell($"F{counter}").GetString();
                            letter.ProjectId = GetProjectId();
                            if (!workSheet.Cell($"E{counter}").IsEmpty())
                            {
                                if (workSheet.Cell($"E{counter}").DataType == XLDataType.DateTime)
                                {
                                    try
                                    {
                                        letter.Date = workSheet.Cell($"E{counter}").GetDateTime().ToUniversalTime();
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError(e.StackTrace);
                                    }
                                }
                                else
                                {
                                    var dateTimeStr = workSheet.Cell($"E{counter}").GetString();
                                    if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                        letter.Date = date.ToUniversalTime();
                                }
                            }
                            var statusString = workSheet.Cell($"H{counter}").GetString();
                            if (!string.IsNullOrEmpty(statusString))
                            {
                                letter.LetterStatus = statusString.Split("; ")
                                        .Select(x => new LetterStatus
                                        {
                                            Status = ConstantHelpers.Letter.Status.VALUES.Where(v => v.Value.RemoveAccentMarks().ToUpper() == x.ToUpper()).Select(v => v.Key).FirstOrDefault(),
                                            Letter = letter,
                                        }).ToList();
                            }
                            
                            var groupString = workSheet.Cell($"J{counter}").GetString();
                            if (!string.IsNullOrEmpty(groupString))
                            {
                                letter.LetterInterestGroups = new List<LetterInterestGroup>();
                                foreach (var group in groupString.Split("; "))
                                {
                                    var interestGroup = await _context.InterestGroups.FirstOrDefaultAsync(x => x.Name == group)
                                        ?? interestGroups.FirstOrDefault(x => x.Name == group);
                                    if (interestGroup == null)
                                    {
                                        interestGroup = new InterestGroup
                                        {
                                            Name = group,
                                            ProjectId = project.Id
                                        };
                                        interestGroups.Add(interestGroup);
                                    }
                                    letter.LetterInterestGroups.Add(new LetterInterestGroup
                                    {
                                        Letter = letter,
                                        InterestGroup = interestGroup
                                    });
                                }
                            }
                            var issuerTargetString = workSheet.Cell($"C{counter}").GetString();
                            if (!string.IsNullOrEmpty(issuerTargetString))
                            {
                                letter.LetterIssuerTargets = new List<LetterIssuerTarget>();
                                if(workSheet.Name == "ENVIADAS")
                                {
                                    foreach (var receiver in issuerTargetString.Split("; "))
                                    {
                                        var issuerTarget = await _context.IssuerTargets.FirstOrDefaultAsync(x => x.Acronym == receiver)
                                            ?? issuerTargets.FirstOrDefault(x => x.Acronym == receiver);
                                        if (issuerTarget == null)
                                        {
                                            issuerTarget = new IssuerTarget
                                            {
                                                Acronym = receiver,
                                                Name = workSheet.Cell($"D{counter}").GetString()
                                            };
                                            issuerTargets.Add(issuerTarget);
                                        }
                                        letter.LetterIssuerTargets.Add(new LetterIssuerTarget
                                        {
                                            Letter = letter,
                                            IssuerTarget = issuerTarget
                                        });
                                    }
                                }
                                else
                                {
                                    var issuerTarget = await _context.IssuerTargets.FirstOrDefaultAsync(x => x.Acronym == issuerTargetString)
                                            ?? issuerTargets.FirstOrDefault(x => x.Acronym == issuerTargetString);
                                    if (issuerTarget == null)
                                    {
                                        issuerTarget = new IssuerTarget
                                        {
                                            Acronym = issuerTargetString,
                                            Name = workSheet.Cell($"D{counter}").GetString()
                                        };
                                        issuerTargets.Add(issuerTarget);
                                    }
                                    letter.Issuer = issuerTarget;
                                }
                            }
                            letters.Add(letter);
                            ++counter;
                        }
                    }
                    foreach (var workSheet in workSheets)
                    {
                        counter = 2;
                        while (!workSheet.Cell($"A{counter}").IsEmpty())
                        {
                            var code = workSheet.Cell($"A{counter}").GetString();
                            var letter = letters.FirstOrDefault(x => x.Code == code);
                            var referenceString = workSheet.Cell($"G{counter}").GetString();
                            if (!string.IsNullOrEmpty(referenceString))
                            {
                                letter.References = new List<LetterLetter>();
                                foreach (var name in referenceString.Split("; "))
                                {
                                    var reference = await _context.Letters.FirstOrDefaultAsync(l => l.Name == name)
                                        ?? letters.FirstOrDefault(l => l.Name == name);
                                    if (reference != null)
                                    {
                                        letter.References.Add(new LetterLetter
                                        {
                                            Reference = reference,
                                            Letter = letter
                                        });
                                    }
                                }
                            }
                            ++counter;
                        }
                    }
                    await _context.InterestGroups.AddRangeAsync(interestGroups);
                    await _context.IssuerTargets.AddRangeAsync(issuerTargets);
                    await _context.Letters.AddRangeAsync(letters);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.FULL_ACCESS)]
        [HttpPost("importar-archivos")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> ImportFiles(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var archive = new ZipArchive(mem))
                {
                    var entries = archive.Entries.Where(x => !string.IsNullOrEmpty(x.Name)).ToList();
                    foreach (var entry in entries)
                    {
                        var storage = new CloudStorageService(_storageCredentials);
                        var letter = await _context.Letters
                            .Where(x => x.Type == ConstantHelpers.Letter.Type.SENT)
                            .FirstOrDefaultAsync(x => !string.IsNullOrEmpty(x.Code) && entry.Name.Contains(x.Code));
                        if (letter != null && letter.FileUrl == null)
                        {
                            letter.FileUrl = await storage.UploadFile(entry.Open(),
                                ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL, System.IO.Path.GetExtension(entry.Name), ConstantHelpers.Storage.Blobs.LETTERS_SENT, letter.Code);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.FULL_ACCESS)]
        [HttpGet("exportar")]
        public async Task<IActionResult> ExcelReport()
        {

            SqlParameter letterTypeParam = new SqlParameter("@LetterType", ConstantHelpers.Letter.Type.SENT);
            SqlParameter projectIdParam = new SqlParameter("@ProjectId", GetProjectId());

            var letters = await _context.Set<UspLetter>().FromSqlRaw("execute DocumentaryControl_uspLetters @LetterType, @ProjectId"
                , letterTypeParam, projectIdParam)
                .IgnoreQueryFilters()
                .ToListAsync();



            using (XLWorkbook wb = new XLWorkbook())
            {

                var ws = wb.Worksheets.Add("Cartas Enviadas");

                var count = 1;
                //ws.Cell($"A{count}").Value = "Proveedor";
                ws.Cell($"A{count}").Value = "Nombre";
                ws.Range($"A{count}:A{count + 1}").Merge();

                ws.Cell($"B{count}").Value = "Asunto";
                ws.Range($"B{count}:B{count + 1}").Merge();


                ws.Cell($"C{count}").Value = "Fecha";
                ws.Range($"C{count}:C{count + 1}").Merge();


                ws.Cell($"D{count}").Value = "Emisor";
                ws.Range($"D{count}:D{count + 1}").Merge();


                ws.Cell($"E{count}").Value = "Referencia";
                ws.Range($"E{count}:E{count + 1}").Merge();


                ws.Cell($"F{count}").Value = "Características del Documento";
                ws.Range($"F{count}:F{count + 1}").Merge();

                ws.Cell($"G{count}").Value = "Áreas de Interés";
                ws.Range($"G{count}:G{count + 1}").Merge();

                ws.Cell($"H{count}").Value = "Responsable Directo";
                ws.Range($"H{count}:H{count + 1}").Merge();

                ws.Cell($"I{count}").Value = "Respuestas";
                ws.Range($"I{count}:I{count + 1}").Merge();



                SetRowBorderStyle2(ws, count, "I");
                SetRowBorderStyle2(ws, count + 1, "I");
                ws.Row(count).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(count + 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Range($"A{count}:I{count}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));
                ws.Range($"A{count + 1}:I{count + 1}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));


                count = 3;
                //ws.Column(8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //ws.Column(8).Style.NumberFormat.Format = "d-mm-yy";

                foreach (var first in letters)
                {
                    ws.Cell($"A{count}").Value = first.Name;
                    ws.Cell($"B{count}").Value = first.Subject;
                    ws.Cell($"C{count}").Value = first.LetterDateStr;
                    ws.Cell($"D{count}").Value = first.IssuerName;
                    ws.Cell($"E{count}").Value = first.ReferenceLetters;
                    ws.Cell($"F{count}").Value = first.DocCharacteristics;
                    ws.Cell($"G{count}").Value = first.InterestGroups;
                    ws.Cell($"H{count}").Value = first.Responsable;
                    ws.Cell($"I{count}").Value = first.HasAnswers;




                    count++;
                    SetRowBorderStyle2(ws, count - 1, "I");

                }


                ws.Column(1).Width = 15;
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 15;

                ws.Rows().AdjustToContents();






                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de Cartas Enviadas.xlsx");
                }

            }

        }

        private void SetRowBorderStyle(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Range($"B{rowCount}:{v}{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range($"B{rowCount}:{v}{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private void SetRowBorderStyle2(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }


    }
}