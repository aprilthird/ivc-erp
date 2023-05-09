using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.WorkbookViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.WorkbookSeatViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.DocumentaryControl.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.DOCUMENTARY_CONTROL)]
    [Route("control-documentario/cuadernos-de-obra")]
    public class WorkbookSeatController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public WorkbookSeatController(IvcDbContext context,
            ILogger<WorkbookSeatController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("autores")]
        public async Task<IActionResult> GetWriters(int? page = null, int rowsPerPage = 5)
        {
            var projectId = GetProjectId();

            var query = _context.IssuerTargets.AsQueryable();
            if (page.HasValue)
                query = query.Skip(page.Value * rowsPerPage).Take(rowsPerPage);
            var model = ConstantHelpers.Workbook.WroteBy
                .VALUES.Select(x => new WriterSummaryViewModel
                {
                    Id = x.Key,
                    Name = x.Value,
                    TotalCount = _context.WorkbookSeats
                        .Include(x => x.Workbook)
                        .Where(x => x.Workbook.ProjectId == projectId)
                        .Count(w => w.WroteBy == x.Key)
                }).ToList();
            return Ok(model);
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int? wroteBy = null, int? type = null, Guid? workbookId = null)
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);
            var orderNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.ORDER_COLUMN]);
            var orderDirection = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.ORDER_DIRECTION].ToString().ToUpper();

            var query = _context.WorkbookSeats
                .AsNoTracking()
                .AsQueryable();

            var projectId = GetProjectId();
            query = query.Where(x => x.Workbook.ProjectId == projectId);

            if (orderNumber == 0)
                query = orderDirection == ConstantHelpers.Datatable.ServerSide.Default.ORDER_DIRECTION
                    ? query.OrderByDescending(x => x.Workbook.Number) : query.OrderBy(x => x.Workbook.Number);
            if (orderNumber == 1)
                query = orderDirection == ConstantHelpers.Datatable.ServerSide.Default.ORDER_DIRECTION
                    ? query.OrderByDescending(x => x.Number) : query.OrderBy(x => x.Number);
            if (orderNumber == 2)
                query = orderDirection == ConstantHelpers.Datatable.ServerSide.Default.ORDER_DIRECTION
                    ? query.OrderByDescending(x => x.Date) : query.OrderBy(x => x.Date);

            if (wroteBy.HasValue)
                query = query.Where(x => x.WroteBy == wroteBy.Value);
            if (type.HasValue)
                query = query.Where(x => x.Type == type.Value);
            if (workbookId.HasValue)
                query = query.Where(x => x.WorkbookId == workbookId.Value);

            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Subject.Contains(search) ||
                   x.Number.ToString().Contains(search));

            var data = await query
                .Skip(currentNumber)
                .Take(recordsPerPage)
                .Select(x => new WorkbookSeatViewModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    Subject = x.Subject,
                    FileUrl = x.FileUrl,
                    Date = x.Date.ToDateString(),
                    ResponseDate = x.ResponseDate.HasValue
                        ? x.ResponseDate.Value.ToDateString() : string.Empty,
                    ResponseTermDays = x.ResponseDate.HasValue
                        ? (x.ResponseDate.Value - x.Date).Days : null as int?,
                    WorkbookId = x.WorkbookId,
                    WorkbookTypeId = x.WorkbookTypeId,
                    WorkbookType = new WorkbookTypeViewModel
                    {
                        Description = x.WorkbookType.Description,
                        Color = x.WorkbookType.PillColor
                    },
                    Type = x.Type,
                    Status = x.Status,
                    WroteBy = x.WroteBy,
                    Detail = x.Detail,
                    Workbook = new WorkbookViewModel
                    {
                        Number = x.Workbook.Number
                    }
                }).ToListAsync();

            return Ok(new
            {
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = await query.CountAsync(),
                data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLetter(Guid id)
        {
            var model = await _context.WorkbookSeats
                .Where(x => x.Id == id)
                .Select(x => new WorkbookSeatViewModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    Subject = x.Subject,
                    FileUrl = x.FileUrl,
                    Date = x.Date.ToDateString(),
                    WorkbookId = x.WorkbookId,
                    WorkbookTypeId = x.WorkbookTypeId,
                    Status = x.Status,
                    WroteBy = x.WroteBy,
                    Detail = x.Detail
                }).FirstOrDefaultAsync();
            return Ok(model);
        }

        [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkbookSeatViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workbookSeat = new WorkbookSeat
            {
                Number = model.Number,
                Subject = model.Subject,
                Date = model.Date.ToDateTime(),
                WorkbookTypeId = model.WorkbookTypeId,
                Status = model.Status,
                WroteBy = model.WroteBy,
                WorkbookId = model.WorkbookId,
                Detail = model.Detail
            };
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                workbookSeat.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL, System.IO.Path.GetExtension(model.File.FileName), ConstantHelpers.Storage.Blobs.WORKBOOK_SEATS, workbookSeat.Number.ToString());
            }
            await _context.WorkbookSeats.AddAsync(workbookSeat);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkbookSeatViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var workbookSeat = await _context.WorkbookSeats
                .FirstOrDefaultAsync(x => x.Id == id);

            if(workbookSeat.Status == ConstantHelpers.Workbook.Status.PENDING
                && model.Status != ConstantHelpers.Workbook.Status.PENDING)
            {
                if ((workbookSeat.Type == ConstantHelpers.Workbook.Type.QUERY
                    && model.Type == ConstantHelpers.Workbook.Type.QUERY_RESOLUTION)
                    || (workbookSeat.Type == ConstantHelpers.Workbook.Type.INFORMATION_REQUEST
                    && model.Type == ConstantHelpers.Workbook.Type.INFORMATION_DELIVERY))
                    workbookSeat.ResponseDate = DateTime.UtcNow;
            }

            workbookSeat.Number = model.Number;
            workbookSeat.Subject = model.Subject;
            workbookSeat.Type = model.Type;
            workbookSeat.Status = model.Status;
            workbookSeat.Date = model.Date.ToDateTime();
            workbookSeat.WroteBy = model.WroteBy;
            workbookSeat.WorkbookId = model.WorkbookId;
            workbookSeat.Detail = model.Detail;
            workbookSeat.WorkbookTypeId = model.WorkbookTypeId;
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (workbookSeat.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.WORKBOOK_SEATS}/{workbookSeat.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
                workbookSeat.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL, System.IO.Path.GetExtension(model.File.FileName), ConstantHelpers.Storage.Blobs.WORKBOOK_SEATS, workbookSeat.Number.ToString());
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workbookSeat = await _context.WorkbookSeats
                .FirstOrDefaultAsync(x => x.Id == id);
            if (workbookSeat == null)
                return BadRequest($"Cuaderno de Obra con Id '{id}' no encontrada.");
            if (workbookSeat.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.WORKBOOK_SEATS}/{workbookSeat.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
            }
            _context.WorkbookSeats.Remove(workbookSeat);
            await _context.SaveChangesAsync();
            return Ok();
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
                    var workSheet = workBook.Worksheets.FirstOrDefault(x => x.Name == "CUADERNOS DE OBRA");
                    var counter = 2;
                    var workbooks = new List<Workbook>();
                    var workbookSeats = new List<WorkbookSeat>();
                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var workbookSeat = new WorkbookSeat();
                        if(!workSheet.Cell($"A{counter}").IsEmpty())
                        {
                            var workbookNumber = (int)workSheet.Cell($"A{counter}").GetDouble();
                            var workbook = await _context.Workbooks.FirstOrDefaultAsync(x => x.Number == workbookNumber)
                                ?? workbooks.FirstOrDefault(x => x.Number == workbookNumber);
                            if(workbook == null)
                            {
                                workbook = new Workbook()
                                {
                                    Number = workbookNumber
                                };
                                workbooks.Add(workbook);
                            }
                            workbookSeat.Workbook = workbook;
                        }
                        workbookSeat.Number = (int)workSheet.Cell($"B{counter}").GetDouble();
                        workbookSeat.Subject = workSheet.Cell($"E{counter}").GetString();
                        workbookSeat.WroteBy = ConstantHelpers.Workbook.WroteBy.VALUES
                            .Where(v => v.Value.RemoveAccentMarks().ToUpper() == workSheet.Cell($"C{counter}").GetString().ToUpper())
                            .Select(v => v.Key).FirstOrDefault();
                        workbookSeat.Type = ConstantHelpers.Workbook.Type.VALUES
                            .Where(v => v.Value.RemoveAccentMarks().ToUpper() == workSheet.Cell($"F{counter}").GetString().ToUpper())
                            .Select(v => v.Key).FirstOrDefault();
                        if (!workSheet.Cell($"D{counter}").IsEmpty())
                        {
                            if (workSheet.Cell($"D{counter}").DataType == XLDataType.DateTime)
                            {
                                try
                                {
                                    workbookSeat.Date = workSheet.Cell($"D{counter}").GetDateTime().ToUniversalTime();
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(e.StackTrace);
                                }
                            }
                            else
                            {
                                var dateTimeStr = workSheet.Cell($"D{counter}").GetString();
                                if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                    workbookSeat.Date = date.ToUniversalTime();
                            }
                        }
                        workbookSeats.Add(workbookSeat);
                        ++counter;
                    }
                    await _context.Workbooks.AddRangeAsync(workbooks);
                    await _context.WorkbookSeats.AddRangeAsync(workbookSeats);
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
                        var names = entry.FullName.Split("_");
                        var workbookName = names[1].Split(".");
                        var workbookSeatName = workbookName[0];
                        var workbookSeat = await _context.WorkbookSeats
                            .FirstOrDefaultAsync(x => x.Number.ToString() == workbookSeatName);

                        if(workbookSeat != null && workbookSeat.FileUrl != null)
                        {
                            await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.WORKBOOK_SEATS}/{workbookSeat.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
                            workbookSeat.FileUrl = await storage.UploadFile(entry.Open(),
                                ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL, System.IO.Path.GetExtension(entry.Name), ConstantHelpers.Storage.Blobs.WORKBOOK_SEATS, workbookSeat.Number.ToString());
                            await _context.SaveChangesAsync();
                        }
                        if (workbookSeat != null && workbookSeat.FileUrl == null)
                        {
                            workbookSeat.FileUrl = await storage.UploadFile(entry.Open(),
                                ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL, 
                                System.IO.Path.GetExtension(entry.Name), 
                                ConstantHelpers.Storage.Blobs.WORKBOOK_SEATS, workbookSeat.Number.ToString());
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

            var query = _context.WorkbookSeats
                .Include(x=>x.Workbook)
                .Include(x => x.WorkbookType)
                .AsNoTracking()
                .AsQueryable();

            var projectId = GetProjectId();
            query = query.Where(x => x.Workbook.ProjectId == projectId);

            var data = await query
            .Select(x => new WorkbookSeatViewModel
            {
                Id = x.Id,
                Number = x.Number,
                Subject = x.Subject,
                FileUrl = x.FileUrl,
                Date = x.Date.ToDateString(),
                ResponseDate = x.ResponseDate.HasValue
                    ? x.ResponseDate.Value.ToDateString() : string.Empty,
                ResponseTermDays = x.ResponseDate.HasValue
                    ? (x.ResponseDate.Value - x.Date).Days : null as int?,
                WorkbookId = x.WorkbookId,
                WorkbookTypeId = x.WorkbookTypeId,
                WorkbookType = new WorkbookTypeViewModel
                {
                    Description = x.WorkbookType.Description,
                    Color = x.WorkbookType.PillColor
                },
                Type = x.Type,
                Status = x.Status,
                WroteBy = x.WroteBy,
                Detail = x.Detail,
                Workbook = new WorkbookViewModel
                {
                    Number = x.Workbook.Number
                }
            }).ToListAsync();

            using (XLWorkbook wb = new XLWorkbook())
            {

                var ws = wb.Worksheets.Add("Cuadernos de obra");

                var count = 1;
                //ws.Cell($"A{count}").Value = "Proveedor";
                ws.Cell($"A{count}").Value = "Número";
                ws.Range($"A{count}:A{count + 1}").Merge();

                ws.Cell($"B{count}").Value = "Asiento";
                ws.Range($"B{count}:B{count + 1}").Merge();


                ws.Cell($"C{count}").Value = "Asunto";
                ws.Range($"C{count}:C{count + 1}").Merge();


                ws.Cell($"D{count}").Value = "Fecha";
                ws.Range($"D{count}:D{count + 1}").Merge();


                ws.Cell($"E{count}").Value = "Tipo";
                ws.Range($"E{count}:E{count + 1}").Merge();


                ws.Cell($"F{count}").Value = "Escribe";
                ws.Range($"F{count}:F{count + 1}").Merge();

               



                SetRowBorderStyle2(ws, count, "F");
                SetRowBorderStyle2(ws, count + 1, "F");
                ws.Row(count).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(count + 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Range($"A{count}:F{count}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));
                ws.Range($"A{count + 1}:F{count + 1}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));


                count = 3;
                //ws.Column(8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //ws.Column(8).Style.NumberFormat.Format = "d-mm-yy";

                foreach (var first in data)
                {
                    ws.Cell($"A{count}").Value = first.Workbook.Number;
                    ws.Cell($"B{count}").Value = first.Number;
                    ws.Cell($"C{count}").Value = first.Subject;
                    ws.Cell($"D{count}").Value = first.Date;
                    ws.Cell($"E{count}").Value = first.WorkbookType.Description;
                    switch(first.WroteBy)
                    {
                        case 1: ws.Cell($"F{count}").Value = "Contratista";
                            break;
                        case 2: ws.Cell($"F{count}").Value = "Supervisión";
                            break;
                    }





                    count++;
                    SetRowBorderStyle2(ws, count - 1, "F");

                }


                ws.Column(1).Width = 15;
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
    

                ws.Rows().AdjustToContents();






                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de Cuadernos de Obra.xlsx");
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