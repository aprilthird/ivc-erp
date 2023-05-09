using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.UspModels.WareHouse;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.FieldRequestViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/atencion-pedidos-campo")]
    public class AttendFieldRequestController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public AttendFieldRequestController(IvcDbContext context, 
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials,
               ILogger<AttendFieldRequestController> logger) 
            : base(context, userManager, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? budgetTitleId = null, Guid? projectFormulaId = null,
           Guid? workFrontId = null, Guid? sewerGroupId = null, Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            var foldings = await _context.FieldRequestFoldings
                .Include(x => x.GoalBudgetInput.Supply)
                .Include(x => x.GoalBudgetInput.Supply.SupplyGroup)
                .ToListAsync();

            var query = _context.FieldRequests
                .Include(x => x.BudgetTitle)
                //.Include(x => x.ProjectFormula)
                .Include(x => x.WorkFront)
                //.Include(x => x.Warehouse)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.SewerGroup)
                .Include(x => x.SewerGroup.WorkFrontHead.User)
                .Where(x => x.WorkFront.ProjectId == GetProjectId())
                //.Where(x => x.Status == ConstantHelpers.Warehouse.FieldRequest.Status.VALIDATED
                .Where(x => x.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ISSUED
                || x.Status == ConstantHelpers.Warehouse.FieldRequest.Status.READYTOATTEND 
                || x.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED);

            var formulas = await _context.FieldRequestProjectFormulas
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == GetProjectId())
                .ToListAsync();

            var users = await _context.Users.ToListAsync();

            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);

            //if (projectFormulaId.HasValue)
              //  query = query.Where(x => x.ProjectFormulaId == projectFormulaId);

            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);

            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerGroupId == sewerGroupId);

            //if (supplyFamilyId.HasValue)
            //    query = query.Where(x => x.SupplyFamilyId == supplyFamilyId);

            var data = new List<FieldRequestViewModel>();

            foreach (var item in query)
            {
                var folding = foldings.Where(x => x.FieldRequestId == item.Id).ToList();
                var grupo = folding.Where(x => x.GoalBudgetInput.Supply.SupplyGroupId == supplyGroupId);

                if (supplyGroupId.HasValue && grupo == null)
                    continue;

                data.Add(new FieldRequestViewModel
                {
                    Id = item.Id,
                    DocumentNumber = item.DocumentNumber.ToString("D4"),
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = item.BudgetTitle.Name
                    },
                    DeliveryDate = item.DeliveryDate.ToDateString(),
                    //ProjectFormula = new ProjectFormulaViewModel
                    //{
                    //    Name = item.ProjectFormula.Code + " - " + item.ProjectFormula.Name
                    //},
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = item.WorkFront.Code
                    },
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = item.SewerGroup.Code
                    },
                    //Warehouse = new WarehouseViewModel
                    //{
                    //    Address = item.Warehouse.Address
                    //},
                    //SupplyFamily = new SupplyFamilyViewModel
                    //{
                    //    Name = item.SupplyFamily.Name
                    //},
                    Groups = string.Join("-",
                   folding.GroupBy(x => x.GoalBudgetInput.Supply.SupplyGroupId)
                    .Select(y => y.FirstOrDefault().GoalBudgetInput.Supply.SupplyGroup.Name)),
                    Formulas = string.Join("/", formulas.Where(x => x.FieldRequestId == item.Id)
                    .Select(x => x.ProjectFormula.Code).ToList()),
                    UserName = users.FirstOrDefault(x => x.Id == item.IssuedUserId).FullName,
                    Status = item.Status
                });
            }
            return Ok(data);
        }

        [HttpPut("listo/{id}")]
        public async Task<IActionResult> UpdateStatusReadyToAttend(Guid id, FieldRequestViewModel model)
        {
            var request = await _context.FieldRequests.FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return BadRequest("No se ha encontrado el requerimiento de campo.");

            var items = await _context.FieldRequestFoldings
                .Where(x => x.FieldRequestId == id)
                .ToListAsync();

            request.Status = ConstantHelpers.Warehouse.FieldRequest.Status.READYTOATTEND;

            foreach (var stringItem in model.Items)
            {
                var stringSplit = stringItem.Split("|");

                var itemId = stringSplit[0];
                var itemMeasure = stringSplit[1];

                if (itemId == "undefined" || itemMeasure == "undefined")
                    return BadRequest("Quite lo filtros de la tabla de insumos para guardar");

                var item = items.FirstOrDefault(x => x.Id == Guid.Parse(itemId));

                if (itemMeasure.ToDoubleString() > item.ValidatedQuantity)
                    return BadRequest("Se ha superado el metrado validado");

                item.DeliveredQuantity = itemMeasure.ToDoubleString();
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("cargar-guia/{id}")]
        public async Task<IActionResult> LoadPdf(Guid id, FieldRequestViewModel model)
        {
            var request = await _context.FieldRequests.FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return BadRequest("No se ha encontrado el requerimiento de campo");

            //if (request.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED)
              //  return BadRequest("El pedido de campo ya fue atendido.");

            //if (request.FileUrl == null)
              //  return BadRequest("No se ha cargado la guía escaneada");

            var items = await _context.FieldRequestFoldings
                .Include(x => x.GoalBudgetInput)
                .Include(x => x.GoalBudgetInput.Supply)
                .Where(x => x.FieldRequestId == id)
                .ToListAsync();

            var stock = await _context.Stocks
                .ToListAsync();

            var meta = await _context.GoalBudgetInputs
                .ToListAsync();

            foreach (var item in items)
            {
                var stc = stock.FirstOrDefault(x => x.SupplyId == item.GoalBudgetInput.SupplyId);

                var insumo = meta.FirstOrDefault(x => x.Id == item.GoalBudgetInputId);

                if (stc == null)
                    return BadRequest("El item " + item.GoalBudgetInput.Supply.Description + " no se encuentra en stock");

                if (item.DeliveredQuantity > stc.Measure)
                    return BadRequest("Se ha superado el stock disponible del insumo " + item.GoalBudgetInput.Supply.Description);

                if (item.DeliveredQuantity > insumo.WarehouseCurrentMetered)
                    return BadRequest("Se ha superado el metrado disponible en meta " + item.GoalBudgetInput.Supply.Description);

                stc.Measure -= item.DeliveredQuantity;
                insumo.WarehouseCurrentMetered -= item.DeliveredQuantity;
            }

            request.Status = ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (request.FileUrl != null)
                    await storage.TryDelete(request.FileUrl, ConstantHelpers.Storage.Containers.WAREHOUSE);
                request.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.WAREHOUSE, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.FIELD_REQUEST, request.DocumentNumber.ToString("D4"));
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        /*
        [HttpPut("atender/{id}")]
        public async Task<IActionResult> UpdateStatusAttend(Guid id)
        {
            var request = await _context.FieldRequests.FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return BadRequest("No se ha encontrado el requerimiento de campo");

            if (request.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED)
                return BadRequest("El pedido de campo ya fue atendido.");

            if (request.FileUrl == null)
                return BadRequest("No se ha cargado la guía escaneada");

            var items = await _context.FieldRequestFoldings
                .Include(x => x.GoalBudgetInput)
                .Include(x => x.GoalBudgetInput.Supply)
                .Where(x => x.FieldRequestId == id)
                .ToListAsync();

            var stock = await _context.Stocks
                .ToListAsync();

            var meta = await _context.GoalBudgetInputs
                .ToListAsync();

            foreach(var item in items)
            {
                var stc = stock.FirstOrDefault(x => x.SupplyId == item.GoalBudgetInput.SupplyId);

                var insumo = meta.FirstOrDefault(x => x.Id == item.GoalBudgetInputId);

                if (stc == null)
                    return BadRequest("El item " + item.GoalBudgetInput.Supply.Description + " no se encuentra en stock");

                if (item.DeliveredQuantity > stc.Measure)
                    return BadRequest("Se ha superado el stock disponible del insumo " + item.GoalBudgetInput.Supply.Description);

                if(item.DeliveredQuantity > insumo.WarehouseCurrentMetered)
                    return BadRequest("Se ha superado el metrado disponible en meta " + item.GoalBudgetInput.Supply.Description);

                stc.Measure -= item.DeliveredQuantity;
                insumo.WarehouseCurrentMetered -= item.DeliveredQuantity;
            }

            request.Status = ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED;

            await _context.SaveChangesAsync();
            return Ok();
        }
        */
        [HttpGet("vale/{id}")]
        public async Task<IActionResult> Test(Guid id)
        {
            var items = await _context.FieldRequestFoldings
                .Include(x => x.ProjectPhase)
                .Include(x => x.GoalBudgetInput.Supply)
                .Include(x => x.FieldRequest.WorkFront)
                .Include(x => x.FieldRequest.SewerGroup)
                .Include(x => x.GoalBudgetInput.MeasurementUnit)
                .Include(x => x.FieldRequest.BudgetTitle.Project)
                .Include(x => x.GoalBudgetInput.Supply.SupplyGroup)
                .Include(x => x.GoalBudgetInput.Supply.SupplyFamily)
                .Include(x => x.FieldRequest.SewerGroup.ProjectCollaborator)
                .Where(x => x.FieldRequestId == id)
                .ToListAsync();

            var request = items.FirstOrDefault().FieldRequest;

            var usuario = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.IssuedUserId);

            var formulas = await _context.FieldRequestProjectFormulas
                .Include(x => x.ProjectFormula)
                .Where(x => x.FieldRequestId == id)
                .Select(x => x.ProjectFormula.Code + " - " + x.ProjectFormula.Name)
                .ToListAsync();

            var stocks = await _context.Stocks
                .Where(x => x.ProjectId == request.BudgetTitle.ProjectId)
                .ToListAsync();

            using (var ms = new MemoryStream())
            {
                using (TextWriter tw = new StreamWriter(ms))
                {
                    int limite = 142;
                    int currentPag = 1;
                    int maxPag = (int)Math.Ceiling((double)items.Count() / 25);
                    int itemNumber = 1;
                    int aux = limite;
                    var limiteItems = 23;

                    var tLine = "";
                    var tLine2 = "";
                    var tLine3 = "";

                    var tAux = "";

                    void Cabecera()
                    {
                        tLine = "ERP-IVC";

                        tAux = " CODIGO : CSH/GAL-For-01";

                        tLine += new string(' ', limite - tAux.Length - tLine.Length);
                        tLine += tAux;

                        tw.WriteLine(tLine);

                        /// -----------------------
                        tLine = request.BudgetTitle.Project.Abbreviation;
                        tAux = "GESTIÓN DE ALMACÉN";
                        aux = limite / 2;
                        aux -= (tAux.Length / 2);
                        aux -= tLine.Length;
                        while (aux > 0)
                        {
                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        aux = limite;
                        tAux = $"VERSIÓN :       2       ";
                        aux -= tLine.Length;
                        aux -= tAux.Length;
                        while (aux > 0)
                        {
                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        tw.WriteLine(tLine);

                        /// -----------------------

                        tLine = "";

                        tAux = "  FECHA :  14/02/2022   ";

                        tLine += new string(' ', limite - tAux.Length);
                        tLine += tAux;

                        tw.WriteLine(tLine);

                        /// -----------------------
                      
                        tAux = "VALE DE PEDIDO Y ENTREGA DE MATERIALES DE ALMACEN N° "
                            + request.DocumentNumber.ToString("D6");
                        tLine = "";
                        aux = limite / 2;
                        aux -= (tAux.Length / 2);
                        while (aux > 0)
                        {

                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        tAux = $" PÁGINA :    {currentPag} de {maxPag}     ";

                        aux = limite;
                        aux -= tLine.Length;
                        aux -= tAux.Length;
                        while (aux > 0)
                        {

                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        tw.WriteLine(tLine);

                        /// -----------------------

                        tw.WriteLine();
                        tw.WriteLine("    O.T.N°           :    "
                            + request.WorkOrder);
                        tw.WriteLine("    FÓRMULA          :    "
                            + string.Join(", ", formulas));
                        tw.WriteLine("    FRENTE           :    "
                            + request.WorkFront.Code);
                        tw.WriteLine("    CUADRILLA        :    "
                            + request.SewerGroup.Code);
                        tw.WriteLine("    COLABORADOR      :    "
                            + (request.SewerGroup.ProjectCollaboratorId != null ? request.SewerGroup.ProjectCollaborator.FullName : ""));
                        tw.WriteLine("    FECHA DE EMISIÓN :    "
                            + request.IssueDate.Value.ToDateString());
                        tw.WriteLine("    FECHA DE ENTREGA :    "
                            + request.DeliveryDate.ToDateString());

                        tw.WriteLine();

                        tLine3 = " ------ ";

                        tLine = "  ITEM  ";
                        tAux = "CÓDIGO";

                        void Columna(int largo)
                        {
                            aux = largo / 2;
                            aux -= tAux.Length / 2;
                            while (aux > 0)
                            {
                                tLine += " ";
                                tLine3 += "-";
                                aux -= 1;
                            }

                            tLine += tAux;
                            tLine3 += new string('-', tAux.Length);

                            aux = largo / 2;
                            aux -= tAux.Length / 2;
                            while (aux > 0)
                            {
                                tLine += " ";
                                tLine3 += "-";
                                aux -= 1;
                            }

                            tLine += " ";
                            tLine3 += " ";
                        }

                        Columna(14);

                        tAux = "DESCRIPCIÓN";

                        Columna(70);

                        tAux = "UND";

                        Columna(9);

                        tAux = "FASE";

                        Columna(10);

                        tLine2 = new string(' ', tLine.Length + 1);

                        tAux = "CANTIDAD";
                        tLine2 += "SOLICITADA";
                        tLine2 += new string(' ', 4);

                        Columna(12);

                        tAux = "CANTIDAD";
                        tLine2 += "ENTREGADA";

                        Columna(12);

                        tw.WriteLine(tLine);
                        tw.WriteLine(tLine2);
                        tw.WriteLine(tLine3);
                    }

                    Cabecera();

                    var counter = 1;
                    var monto = 0.0;

                    foreach(var item in items)
                    {
                        monto += Math.Round(stocks
                            .FirstOrDefault(x => x.SupplyId == item.GoalBudgetInput.SupplyId)
                            .UnitPrice * item.DeliveredQuantity, 2);

                        if (counter > limiteItems)
                        {
                            tLine3 = " ";
                            tLine3 += new string('-', limite - 2);
                            tw.WriteLine(tLine3);
                            limiteItems += limiteItems;
                            currentPag++;
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            Cabecera();
                        }

                        var isLong = false;
                        tLine = "  ";
                        tLine += itemNumber.ToString("D4");
                        tLine += "   ";
                        tLine += item.GoalBudgetInput.Supply.FullCode;
                        tLine += "    ";

                        tLine2 = new string(' ', tLine.Length);
                        tLine2 += "     ";

                        var description = item.GoalBudgetInput.Supply.Description;


                        counter++;
                        if (item.GoalBudgetInput.Supply.Description.Length > 67)
                        {
                            isLong = true;
                            tLine += description.Substring(0, 67);
                            tLine2 += description.Substring(67, description.Length - 67);
                            counter++;
                        }
                        else
                        {
                            tLine += description;
                        }

                        while(tLine.Length < 94)
                        {
                            tLine += " ";
                        }

                        tLine += "   ";
                        tLine += item.GoalBudgetInput.MeasurementUnit.Abbreviation;

                        while (tLine.Length < 104)
                        {
                            tLine += " ";
                        }

                        tLine += "   ";
                        tLine += item.ProjectPhase.Code;

                        while (tLine.Length < 115)
                        {
                            tLine += " ";
                        }

                        var cant = item.Quantity.ToString("N2", CultureInfo.InvariantCulture);

                        tLine += new string(' ', 11 - cant.Length);

                        tLine += cant;

                        tLine += "  ";

                        cant = item.DeliveredQuantity.ToString("N2", CultureInfo.InvariantCulture);

                        tLine += new string(' ', 11 - cant.Length);
                        tLine += cant;

                        itemNumber++;

                        tw.WriteLine(tLine);

                        if (isLong == true)
                            tw.WriteLine(tLine2);
                    }
                    /*
                    while (counter < limiteItems)
                    {
                        tw.WriteLine();
                        counter++;
                    }*/

                    tw.WriteLine();

                    tLine3 = new string(' ', 116);
                    tLine3 += "------------";
                    tLine3 += " ------------";
                    tw.WriteLine(tLine3);
                    tLine3 = new string(' ', 115);
                    var total = items.Sum(x => x.Quantity).ToString("N2", CultureInfo.InvariantCulture);

                    tLine3 += new string(' ', 11 - total.Length);

                    tLine3 += total;


                    tLine3 += "  ";

                    total = items.Sum(x => x.DeliveredQuantity).ToString("N2", CultureInfo.InvariantCulture);

                    tLine3 += new string(' ', 11 - total.Length);
                    tLine3 += total;
                    tw.WriteLine(tLine3);
                    tLine3 = " ";
                    tLine3 += new string('-', limite - 2);
                    tw.WriteLine(tLine3);
                    /*
                    var zone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                    if(zone == null)
                        zone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                    DateTime time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, zone);
                    */

                    DateTime time = DateTime.UtcNow.ToDefaultTimeZone();

                    tw.WriteLine(" OBSERVACIONES:  " + request.Observation);
                    tw.WriteLine();
                    tw.WriteLine();
                    tLine3 = " AUTORIZANTE    :  " + usuario.FullName;

                    tLine3 += new string(' ', 55 - tLine3.Length);
                    
                    tAux = " RECEPTOR  :  ________________________ ";

                    tLine3 += tAux;

                    tLine3 += new string(' ', 45 - tAux.Length);

                    tAux = " MONTO DESPACHO S/  :  ";

                    var cantAux = monto.ToString("N2", CultureInfo.InvariantCulture);

                    tAux += new string(' ', 16 - cantAux.Length);

                    tAux += cantAux;

                    tLine3 += tAux;
                    tw.WriteLine(tLine3);
                    tw.WriteLine();
                    tLine3 = " FECHA DESPACHO :  " + time.Date.ToDateString();

                    tLine3 += new string(' ', 55 - tLine3.Length);

                    tAux = " DNI       :  ________________________ ";

                    tLine3 += tAux;
                    tw.WriteLine(tLine3);
                    tw.WriteLine();
                    tLine3 = " HORA           :  " + time.ToString("HH:mm");

                    tLine3 += new string(' ', 55 - tLine3.Length);

                    tAux = " FIRMA     :  ________________________ ";

                    tLine3 += tAux;
                    tw.WriteLine(tLine3);




                    tw.Flush();
                    ms.Position = 0;


                    return File(ms.ToArray(), "text/plain", items.FirstOrDefault().FieldRequest
                        .DocumentNumber.ToString("D6") + ".txt");
                }

            }
        }

        [HttpGet("reporte/{id}")]
        public async Task<IActionResult> Report(Guid id)
        {

            var pId = GetProjectId();

            string uid = GetUserId();

            var u = await _context.Users.Where(x => x.Id == uid).FirstOrDefaultAsync();
            //sp afuera
            var data = await _context.Set<UspFieldRequestReport>().FromSqlRaw("execute WareHouse_uspFieldRequestReport")
                        .IgnoreQueryFilters()
                        .ToListAsync();

            data = data.Where(x => x.ProjectId == pId && x.Id == id).ToList();

            //sp folding

            SqlParameter param1 = new SqlParameter("@Id", id);

            var datafolding = await _context.Set<UspFieldRequestFoldingReport>().FromSqlRaw("execute WareHouse_uspFieldRequestFoldingReport @Id", param1)
                        .IgnoreQueryFilters()
                        .ToListAsync();



            var first = data.FirstOrDefault();

            var project = _context.Projects.Where(x => x.Id == pId).FirstOrDefault();

            var enlace = project.LogoUrl.ToString();

            using (MemoryStream ms = new MemoryStream())
            {

                WebClient client = new WebClient();
                Stream img = client.OpenRead(enlace);
                Bitmap bitmap; bitmap = new Bitmap(img);

                Image image = (Image)bitmap;

                //bitmap.Save(msaux, System.Drawing.Imaging.ImageFormat.Png);

                ///////
                ///signature 1
                 WebClient clients1 = new WebClient();
                Stream imgSignature = clients1.OpenRead("https://erpivcstorage.blob.core.windows.net/usuarios/firmas/firmasjuntas.jpg");
                Bitmap bmp; bmp = new Bitmap(imgSignature);
                Image imagesignature = (Image)bmp;

                //signature 2
                WebClient clients2 = new WebClient();
                Stream imgSignature2 = clients2.OpenRead("https://erpivcstorage.blob.core.windows.net/usuarios/firmas/firma2.jpg");
                Bitmap bmp2; bmp2 = new Bitmap(imgSignature2);
                Image imagesignature2 = (Image)bmp2;

                ///
                PdfDocument doc = new PdfDocument();
                PdfPageBase page = doc.Pages.Add(new SizeF(711, 792), new PdfMargins(0));

                PdfGrid grid = new PdfGrid();
                grid.Columns.Add(5);
                float width = page.Canvas.ClientSize.Width - (grid.Columns.Count + 1);

                grid.Columns[0].Width = 130;
                grid.Columns[1].Width = 190;
                grid.Columns[2].Width = 190;
                grid.Columns[3].Width = 50;
                grid.Columns[4].Width = 100;

                //float width = page.Canvas.ClientSize.Width - (grid.Columns.Count + 1);
                //for (int j = 0; j < grid.Columns.Count; j++)
                //{
                //    grid.Columns[j].Width = 100;
                //    //grid.Columns[j].Width = 100;
                //}

                PdfGridRow row0 = grid.Rows.Add();
                PdfGridRow row1 = grid.Rows.Add();
                PdfGridRow row2 = grid.Rows.Add();
                PdfGridRow row3 = grid.Rows.Add();
                float height = 20.0f;
                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    grid.Rows[i].Height = height;
                }

                PdfGridCellContentList lst = new PdfGridCellContentList();
                PdfGridCellContent textAndStyle = new PdfGridCellContent();
                textAndStyle.Image = PdfImage.FromStream(ToStream(image));
                textAndStyle.ImageSize = new SizeF(130, 80);

                lst.List.Add(textAndStyle);

                //grid.Draw(page, new PointF(25.5, 50));

                row0.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                row1.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                row2.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                row3.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);

                var text = "";

                if(project.Abbreviation.Contains("("))
                {
                    var temptext = project.Abbreviation.Split("(")[1];
                    text = temptext.Replace(")", "");
                }
                else
                {
                    text = project.Abbreviation;
                }

                row0.Cells[0].Value = text;
                row0.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                row0.Cells[0].RowSpan = 4;
                row0.Cells[0].ColumnSpan = 1;

                row0.Cells[1].Value = "GESTIÓN DE ALMACÉN";
                row0.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                row0.Cells[1].ColumnSpan = 2;
                row0.Cells[1].RowSpan = 1;

                row1.Cells[1].Value = "PEDIDO Y ENTREGA DE MATERIALES DE ALMACÉN";
                row1.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                row1.Cells[1].ColumnSpan = 2;
                row1.Cells[1].RowSpan = 3;


                row0.Cells[3].Value = "Código";
                
                row0.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);


                row1.Cells[3].Value = "Versión";
                
                row1.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);


                row2.Cells[3].Value = "Fecha";
                
                row2.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);


                row3.Cells[3].Value = "Página";
                
                row3.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);


                if (project.CostCenter == "050-2018")
                    row0.Cells[4].Value = "JIC/GAL-For-01";
                if (project.CostCenter == "001")
                    row0.Cells[4].Value = "CSH/GAL-For-01";
                
                row0.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);


                row1.Cells[4].Value = "1";
                
                row1.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);


                row2.Cells[4].Value = "28/09/2021";
                
                row2.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);


                row3.Cells[4].Value = "1 de 1";
                
                row3.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);


                grid.Draw(page, new PointF(25.5f, 0));

                ////////////////////////////////////////////////

                PdfGrid grid2 = new PdfGrid();
                grid2.Columns.Add(6);
                float width2 = page.Canvas.ClientSize.Width - (grid2.Columns.Count + 1);
                //for (int j = 0; j < grid2.Columns.Count; j++)
                //{
                //    grid2.Columns[j].Width = 20;
                //}

                grid2.Columns[0].Width = 70;
                grid2.Columns[1].Width = 205;
                grid2.Columns[2].Width = 205;
                grid2.Columns[3].Width = 60;
                grid2.Columns[4].Width = 60;
                grid2.Columns[5].Width = 60;



                PdfGridRow gridrow0 = grid2.Rows.Add();
                PdfGridRow gridrow1 = grid2.Rows.Add();
                PdfGridRow gridrow2 = grid2.Rows.Add();
                PdfGridRow gridrow3 = grid2.Rows.Add();
                PdfGridRow gridrow4 = grid2.Rows.Add();
                PdfGridRow gridrow5 = grid2.Rows.Add();
                PdfGridRow gridrow6 = grid2.Rows.Add();
                PdfGridRow gridrow7 = grid2.Rows.Add();

                gridrow0.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                gridrow1.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                gridrow2.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                gridrow3.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                gridrow4.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                gridrow5.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                gridrow6.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                gridrow7.Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);

                float height2 = 20.0f;
                for (int i = 0; i < grid2.Rows.Count; i++)
                {
                    grid2.Rows[i].Height = height2;
                }

                gridrow0.Cells[0].Value = "Obra";
                gridrow0.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow0.Cells[0].RowSpan = 4;
                gridrow0.Cells[0].ColumnSpan = 1;

                gridrow0.Cells[1].Value = project.Name;
                //gridrow0.Cells[1].Style.Font = new PdfTrueTypeFont("Sans Serif", 6f, PdfFontStyle.Regular, true);
                gridrow0.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow0.Cells[1].ColumnSpan = 2;
                gridrow0.Cells[1].RowSpan = 4;

                gridrow0.Cells[3].Value = "N°";
                //gridrow0.Cells[3].Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                gridrow0.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow0.Cells[3].RowSpan = 4;
                gridrow0.Cells[3].ColumnSpan = 1;


                gridrow0.Cells[4].Value = first.DocumentNumber.ToString("d4");
                //gridrow0.Cells[4].Style.Font = new PdfTrueTypeFont("Sans Serif", 10f, PdfFontStyle.Regular, true);
                gridrow0.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow0.Cells[4].ColumnSpan = 2;
                gridrow0.Cells[4].RowSpan = 4;

                gridrow4.Cells[0].Value = "O.T.N°:";
                gridrow4.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow0.Cells[0].ColumnSpan = 1;
                //gridrow4.Cells[0].Style.Font = new PdfTrueTypeFont("Sans Serif", 7f, PdfFontStyle.Regular, true);


                gridrow4.Cells[1].Value = first.WorkOrder;
                gridrow4.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow4.Cells[1].ColumnSpan = 2;
                //gridrow4.Cells[1].Style.Font = new PdfTrueTypeFont("Sans Serif", 7f, PdfFontStyle.Regular, true);




                gridrow4.Cells[3].Value = "DIA";
                gridrow4.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow4.Cells[3].RowSpan = 2;

                gridrow4.Cells[4].Value = "MES";
                gridrow4.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow4.Cells[4].RowSpan = 2;

                gridrow4.Cells[5].Value = "AÑO";
                gridrow4.Cells[5].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow4.Cells[5].RowSpan = 2;

                gridrow5.Cells[0].Value = "FORMULA:";
                gridrow5.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                //gridrow5.Cells[0].Style.Font = new PdfTrueTypeFont("Sans Serif", 7f, PdfFontStyle.Regular, true);


                gridrow5.Cells[1].Value = first.FormulaCodes;
                gridrow5.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow5.Cells[1].ColumnSpan = 2;
                //gridrow5.Cells[1].Style.Font = new PdfTrueTypeFont("Sans Serif", 7f, PdfFontStyle.Regular, true);



                gridrow6.Cells[0].Value = "FRENTE:";
                gridrow6.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                //gridrow6.Cells[0].Style.Font = new PdfTrueTypeFont("Sans Serif", 7f, PdfFontStyle.Regular, true);

                gridrow6.Cells[1].Value = first.WorkFrontCode;
                gridrow6.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow6.Cells[1].ColumnSpan = 2;
                //gridrow6.Cells[1].Style.Font = new PdfTrueTypeFont("Sans Serif", 7f, PdfFontStyle.Regular, true);



                gridrow6.Cells[3].Value = first.Day.ToString("d2");
                gridrow6.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow6.Cells[3].RowSpan = 2;

                gridrow6.Cells[4].Value = first.Month.ToString("d2");
                gridrow6.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow6.Cells[4].RowSpan = 2;

                gridrow6.Cells[5].Value = first.Year.ToString();
                gridrow6.Cells[5].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow6.Cells[5].RowSpan = 2;

                gridrow7.Cells[0].Value = "CUADRILLA: ";
                gridrow7.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                //gridrow7.Cells[0].Style.Font = new PdfTrueTypeFont("Sans Serif", 7f, PdfFontStyle.Regular, true);

                gridrow7.Cells[1].Value = first.SewerGroupCode;
                gridrow7.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                gridrow7.Cells[1].ColumnSpan = 2;
                //gridrow7.Cells[1].Style.Font = new PdfTrueTypeFont("Sans Serif", 7f, PdfFontStyle.Regular, true);



                grid2.Draw(page, new PointF(25.5f, 90));

                ////////////////////////////////////////////////

                PdfGrid grid3 = new PdfGrid();
                grid3.Columns.Add(6);


                grid3.Columns[0].Width = 70;
                grid3.Columns[1].Width = 60;
                grid3.Columns[2].Width = 350;
                grid3.Columns[3].Width = 60;
                grid3.Columns[4].Width = 60;
                grid3.Columns[5].Width = 60;

                //float width3 = page.Canvas.ClientSize.Width - (grid3.Columns.Count + 1);
                //for (int j = 0; j < grid3.Columns.Count; j++)
                //{
                //    grid3.Columns[j].Width = width3 * 0.16f;
                //}

                PdfGridRow grid3row0 = grid3.Rows.Add();
                PdfGridRow grid3row1 = grid3.Rows.Add();


                grid3row0.Style.Font = new PdfTrueTypeFont("Sans Serif", 9f, PdfFontStyle.Regular, true);

                float height3 = 20.0f;


                grid3row0.Cells[0].Value = "FASE";
                grid3row0.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                grid3row0.Cells[0].ColumnSpan = 1;
                grid3row0.Cells[0].RowSpan= 2;



                grid3row0.Cells[1].Value = "CÓDIGO";
                grid3row0.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                grid3row0.Cells[1].ColumnSpan = 1;
                grid3row0.Cells[1].RowSpan = 2;

                grid3row0.Cells[2].Value = "DESCRIPCIÓN DEL PRODUCTO";
                grid3row0.Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                grid3row0.Cells[2].ColumnSpan = 1;
                grid3row0.Cells[2].RowSpan = 2;


                grid3row0.Cells[3].Value = "UND.";
                grid3row0.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                grid3row0.Cells[3].ColumnSpan = 1;
                grid3row0.Cells[3].RowSpan = 2;

                grid3row0.Cells[4].Value = "CANTIDAD SOLICITADA";
                grid3row0.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                grid3row0.Cells[4].ColumnSpan = 1;
                grid3row0.Cells[4].RowSpan = 2;

                grid3row0.Cells[5].Value = "CANTIDAD ENTREGADA";
                grid3row0.Cells[5].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                grid3row0.Cells[5].ColumnSpan = 1;
                grid3row0.Cells[5].RowSpan = 2;



                var count = 0;
                if (datafolding.Count != 0)
                    foreach (var item in datafolding)
                    {
                        PdfGridRow row = grid3.Rows.Add();
                        row.Style.Font = new PdfTrueTypeFont("Sans Serif", 9f, PdfFontStyle.Regular, true);
                        row.Cells[0].Value = item.Phase;
                        row.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        row.Cells[0].ColumnSpan = 1;

                        row.Cells[1].Value = item.FullCode;
                        row.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        row.Cells[1].ColumnSpan = 1;


                        row.Cells[2].Value = item.Description;
                        row.Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        row.Cells[2].ColumnSpan = 1;


                        row.Cells[3].Value = item.UnitAbbreviation;
                        row.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        row.Cells[3].ColumnSpan = 1;

                        row.Cells[4].Value = item.Quantity.ToString();
                        row.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        row.Cells[4].ColumnSpan = 1;

                        row.Cells[5].Value = item.DeliveredQuantity.ToString();
                        row.Cells[5].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        row.Cells[5].ColumnSpan = 1;

                        count++;
                    }

                    for ( int i =0; i < 14-count; i++)
                    {
                        PdfGridRow row = grid3.Rows.Add();
                    }

                for (int i = 0; i < grid3.Rows.Count; i++)
                {
                    grid3.Rows[i].Height = height3;
                }


                grid3.Draw(page, new PointF(25.5f, 260));

                ///////////////////////////////////////////
                ///

                PdfGrid grid4 = new PdfGrid();
                grid4.Columns.Add(6);

                    grid4.Columns[0].Width = 110;
                    grid4.Columns[1].Width = 110;
                    grid4.Columns[2].Width = 110;
                    grid4.Columns[3].Width = 110;
                    grid4.Columns[4].Width = 110;
                    grid4.Columns[5].Width = 110;

                PdfGridCellContentList lsts1 = new PdfGridCellContentList();
                PdfGridCellContent textAndStyles1 = new PdfGridCellContent();
                textAndStyles1.Image = PdfImage.FromStream(ToStream(imagesignature));
                textAndStyles1.ImageSize = new SizeF(510, 40);

                lsts1.List.Add(textAndStyles1);

                PdfGridCellContentList lsts2 = new PdfGridCellContentList();
                PdfGridCellContent textAndStyles2 = new PdfGridCellContent();
                textAndStyles2.Image = PdfImage.FromStream(ToStream(imagesignature2));
                textAndStyles2.ImageSize = new SizeF(255, 40);

                lsts2.List.Add(textAndStyles2);

                PdfGridRow grid4row0 = grid4.Rows.Add();
                PdfGridRow grid4row1 = grid4.Rows.Add();
                PdfGridRow grid4row2 = grid4.Rows.Add();
                PdfGridRow grid4row3 = grid4.Rows.Add();
                PdfGridRow grid4row4 = grid4.Rows.Add();
                PdfGridRow grid4row5 = grid4.Rows.Add();


                grid4row0.Style.Font = new PdfTrueTypeFont("Sans Serif", 9f, PdfFontStyle.Regular, true);
                grid4row1.Style.Font = new PdfTrueTypeFont("Sans Serif", 9f, PdfFontStyle.Regular, true);
                grid4row2.Style.Font = new PdfTrueTypeFont("Sans Serif", 9f, PdfFontStyle.Regular, true);
                grid4row3.Style.Font = new PdfTrueTypeFont("Sans Serif", 9f, PdfFontStyle.Regular, true);
                grid4row4.Style.Font = new PdfTrueTypeFont("Sans Serif", 9f, PdfFontStyle.Regular, true);
                grid4row5.Style.Font = new PdfTrueTypeFont("Sans Serif", 9f, PdfFontStyle.Regular, true);
                

                float height4 = 20.0f;
                for (int i = 0; i < grid4.Rows.Count; i++)
                {
                    grid4.Rows[i].Height = height4;
                }

                grid4row0.Cells[0].Value = "Observaciones: "+ first.Observation;
               
                grid4row0.Cells[0].ColumnSpan = 6;
                grid4row0.Cells[0].RowSpan = 3;

                grid4row3.Cells[0].Value = "V°B° AUTORIZANTE \n"+ u.Name + " " + u.PaternalSurname + " " + u.MaternalSurname;
                grid4row3.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                grid4row3.Cells[0].ColumnSpan = 3;
                grid4row3.Cells[0].RowSpan = 3;

                grid4row3.Cells[3].Value = "PERSONA QUE RECOGE \nApellidos y Nombre: _______________________________  \nDNI: _______________________________  \nFIRMA: _______________________________";
                grid4row3.Cells[3].ColumnSpan = 3;
                grid4row3.Cells[3].RowSpan = 3;





                //grid4row6.Cells[3].Value = lsts2;
                //grid4row6.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                //grid4row6.Cells[3].ColumnSpan = 3;
                //grid4row6.Cells[3].RowSpan = 2;

                grid4.Draw(page, new PointF(25.5f, 600));

                doc.SaveToStream(ms);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "prueba.pdf");
            }
        }

        private Stream ToStream(Image image)
        {
            var stream = new MemoryStream();

            image.Save(stream, image.RawFormat);
            stream.Position = 0;

            return stream;
        }
    }
}
