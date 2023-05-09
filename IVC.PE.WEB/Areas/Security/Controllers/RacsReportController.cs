using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.Security;
using IVC.PE.WEB.Areas.Security.ViewModels.RacsReportViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IVC.PE.WEB.Areas.Security.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Security.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.SECURITY)]
    [Route("seguridad/reportes-racs")]
    public class RacsReportController : BaseController
    {
        public RacsReportController(IvcDbContext context,
            ILogger<RacsReportController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("condicion-subestandar-grafico")]
        public async Task<IActionResult> GetSubstandarConditionGraph(/*Guid? pId = null*/)
        {
            var racsSCQ = await _context.Set<UspRacsSCQ>().FromSqlRaw("execute Security_uspRacsSCQ")
                .IgnoreQueryFilters()
                .ToListAsync();

            //if (pId.HasValue)
            //    racsSCQ = racsSCQ.Where(x => x.ProjectId == pId.Value).ToList();

            var chart = new ChartViewModel();

            chart.cols = new List<ChartViewModel.Col>
            {
                {
                    new ChartViewModel.Col
                    {
                        label = "Condición",
                        type = "string"
                    }
                }
            };

            var rowData = new Dictionary<int, List<KeyValuePair<Guid, int>>>();
            
            for (int i = 1; i <= 29; i++)
            {
                rowData[i] = new List<KeyValuePair<Guid, int>>();
            }

            var selectedProjectId = GetProjectId();
            var projects = new List<Guid>();
            
            foreach (var racs in racsSCQ.Where(x => x.ProjectId == selectedProjectId))
            {
                projects.Add(racs.ProjectId);
                chart.cols.Add(new ChartViewModel.Col
                {
                    label = racs.Project,
                    type = "number"
                });

                chart.cols.Add(new ChartViewModel.Col
                {
                    role = "annotation"
                });

                rowData[1].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ01));
                rowData[2].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ02));
                rowData[3].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ03));
                rowData[4].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ04));
                rowData[5].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ05));
                rowData[6].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ06));
                rowData[7].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ07));
                rowData[8].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ08));
                rowData[9].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ09));
                rowData[10].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ10));
                rowData[11].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ11));
                rowData[12].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ12));
                rowData[13].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ13));
                rowData[14].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ14));
                rowData[15].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ15));
                rowData[16].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ16));
                rowData[17].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ17));
                rowData[18].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ18));
                rowData[19].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ19));
                rowData[20].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ20));
                rowData[21].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ21));
                rowData[22].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ22));
                rowData[23].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ23));
                rowData[24].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ24));
                rowData[25].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ25));
                rowData[26].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ26));
                rowData[27].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ27));
                rowData[28].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ28));
                rowData[29].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SCQ29));
            }

            chart.rows = new List<ChartViewModel.Row>();
            for (int i = 1; i <= 29; i++)
            {
                chart.rows.Add(new ChartViewModel.Row
                {
                    c = new List<ChartViewModel.C>
                    {
                        {
                            new ChartViewModel.C
                            {
                                v = RacsSCQ.VALUES[i]
                            }
                        }
                    }
                });

                foreach (var project in projects)
                {
                    chart.rows[i-1].c.Add(new ChartViewModel.C
                    {
                        v = rowData[i].First(x => x.Key == project).Value
                    });

                    chart.rows[i - 1].c.Add(new ChartViewModel.C
                    {
                        v = rowData[i].First(x => x.Key == project).Value
                    });
                }
            }

            return Ok(chart);
        }

        [HttpGet("acto-subestandar-grafico")]
        public async Task<IActionResult> GetSubstandarActGraph(/*Guid? pId = null*/)
        {
            var racsSAQ = await _context.Set<UspRacsSAQ>().FromSqlRaw("execute Security_uspRacsSAQ")
                .IgnoreQueryFilters()
                .ToListAsync();

            var chart = new ChartViewModel();

            chart.cols = new List<ChartViewModel.Col>
            {
                {
                    new ChartViewModel.Col
                    {
                        label = "Acto",
                        type = "string"
                    }
                }
            };

            Dictionary<int, List<KeyValuePair<Guid, int>>> rowData = new Dictionary<int, List<KeyValuePair<Guid, int>>>();
            
            for (int i = 1; i <= 36; i++)
            {
                rowData[i] = new List<KeyValuePair<Guid, int>>();
            }
            
            var selectedProjectId = GetProjectId();
            var projects = new List<Guid>();

            foreach (var racs in racsSAQ.Where(x => x.ProjectId == selectedProjectId))
            {
                projects.Add(racs.ProjectId);
                chart.cols.Add(new ChartViewModel.Col
                {
                    label = racs.Project,
                    type = "number"
                });

                chart.cols.Add(new ChartViewModel.Col
                {
                    role = "annotation"
                });

                rowData[1].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ01));
                rowData[2].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ02));
                rowData[3].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ03));
                rowData[4].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ04));
                rowData[5].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ05));
                rowData[6].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ06));
                rowData[7].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ07));
                rowData[8].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ08));
                rowData[9].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ09));
                rowData[10].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ10));
                rowData[11].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ11));
                rowData[12].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ12));
                rowData[13].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ13));
                rowData[14].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ14));
                rowData[15].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ15));
                rowData[16].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ16));
                rowData[17].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ17));
                rowData[18].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ18));
                rowData[19].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ19));
                rowData[20].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ20));
                rowData[21].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ21));
                rowData[22].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ22));
                rowData[23].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ23));
                rowData[24].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ24));
                rowData[25].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ25));
                rowData[26].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ26));
                rowData[27].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ27));
                rowData[28].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ28));
                rowData[29].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ29));
                rowData[30].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ30));
                rowData[31].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ31));
                rowData[32].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ32));
                rowData[33].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ33));
                rowData[34].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ34));
                rowData[35].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ35));
                rowData[36].Add(new KeyValuePair<Guid, int>(racs.ProjectId, racs.SAQ36));
            }

            chart.rows = new List<ChartViewModel.Row>();
            for (int i = 1; i <= 36; i++)
            {
                chart.rows.Add(new ChartViewModel.Row
                {
                    c = new List<ChartViewModel.C>
                    {
                        {
                            new ChartViewModel.C
                            {
                                v = RacsSAQ.VALUES[i]
                            }
                        }
                    }
                });

                foreach (var project in projects)
                {
                    chart.rows[i - 1].c.Add(new ChartViewModel.C
                    {
                        v = rowData[i].First(x => x.Key == project).Value
                    });

                    chart.rows[i - 1].c.Add(new ChartViewModel.C
                    {
                        v = rowData[i].First(x => x.Key == project).Value
                    });
                }
            }

            return Ok(chart);
        }

        [HttpGet("por-cudarillas")]
        public async Task<IActionResult> GetRacsSewergroupExcel(int? year = null,  int? month = null)
        {
            var projectId = GetProjectId();
            var projectParam = new SqlParameter("@ProjectId", projectId);

            var racsSg = await _context.Set<UspRacsSewegroup>().FromSqlRaw("execute Security_uspRacsSewergroup @ProjectId", projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            if (year.HasValue)
                racsSg = racsSg.Where(x => x.ReportDate.Year == year.Value).ToList();
            if (month.HasValue)
                racsSg = racsSg.Where(x => x.ReportDate.Month == month.Value).ToList();

            var racsSCQ = await _context.Set<UspRacsSCQ>().FromSqlRaw("execute Security_uspRacsSCQ")
                .IgnoreQueryFilters()
                .ToListAsync();

            var racsSAQ = await _context.Set<UspRacsSAQ>().FromSqlRaw("execute Security_uspRacsSAQ")
                .IgnoreQueryFilters()
                .ToListAsync();

            var filterStr = string.Concat(year.HasValue ? "/"+year.Value.ToString() : string.Empty,
                            month.HasValue ? "-" + month.Value.ToString() : string.Empty);
            TempData[$"racsCuadrillas{filterStr}"] = JsonConvert.SerializeObject(new UspRacsViewModel
            {
                UspRacsSewegroups = racsSg,
                UspRacsSCQs = racsSCQ,
                UspRacsSAQs = racsSAQ
            });

            return Ok($"racsCuadrillas{filterStr}");
        }

        [HttpGet("descargar-excel")]
        public FileResult DownloadFile (string excelName)
        {
            if (TempData[excelName] == null)
            {
                RedirectToAction("Empty");
            }

            var dts = new List<DataTable>();
            string fileName = string.Empty;
            var excel = excelName.Split('/');
            switch (excel[0])
            {
                case "racsCuadrillas":
                    var racs = JsonConvert.DeserializeObject<UspRacsViewModel>(TempData[excelName].ToString());
                    fileName = $"ReporteRacs{excel[1]}.xlsx";
                    dts = GetData(racs);
                    break;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet
                foreach (var dt in dts)
                {
                    wb.Worksheets.Add(dt);
                    wb.Worksheet(dt.TableName).Columns().AdjustToContents();
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        private List<DataTable> GetData(UspRacsViewModel racs)
        {
            var dts = new List<DataTable>();
            //Creating DataTable
            DataTable dt = new DataTable
            {
                //Setting Table Name
                TableName = "RACS"
            };
            //Add Columns
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Fecha de Reporte", typeof(DateTime));
            dt.Columns.Add("Jefe de Frente", typeof(string));
            dt.Columns.Add("Capataz", typeof(string));
            dt.Columns.Add("Reporta", typeof(string));
            dt.Columns.Add("RACS", typeof(int));
            dt.Columns.Add("En Progreso", typeof(int));
            dt.Columns.Add("Levantados", typeof(int));
            dt.Columns.Add("Identifica Cond.Sub.", typeof(int));
            dt.Columns.Add("Identifica Acto.Sub.", typeof(int));
            //Add Rows in DataTable
            foreach (var item in racs.UspRacsSewegroups)
            {
                dt.Rows.Add(
                    item.Code,
                    item.ReportDate.Date,                    
                    item.WorkFrontHead,
                    item.Foreman,
                    item.Username,
                    item.RacsTotal,
                    item.RacsInProgress,
                    item.RacsLifted,
                    item.RacsCondition,
                    item.RacsAct
                );
            }
            dt.AcceptChanges();
            dts.Add(dt);

            dt = new DataTable
            {
                //Setting Table Name
                TableName = "RACS-Condiciones"
            };
            dt.Columns.Add("Condición Subestandar", typeof(string));
            foreach (var q in RacsSCQ.VALUES)
            {
                dt.Rows.Add(q.Value);
            }
            foreach (var item in racs.UspRacsSCQs)
            {
                dt.Columns.Add(item.Project, typeof(int));

                dt.Rows[0][item.Project] = item.SCQ01;
                dt.Rows[1][item.Project] = item.SCQ02;
                dt.Rows[2][item.Project] = item.SCQ03;
                dt.Rows[3][item.Project] = item.SCQ04;
                dt.Rows[4][item.Project] = item.SCQ05;
                dt.Rows[5][item.Project] = item.SCQ06;
                dt.Rows[6][item.Project] = item.SCQ07;
                dt.Rows[7][item.Project] = item.SCQ08;
                dt.Rows[8][item.Project] = item.SCQ09;
                dt.Rows[9][item.Project] = item.SCQ10;
                dt.Rows[10][item.Project] = item.SCQ11;
                dt.Rows[11][item.Project] = item.SCQ12;
                dt.Rows[12][item.Project] = item.SCQ13;
                dt.Rows[13][item.Project] = item.SCQ14;
                dt.Rows[14][item.Project] = item.SCQ15;
                dt.Rows[15][item.Project] = item.SCQ16;
                dt.Rows[16][item.Project] = item.SCQ17;
                dt.Rows[17][item.Project] = item.SCQ18;
                dt.Rows[18][item.Project] = item.SCQ19;
                dt.Rows[19][item.Project] = item.SCQ20;
                dt.Rows[20][item.Project] = item.SCQ21;
                dt.Rows[21][item.Project] = item.SCQ22;
                dt.Rows[22][item.Project] = item.SCQ23;
                dt.Rows[23][item.Project] = item.SCQ24;
                dt.Rows[24][item.Project] = item.SCQ25;
                dt.Rows[25][item.Project] = item.SCQ26;
            }
            dt.AcceptChanges();
            dts.Add(dt);

            dt = new DataTable
            {
                //Setting Table Name
                TableName = "RACS-Actos"
            };
            dt.Columns.Add("Acto Subestandar", typeof(string));
            foreach (var q in RacsSAQ.VALUES)
            {
                dt.Rows.Add(q.Value);
            }
            foreach (var item in racs.UspRacsSAQs)
            {
                dt.Columns.Add(item.Project, typeof(int));

                dt.Rows[0][item.Project] = item.SAQ01;
                dt.Rows[1][item.Project] = item.SAQ02;
                dt.Rows[2][item.Project] = item.SAQ03;
                dt.Rows[3][item.Project] = item.SAQ04;
                dt.Rows[4][item.Project] = item.SAQ05;
                dt.Rows[5][item.Project] = item.SAQ06;
                dt.Rows[6][item.Project] = item.SAQ07;
                dt.Rows[7][item.Project] = item.SAQ08;
                dt.Rows[8][item.Project] = item.SAQ09;
                dt.Rows[9][item.Project] = item.SAQ10;
                dt.Rows[10][item.Project] = item.SAQ11;
                dt.Rows[11][item.Project] = item.SAQ12;
                dt.Rows[12][item.Project] = item.SAQ13;
                dt.Rows[13][item.Project] = item.SAQ14;
                dt.Rows[14][item.Project] = item.SAQ15;
                dt.Rows[15][item.Project] = item.SAQ16;
                dt.Rows[16][item.Project] = item.SAQ17;
                dt.Rows[17][item.Project] = item.SAQ18;
                dt.Rows[18][item.Project] = item.SAQ19;
            }
            dt.AcceptChanges();
            dts.Add(dt);

            return dts;
        }

        #region Helpers
        public static class RacsSCQ
        {
            public const string SQC01 = "01. Falta Orden y Limpieza en áreas de trabajo y almacén.";
            public const string SQC02 = "02. Falta señalización (cintas, carteles) en las áreas o delimitación deficiente.";
            public const string SQC03 = "03. EPPs NO recomendados al área y al trabajo, no hay stock..";
            public const string SQC04 = "04. Documentación MASS desactualizados, sin archivar, no impresos, sin firmar y no están publicados.";
            public const string SQC05 = "05. Derrame de Hidrocarburo y/o concreto.";
            public const string SQC06 = "06. Escaleras (sin inspección, mal almacenadas, mal posicionada)";
            public const string SQC07 = "07. Instalaciones eléctricas mal instaladas o inadecuadas.";
            public const string SQC08 = "08. Maderas con clavos, presencia de elementos punzantes.";
            public const string SQC09 = "09. Mal Manejo y disposición de RRSS.";
            public const string SQC10 = "10. Accesos inadecuados (sin baranda, berma en mal estado, presencia de obstáculos)";
            public const string SQC11 = "11. No se evidencia SCTR vigente e impreso y publicado en obra.";
            public const string SQC12 = "12. Falta agua para el personal.";
            public const string SQC13 = "13. Ausencia del Residente.";
            public const string SQC14 = "14. Ausencia del jefe y/o Supervisor SSI.";
            public const string SQC15 = "15. Equipos de emergencias en mal estado, sin inspección, materiales incompletos o ausencia o mal ubicados.";
            public const string SQC16 = "16. Mal manejo de producto químico (mal almacenamiento, mala disposición, falta hoja MSDS, sin rotulado)";
            public const string SQC17 = "17. Andamios (sin tarjeta, sin barandas, rodapiés, sin inspección, plataforma incompleta, inestables)";
            public const string SQC18 = "18. Falta o deficiente colocación de dispositivos de seguridad (guardas u otros)";
            public const string SQC19 = "19. Diseño inadecuado del equipo o del trabajo.";
            public const string SQC20 = "20. Falta o deficiente Iluminación en la zona de trabajo.";
            public const string SQC21 = "21. Materiales inflamables cerca de trabajos en caliente.";
            public const string SQC22 = "22. Mantenimiento inadecuado de equipos o herramientas.";
            public const string SQC23 = "23. Deficiente estados de los sistemas de cierre (Cerraduras, pestillos, candados, otros)";
            public const string SQC24 = "24. Escaleras, andamios, plataformas en mal estado.";
            public const string SQC25 = "25. Condiciones climáticas adversas (tormenta eléctrica, granizada, etc.)";
            public const string SQC26 = "26. Presencia de agentes nocivos en la atmósfera.";
            public const string SQC27 = "27. Material cerca al borde de la zanja.";
            public const string SQC28 = "28. Falta de coordinación o ausencia de vigías/ Excesiva velocidad de vehículos particulares/ Tolva de volquete sin asegurar/ Incumplimiento de Manejo Defensivo/ Trabajador dentro de radio de trabajo.";
            public const string SQC29 = "29. Otras condiciones subestandares";


            public static Dictionary<int, string> VALUES = new Dictionary<int, string>
            {
                {1, SQC01},
                {2, SQC02},
                {3, SQC03},
                {4, SQC04},
                {5, SQC05},
                {6, SQC06},
                {7, SQC07},
                {8, SQC08},
                {9, SQC09},
                {10, SQC10},
                {11, SQC11},
                {12, SQC12},
                {13, SQC13},
                {14, SQC14},
                {15, SQC15},
                {16, SQC16},
                {17, SQC17},
                {18, SQC18},
                {19, SQC19},
                {20, SQC20},
                {21, SQC21},
                {22, SQC22},
                {23, SQC23},
                {24, SQC24},
                {25, SQC25},
                {26, SQC26},
                {27, SQC27},
                {28, SQC28},
                {29, SQC29},
            };
        }

        public static class RacsSAQ
        {
            public const string SAQ01 = "1. Malas posturas, riesgo ergonómico.";
            public const string SAQ02 = "2. Trabajador sobre plataformas inestables.";
            public const string SAQ03 = "3. EPP´s son utilizados inadecuadamente o no los usan.";
            public const string SAQ04 = "4. Sin tarjetas de entrenamiento o desactualizadas o sin dictado de cursos.";
            public const string SAQ05 = "5. No utilizar lentes de seguridad o en mal estado.";
            public const string SAQ06 = "6. Personal sin protección de manos o guantes en mal estado.";
            public const string SAQ07 = "7. No utilizar arnés, mala utilización de línea de anclaje y línea de vida, mal almacenamiento, falta de inspección.";
            public const string SAQ08 = "8. No se generan permisos para trabajos de alto riesgo, firmas incompletas, no está en el área de trabajo.";
            public const string SAQ09 = "9. No utilizar protector respiratorio según la actividad (polvos, gases, vapores, etc.)";
            public const string SAQ10 = "10. Mala utilización de herramientas manuales (sin inspección, malas condiciones) o hechizas.";
            public const string SAQ11 = "11. No se generó el ATS o Evaluación deficiente o firmas incompletas.";
            public const string SAQ12 = "12. No se generó la charla diaria o no firman el registro.";
            public const string SAQ13 = "13. No cumplir con los procedimientos de trabajo establecidos.";
            public const string SAQ14 = "14. Interferir o retirar dispositivos de Seguridad o de control Ambiental";
            public const string SAQ15 = "15. Trabajar sobre equipos en movimiento o riesgosos";
            public const string SAQ16 = "16. Operar o conducir a velocidad inadecuada o distraído";
            public const string SAQ17 = "17. Adoptar posiciones o posturas peligrosas";
            public const string SAQ18 = "18. Falta de atención en la tarea.";
            public const string SAQ19 = "19. Distraer, molestar, insultar, reñir, sorprender a otros colaboradores.";
            public const string SAQ20 = "20. No asegurar las herramientas o materiales en trabajos en altura.";
            public const string SAQ21 = "21. No respetar el área de seguridad en las maniobras.";
            public const string SAQ22 = "22. Realizar trabajos sin bloquear energías al intervenir maquinas o equipos.";
            public const string SAQ23 = "23. Ingresar a áreas restringidas sin autorización.";
            public const string SAQ24 = "24. No utilizar equipos de contención de derrames.";
            public const string SAQ25 = "25. No realizar la segregación adecuada de los residuos.";
            public const string SAQ26 = "26. Manipulación o traslado inadecuado de materiales peligrosos.";
            public const string SAQ27 = "27. Dejar material, equipos, otros, abandonados/expuestos.";
            public const string SAQ28 = "28. Dejar puertas y ventanas abiertas al término de sus labores.";
            public const string SAQ29 = "29. Otras condiciones Subestándar.";
            public const string SAQ30 = "30. No realizar el Check list de Pre Uso.";
            public const string SAQ31 = "31. Fumar en zonas no autorizadas o cerca combustibles.";
            public const string SAQ32 = "32. Levantar objetos de forma inadecuada.";
            public const string SAQ33 = "33. Realizar trabajos sin estar capacitado.";
            public const string SAQ34 = "34. Trabajar bajo influencia de alcohol y/o drogas.";
            public const string SAQ35 = "35. Realizar trabajos de alto riesgo (altura, caliente, etc.) sin autorización.";
            public const string SAQ36 = "36. Otros actos subestándares.";




            public static Dictionary<int, string> VALUES = new Dictionary<int, string>
            {
                {1, SAQ01},
                {2, SAQ02},
                {3, SAQ03},
                {4, SAQ04},
                {5, SAQ05},
                {6, SAQ06},
                {7, SAQ07},
                {8, SAQ08},
                {9, SAQ09},
                {10, SAQ10},
                {11, SAQ11},
                {12, SAQ12},
                {13, SAQ13},
                {14, SAQ14},
                {15, SAQ15},
                {16, SAQ16},
                {17, SAQ17},
                {18, SAQ18},
                {19, SAQ19},
                {20, SAQ20},
                {21, SAQ21},
                {22, SAQ22},
                {23, SAQ23},
                {24, SAQ24},
                {25, SAQ25},
                {26, SAQ26},
                {27, SAQ27},
                {28, SAQ28},
                {29, SAQ29},
                {30, SAQ30},
                {31, SAQ31},
                {32, SAQ32},
                {33, SAQ33},
                {34, SAQ34},
                {35, SAQ35},
                {36, SAQ36}
            };
        }
        #endregion
    }
}
