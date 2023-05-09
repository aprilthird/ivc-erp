using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.MeteredsRestatedByStreetchViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/metrados-replanteo-por-tramo")]
    public class MeteredsRestatedByStreetchController : BaseController
    {
        public MeteredsRestatedByStreetchController(IvcDbContext context,
           ILogger<MeteredsRestatedByStreetchController> logger) : base(context, logger)
        {

        }

        public IActionResult Index()
        {
            return View();
        }


        //Get All con bugettitleidnull,projectformulaidnull,workfrontidnnunll,sewergrupidnnull
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? budgetTitleId = null, Guid? projectFormulaId = null, Guid? workFrontId = null, Guid? sewerGroupId = null)
        {
            var pId = GetProjectId();

            var query = await _context.Set<UspMeteredsRestatedByStretch>().FromSqlRaw("execute TechnicalOffice_uspMeteredsRestatedByStretch")
    .IgnoreQueryFilters()
    .ToListAsync();

            query = query.Where(x => x.ProjectId == pId).ToList();

            /*var query = await _context.MeteredsRestatedByStreetchs
                .Include(x => x.WorkFront)
                .Include(x => x.SewerGroup)
                .Include(x => x.ProjectFormula)
                .OrderBy(x => x.ItemNumber)
                .Where(x => x.ProjectFormula.ProjectId == pId)
                .Select(x => new MeteredsRestatedByStreetchViewModel
                {
                    Id = x.Id,
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    //Metered=(x.Metered!=0)?x.Metered.ToString():"",
                    Metered = x.Metered.ToString(),
                    Unit = x.Unit,
                    WorkFrontId = x.WorkFrontId.Value,
                    ProjectFormulaId = x.ProjectFormulaId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    SewerGroupId = x.SewerGroupId.Value,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    },
                    BudgetTittleId = x.BudgetTittleId
                }).ToListAsync();*/

            if (budgetTitleId.HasValue)
            {
                query = query.Where(x => x.BudgetTitleId == budgetTitleId.Value).ToList();
            }

            if (workFrontId.HasValue)
            {
                query = query.Where(x => x.WorkFrontId == workFrontId.Value).ToList();
            }
            if (projectFormulaId.HasValue)
            {
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId.Value).ToList();
            }
            if (sewerGroupId.HasValue)
            {
                query = query.Where(x => x.SewerGroupId == sewerGroupId.Value).ToList();
            }




            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid? id)
        {
            var pId = GetProjectId();

            var query = await _context.MeteredsRestatedByStreetchs
                .Include(x => x.WorkFront)
                .Include(x => x.SewerGroup)
                .Include(x => x.ProjectFormula)
                .Include(x => x.BudgetTitle)
                .Select(x => new MeteredsRestatedByStreetchViewModel
                {
                    Id = x.Id,
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Metered = x.Metered.ToString(),
                    Unit = x.Unit,
                    WorkFrontId = x.WorkFrontId.Value,
                    SewerGroupId = x.SewerGroupId.Value,
                    BudgetTitleId = x.BudgetTitleId,
                    ProjectFormulaId = x.ProjectFormulaId

                }).FirstOrDefaultAsync(x => x.Id == id);


            return Ok(query);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, MeteredsRestatedByStreetchViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var metrado = await _context.MeteredsRestatedByStreetchs
                .FirstOrDefaultAsync(x => x.Id == id);

            metrado.ItemNumber = model.ItemNumber;
            metrado.Description = model.Description;
            metrado.Metered = model.Metered.ToDoubleString();
            metrado.Unit = model.Unit;
            metrado.WorkFrontId = model.WorkFrontId;

            metrado.SewerGroupId = model.SewerGroupId;

            metrado.ProjectFormulaId = model.ProjectFormulaId;
            metrado.BudgetTitleId = model.BudgetTitleId;

            

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var metrado = await _context.MeteredsRestatedByStreetchs
                .FirstOrDefaultAsync(x => x.Id == id);

            if (metrado == null)
                return BadRequest("No se encontró el metrado replanteado");

            _context.MeteredsRestatedByStreetchs.Remove(metrado);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("importar")]
        public async Task<IActionResult> ImportData(Guid formulaId, Guid titleId, IFormFile file, MeteredsRestatedByStreetchViewModel model)
        {
            var aux = _context.MeteredsRestatedByStreetchs.Count();
            var metereds = new List<MeteredsRestatedByStreetch>();

            var workFronts = await _context.WorkFronts.
                Where(x => x.ProjectId == GetProjectId())
                .AsNoTracking().ToListAsync();

            var sewerGroups = await _context.SewerGroups.
                Where(x => x.ProjectId == GetProjectId())
                .AsNoTracking().ToListAsync();

            var metrados = await _context.MeteredsRestatedByStreetchs.AsNoTracking().ToListAsync();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;
                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var item = new MeteredsRestatedByStreetch();
                        var itemNumberExcel = workSheet.Cell($"B{counter}").GetString();
                        var existeItem = metrados
                            .FirstOrDefault(x => x.ItemNumber == itemNumberExcel);

                        var descriptionExcel = workSheet.Cell($"C{counter}").GetString();
                        var unitExcel = workSheet.Cell($"D{counter}").GetString();
                        var meteredExcel = workSheet.Cell($"E{counter}").GetString();
                        var workFrontExcel = workFronts.FirstOrDefault(x => x.Code == workSheet.Cell($"F{counter}").GetString());
                        var sewerGroupExcel = sewerGroups.FirstOrDefault(x => x.Code == workSheet.Cell($"G{counter}").GetString());

                        if (existeItem == null)
                        {
                            item.Id = Guid.NewGuid();
                            item.ProjectFormulaId = model.ProjectFormulaId;
                            item.BudgetTitleId = model.BudgetTitleId;
                            item.ProjectId = GetProjectId();

                            item.ItemNumber = workSheet.Cell($"B{counter}").GetString();
                            item.Description = descriptionExcel;
                            item.Unit = unitExcel;
                            item.Metered = meteredExcel.ToDoubleString();
                            //item.WorkFrontId = workFrontExcel.Id;


                           
                                if (workFrontExcel == null)
                                    return BadRequest($"El Frente de Trabajo de la celda F{counter} no existe");
                                else
                                    item.WorkFrontId = workFrontExcel.Id;
                            
                            /*if(item.Unit=="" && item.Metered == 0)
                            {
                                //item.workFrontId=
                            }*/

                            if (sewerGroupExcel != null)
                                item.SewerGroupId = sewerGroupExcel.Id;

                            metereds.Add(item);
                        }
                        counter++;
                        aux++;
                    }

                }

                mem.Close();

            }

            await _context.MeteredsRestatedByStreetchs.AddRangeAsync(metereds);
            await _context.SaveChangesAsync();
            return Ok();

        }
        //carga masiva
        [HttpGet("excel-carga-masiva")]
        public async Task<FileResult> ExportExcelMassiveLoad()
        {
            string Name = "MetradoReplanteoPorPartida.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var pId = GetProjectId();
                var workSheet = wb.Worksheets.Add("CargaMasiva");
                workSheet.Cell($"B2").Value = "Item";
                workSheet.Cell("B2").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);
               
                workSheet.Cell($"C2").Value = "Descripción";
                workSheet.Cell("C2").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);

                workSheet.Cell($"D2").Value = "Unidad";
                workSheet.Cell("D2").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);

                workSheet.Cell($"E2").Value = "Metrado";
                workSheet.Cell("E2").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);

                workSheet.Cell($"F2").Value = "Frente de trabajo";
                workSheet.Cell("F2").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);

                workSheet.Cell($"G2").Value = "Cuadrilla";
                workSheet.Cell("G2").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);

                workSheet.Range("B2:G3").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:G3").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 13;
                workSheet.Column(3).Width = 67;
                workSheet.Column(4).Width = 13;
                workSheet.Column(5).Width = 17;
                workSheet.Column(6).Width = 25;
                workSheet.Column(7).Width = 25;

                var budgetList = await _context.BudgetTitles.Where(x => x.ProjectId == pId).ToListAsync();
                var formulaList = await _context.ProjectFormulas.Where(x => x.ProjectId == pId).ToListAsync();
                var frontList = await _context.WorkFronts.Where(x => x.ProjectId == pId).ToListAsync();
                var specList = await _context.Specialities.Where(x => x.ProjectId == pId).ToListAsync();
                var versionList = await _context.TechnicalVersions.Where(x => x.ProjectId == pId).ToListAsync();

                //TITULOS DE PRESUPUESTO

                DataTable dtBT = new DataTable();
                dtBT.TableName = "Tìtulos de Presupuesto";
                dtBT.Columns.Add("Titulos de Presupuesto", typeof(string));
                var budgets = budgetList;
                foreach (var item in budgets)
                    dtBT.Rows.Add(item.Name);
                dtBT.AcceptChanges();

                var worksheetBT = wb.Worksheets.Add(dtBT);
                worksheetBT.Column(1).Width = 50;

                //FORMULAS

                DataTable dtPF = new DataTable();
                dtPF.TableName = "Fórmulas";
                dtPF.Columns.Add("Fórmulas", typeof(string));
                var formulas = formulaList;
                foreach (var item in formulas)
                    dtPF.Rows.Add(item.Name);
                dtPF.AcceptChanges();

                var worksheetPF = wb.Worksheets.Add(dtPF);
                worksheetPF.Column(1).Width = 45;

                //FRENTES

                DataTable dtWF = new DataTable();
                dtWF.TableName = "Frentes";
                dtWF.Columns.Add("Frentes", typeof(string));
                var fronts = frontList;
                foreach (var item in fronts)
                    dtWF.Rows.Add(item.Code);
                dtWF.AcceptChanges();

                var worksheetWF = wb.Worksheets.Add(dtWF);
                worksheetWF.Column(1).Width = 30;

                //ESPECIALIDADES
                /*DataTable dtSP = new DataTable();
                dtSP.TableName = "Especialidades";
                dtSP.Columns.Add("Especialidades", typeof(string));
                var specs = specList;
                foreach (var item in specs)
                    dtSP.Rows.Add(item.Description);
                dtSP.AcceptChanges();

                var worksheetSP = wb.Worksheets.Add(dtSP);
                worksheetSP.Column(1).Width = 40;*/

                //VERSIONES

                /*DataTable dtTV = new DataTable();
                dtTV.TableName = "Versiones";
                dtTV.Columns.Add("Versiones", typeof(string));
                var vers = versionList;
                foreach (var item in vers)
                    dtTV.Rows.Add(item.Description);
                dtTV.AcceptChanges();

                var worksheetTV = wb.Worksheets.Add(dtTV);
                worksheetTV.Column(1).Width = 30;*/

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Name);
                }
            }
        }

        [HttpDelete("eliminar-filtro")]
        public async Task <IActionResult> DeleteByFilter(Guid? projectFormulaId=null,Guid? budgetTitleId=null)
        {
            var pId = GetProjectId();

            var query = _context.MeteredsRestatedByStreetchs.Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId==pId);

            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            else
                return BadRequest("No se ha escogido el título de presupuesto");

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            else
                return BadRequest("No se ha escodigo la fórmula");

            var data = await query.ToListAsync();

            _context.MeteredsRestatedByStreetchs.RemoveRange(data);
            await _context.SaveChangesAsync();


            return Ok();
        }
    

       
    }

    


    
}
