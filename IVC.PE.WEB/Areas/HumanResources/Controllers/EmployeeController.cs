using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/empleados")]
    public class EmployeeController : BaseController
    {
        public EmployeeController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<EmployeeController> logger)
            : base(context, userManager, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int? workArea = null, Guid? entryPositionId = null, Guid? currentPositionId = null)
        {
            var data = await _context.EmployeeWorkPeriods
                .Include(x => x.Employee)
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new EmployeeViewModel
                {
                    Id = x.EmployeeId,
                    Name = x.Employee.Name,
                    PaternalSurname = x.Employee.PaternalSurname,
                    MaternalSurname = x.Employee.MaternalSurname,
                    Document = x.Employee.Document,
                    BirthDate = x.Employee.BirthDate.Value.ToDateString()
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var model = await _context.Employees
                .Where(x => x.Id == id)
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    MiddleName = x.MiddleName,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    BirthDate = x.BirthDate.HasValue ? x.BirthDate.Value.ToLocalDateFormat() : null,
                    Document = x.Document,
                    DocumentType = x.DocumentType,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber
                }).FirstOrDefaultAsync();
            return Ok(model);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = new Employee
            {
                Name = model.Name,
                PaternalSurname = model.PaternalSurname,
                MaternalSurname = model.MaternalSurname,
                DocumentType = model.DocumentType,
                Document = model.Document,
                BirthDate = model.BirthDate.ToUtcDateTime(),
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            employee.Name = model.Name;
            employee.PaternalSurname = model.PaternalSurname;
            employee.MaternalSurname = model.MaternalSurname;
            employee.DocumentType = model.DocumentType;
            employee.Document = model.Document;
            employee.BirthDate = model.BirthDate.ToUtcDateTime();
            employee.PhoneNumber = model.PhoneNumber;
            employee.Email = model.Email;

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
                return BadRequest($"Empleado con Id '{id}' no encontrado.");
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return Ok();
        }



        [HttpGet("importar/nuevos")]
        public async Task<IActionResult> ImportNewEmployeesSample()
        {
            string fileName = "EmpleadosNuevos.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"A1").Value = "Nro. Documento";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "T/Documento";
                workSheet.Cell($"B2").Value = "(1:DNI, 2-CEXT, 4-PASAPORTE)";
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"C1").Value = "Apellido Paterno";
                workSheet.Range("C1:C2").Merge();
                workSheet.Range("C1:C2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"D1").Value = "Apellido Materno";
                workSheet.Range("D1:D2").Merge();
                workSheet.Range("D1:D2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"E1").Value = "Nombres";
                workSheet.Range("E1:E2").Merge();
                workSheet.Range("E1:E2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"F1").Value = "Fecha Nacimiento";
                workSheet.Range("F1:F2").Merge();
                workSheet.Range("F1:F2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"G1").Value = "Celular";
                workSheet.Range("G1:G2").Merge();
                workSheet.Range("G1:G2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"H1").Value = "Email";
                workSheet.Range("H1:H2").Merge();
                workSheet.Range("H1:H2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"I1").Value = "Fecha Ingreso";
                workSheet.Range("I1:I2").Merge();
                workSheet.Range("I1:I2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"J1").Value = "Fondo de Pensión";
                workSheet.Cell($"J2").Value = "(1-ONP, 2-PRO, 3-PRO MIXTA, 4-INT, 5-INT MIXTA, 6-PRI, 7-PRI MIXTA, 8-HAB MIXTA, 9-SIN REG PENSIONARIO)";
                workSheet.Range("J1:J2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"K1").Value = "CUSSP";
                workSheet.Range("K1:K2").Merge();
                workSheet.Range("K1:K2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"L1").Value = "Cargo (Id)";
                workSheet.Range("L1:L2").Merge();
                workSheet.Range("L1:L2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"M1").Value = "Asig. Familiar (0:No/1:Si)";
                workSheet.Range("M1:M2").Merge();
                workSheet.Range("M1:M2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"N1").Value = "Sexo (0:M/1:F)";
                workSheet.Range("N1:N2").Merge();
                workSheet.Range("N1:N2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"O1").Value = "Banco (Id)";
                workSheet.Range("O1:O2").Merge();
                workSheet.Range("O1:O2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"P1").Value = "Cta. Bancaria";
                workSheet.Range("P1:P2").Merge();
                workSheet.Range("P1:P2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"Q1").Value = "Cta. Bancaria CCI";
                workSheet.Range("Q1:Q2").Merge();
                workSheet.Range("Q1:Q2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"R1").Value = "Área de Trabajo (Id)";
                workSheet.Range("R1:R2").Merge();
                workSheet.Range("R1:R2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();
                workSheet.Range("A1:Q10").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("A1:Q10").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                var positions = await _context.WorkPositions.Where(x => x.Type == ConstantHelpers.WorkPositions.Type.EMPLOYEE).ToListAsync();
                workSheet = wb.Worksheets.Add("Cargos");
                workSheet.Cell($"A1").Value = "Id";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Cargo";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                var i = 0;
                foreach (var item in positions)
                {
                    workSheet.Cell($"A${3 + i}").Value = item.Id;
                    workSheet.Cell($"B${3 + i}").Value = item.Name;
                    i++;
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();

                var bancos = await _context.Banks.ToListAsync();
                workSheet = wb.Worksheets.Add("Bancos");
                workSheet.Cell($"A1").Value = "Id";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Banco";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                i = 0;
                foreach (var item in bancos)
                {
                    workSheet.Cell($"A${3 + i}").Value = item.Id;
                    workSheet.Cell($"B${3 + i}").Value = item.Name;
                    i++;
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();

                var workAreas = ConstantHelpers.Employee.WorkArea.VALUES;
                workSheet = wb.Worksheets.Add("AreasTrabajo");
                workSheet.Cell($"A1").Value = "Id";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Área";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                i = 0;
                foreach (var item in workAreas)
                {
                    workSheet.Cell($"A${3 + i}").Value = item.Key;
                    workSheet.Cell($"B${3 + i}").Value = item.Value;
                    i++;
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();
                //workSheet.Range($"A1:B{positions.Count + 1}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                //workSheet.Range($"A1:B{positions.Count + 1}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost("importar/nuevos")]
        public async Task<IActionResult> ImportNewEmployees(IFormFile file)
        {
            var errors = new List<KeyValuePair<string, string>>();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;
                    var employeesDb = await _context.Employees
                        .AsNoTracking()
                        .ToListAsync();

                    var newEmployees = new List<Employee>();
                    var newEmployeeWorkPeriods = new List<EmployeeWorkPeriod>();
                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var document = workSheet.Cell($"A{counter}").GetString();
                        var employeeExist = employeesDb.FirstOrDefault(x => x.Document == document);
                        if (employeeExist == null)
                        {
                            try
                            {
                                var employee = new Employee
                                {
                                    Name = workSheet.Cell($"E{counter}").GetString(),
                                    PaternalSurname = workSheet.Cell($"C{counter}").GetString(),
                                    MaternalSurname = workSheet.Cell($"D{counter}").GetString(),
                                    DocumentType = GetDocumentType(workSheet.Cell($"B{counter}").GetString()),
                                    Document = document,
                                    BirthDate = GetDate(workSheet, counter, "F"),
                                    PhoneNumber = workSheet.Cell($"G{counter}").GetString(),
                                    Email = workSheet.Cell($"H{counter}").GetString(),
                                    BankId = GetBank(workSheet.Cell($"O{counter}").GetString()),
                                    BankAccount = workSheet.Cell($"P{counter}").GetString(),
                                    BankAccountCci = workSheet.Cell($"Q{counter}").GetString(),
                                    HaveHouseholdAllowance = false,
                                    Gender = GetGender(workSheet.Cell($"N{counter}").GetString())
                                };

                                var employeeWorkPeriod = new EmployeeWorkPeriod
                                {
                                    Employee = employee,
                                    EntryDate = GetDate(workSheet, counter, "I") ?? DateTime.MinValue,
                                    ProjectId = GetProjectId(),
                                    PensionFundAdministratorId = GetAFPGuid(workSheet.Cell($"J{counter}").GetString()),
                                    PensionFundUniqueIdentificationCode = workSheet.Cell($"K{counter}").GetString(),
                                    WorkArea = workSheet.Cell($"R{counter}").GetValue<int>(),
                                    WorkerPositionId = GetWorkerPositionId(workSheet.Cell($"L{counter}").GetString()),
                                    HasSctr = false,
                                    SctrHealthType = 0,
                                    SctrPensionType = 0,
                                    JudicialRetentionFixedAmmount = 0,
                                    JudicialRetentionPercentRate = 0,
                                    LaborRegimen = 21,
                                    HasEPS = false,
                                    HasEsSaludPlusVida = false,
                                    IsActive = true,
                                };

                                //Chequear errores
                                var haveErrors = false;
                                if (employee.DocumentType == 0)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Tipo de Documento inválido"));
                                    haveErrors = true;
                                }
                                if (employeeWorkPeriod.EntryDate == DateTime.MinValue)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Fecha de Ingreso inválida"));
                                    haveErrors = true;
                                }
                                if (!employeeWorkPeriod.WorkerPositionId.HasValue)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Id de Cargo inválido"));
                                }

                                if (haveErrors)
                                {
                                    counter++;
                                    continue;
                                }

                                newEmployees.Add(employee);
                                newEmployeeWorkPeriods.Add(employeeWorkPeriod);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.StackTrace);
                            }
                        };
                        counter++;
                    }

                    await _context.Employees.AddRangeAsync(newEmployees);
                    await _context.EmployeeWorkPeriods.AddRangeAsync(newEmployeeWorkPeriods);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }

            if (errors.Count != 0)
                TempData["Errores"] = JsonConvert.SerializeObject(errors);

            return Ok(errors.Count());
        }

        [HttpGet("importar/errores")]
        public FileResult ImportErrors()
        {
            if (TempData["Errores"] == null)
            {
                RedirectToAction("Empty");
            }

            var importErrors = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(TempData["Errores"].ToString());

            DataTable dt = GetData(importErrors);
            //Name of File
            string fileName = "ErroresCarga.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        private DataTable GetData(List<KeyValuePair<string, string>> importErrors)
        {
            //Creating DataTable
            DataTable dt = new DataTable();
            //Setiing Table Name
            dt.TableName = "ErrorsData";
            //Add Columns
            dt.Columns.Add("Línea", typeof(int));
            dt.Columns.Add("Error", typeof(string));
            //Add Rows in DataTable
            foreach (var item in importErrors)
            {
                dt.Rows.Add(item.Key, item.Value);
            }
            dt.AcceptChanges();
            return dt;
        }

        private bool GetBool(string v)
        {
            var success = Int32.TryParse(v, out int _value);
            if (success && _value==1)
                return true;
            return false;
        }

        private int GetDocumentType(string documentTypeStr)
        {
            var success = Int32.TryParse(documentTypeStr, out int documentType);
            if (success && (documentType == 1 || documentType == 2 || documentType == 4))
                return documentType;
            return 0;
        }

        private DateTime? GetDate(IXLWorksheet workSheet, int counter, string column)
        {
            if (!workSheet.Cell($"{column}{counter}").IsEmpty())
            {
                if (workSheet.Cell($"{column}{counter}").DataType == XLDataType.DateTime)
                {
                    try
                    {
                        var datetime = workSheet.Cell($"{column}{counter}").GetDateTime();
                        return new DateTime(datetime.Year, datetime.Month, datetime.Day);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.StackTrace);
                    }
                }
            }
            return null;
        }

        private Guid? GetBank(string v)
        {
            var success = Guid.TryParse(v, out Guid position);
            if (success)
                return position;
            return null;
        }

        private int GetGender(string v)
        {
            var success = Int32.TryParse(v, out int category);
            if (success)
                switch (category)
                {
                    case 0:
                        return ConstantHelpers.Worker.Gender.MALE;
                    default:
                        return ConstantHelpers.Worker.Gender.FEMALE;
                }
            return ConstantHelpers.Worker.Gender.MALE;
        }

        private Guid GetAFPGuid(string afpStr)
        {
            var success = Int32.TryParse(afpStr, out int afp);
            if (success)
                switch (afp)
                {
                    case 1: return Guid.Parse("E804DF0C-D783-4534-2B3B-08D7AB570506");
                    case 2: return Guid.Parse("19DB4712-40E4-483D-2B3D-08D7AB570506");
                    case 3: return Guid.Parse("2078074F-D5E8-4D20-2B3A-08D7AB570506");
                    case 4: return Guid.Parse("0D166874-E319-4DF5-2B3C-08D7AB570506");
                    case 5: return Guid.Parse("F064BCC1-563B-48F4-2B3F-08D7AB570506");
                    case 6: return Guid.Parse("7CC8A397-80A5-4F4F-2B40-08D7AB570506");
                    case 7: return Guid.Parse("CC345D37-320C-44C5-2B3E-08D7AB570506");
                    case 8: return Guid.Parse("FE43D32E-19B9-4C62-2B41-08D7AB570506");
                    case 9: return Guid.Parse("8A48449A-5869-482C-2B42-08D7AB570506");
                    default: return Guid.Parse("8A48449A-5869-482C-2B42-08D7AB570506");
                }
            return Guid.Parse("8A48449A-5869-482C-2B42-08D7AB570506");
        }

        private Guid? GetWorkerPositionId(string positionstr)
        {
            var success = Guid.TryParse(positionstr, out Guid position);
            if (success)
                return position;
            return null;
        }
    }
}