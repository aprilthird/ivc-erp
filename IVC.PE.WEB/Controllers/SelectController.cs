using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IVC.PE.WEB.ViewModels;
using IVC.PE.DATA.Context;
using Microsoft.EntityFrameworkCore;
using IVC.PE.CORE.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.CORE.Extensions;

namespace IVC.PE.WEB.Controllers
{
    [Route("select")]
    public class SelectController : BaseController
    {
        public SelectController(IvcDbContext context,
            ILogger<SelectController> logger)
            : base(context, logger)
        {
        }

        [HttpPost("proyectos/definir")]
        public async Task<IActionResult> SetProject(Guid projectId)
        {
            HttpContext.Session.SetString("ProjectId", projectId.ToString());
            return Ok();
        }

        // App Movil
        [HttpGet("proyectos-usuario/{id}")]
        public async Task<IActionResult> GetUserProjects(string id)
        {
            var data = await _context.UserProjects
                .Include(x => x.Project)
                .Where(x => x.UserId == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.ProjectId,
                    Text = x.Project.Abbreviation
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var data = await _context.Roles
                .Select(x => new SelectViewModel<string>
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("transporte-combustible-filtro")]
        public async Task<IActionResult> GetFuelTransportfilter()
        {
            var data = await _context.EquipmentMachineryTypeTransports
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("proyectos")]
        public async Task<IActionResult> GetProjects(bool? existRegister = null)
        {
            var query = _context.Projects.AsQueryable();


            if (existRegister.HasValue)
                if (existRegister.Value == true)
                    query = query.Where(x => _context.BondAdds.Any(a => a.ProjectId == x.Id));

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Abbreviation
                }).ToListAsync();

            return Ok(data);
        }


        [HttpGet("entidades-que-autorizan")]
        public async Task<IActionResult> AuthEntities()
        {
            var data = await _context.AuthorizingEntities
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("tipos-de-autorizacion")]
        public async Task<IActionResult> AuthTypes()
        {
            var data = await _context.AuthorizationTypes
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }


        [HttpGet("tipos-de-renovacion")]
        public async Task<IActionResult> RenovationTypes()
        {
            var data = await _context.RenovationTypes
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("formulas-proyecto")]
        public async Task<IActionResult> GetProjectFormulas()
        {
            var data = await _context.ProjectFormulas
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Code}-{x.Name}"
                }).ToListAsync();

            return Ok(data);
        }



        [HttpGet("jefes-de-frente")]
        public async Task<IActionResult> GetWorkFrontHeads()
        {
            var projectId = GetProjectId();

            var data = await _context.WorkFrontHeads
                .Where(x => x.ProjectId.Value == projectId)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = !string.IsNullOrEmpty(x.UserId)
                        ? $"{x.User.FullName} ({x.Code})" : $"No Asignado ({x.Code})"
                }).Distinct()
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("jefes-de-frente-activos")]
        public async Task<IActionResult> GetWorkerFrontHeadActives(string dateStr, Guid? projectId = null)
        {
            if (!projectId.HasValue)
                return Ok(new List<SelectViewModel>());

            var date = dateStr.ToDateTime();

            var data = await _context.SewerGroupPeriods
                .Include(x => x.SewerGroup)
                .Include(x => x.WorkFrontHead)
                .Include(x => x.WorkFrontHead.User)
                .Where(x => x.DateStart.Date <= date.Date &&
                    (x.DateEnd.HasValue ? x.DateEnd.Value.Date >= date.Date : true) &&
                    x.SewerGroup.ProjectId == projectId.Value &&
                    x.WorkFrontHeadId.HasValue)
                .Select(x => new SelectViewModel
                {
                    Id = x.WorkFrontHeadId.Value,
                    Text = x.WorkFrontHead.Code + " - " + x.WorkFrontHead.User.RawFullName
                }).ToListAsync();

            data = data.DistinctBy(x => x.Id).ToList();
            data = data.OrderBy(x => x.Text).ToList();

            return Ok(data);
        }

        //--
        [HttpGet("frentes-de-calidad")]
        public async Task<IActionResult> GetQualityFronts()
        {
            var projectId = GetProjectId();
            var data = await _context.QualityFronts
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }
        //--

        [HttpGet("frentes")]
        public async Task<IActionResult> GetWorkFronts()
        {
            var projectId = GetProjectId();
            var data = await _context.WorkFronts
                .Where(x => x.ProjectId.Value == projectId)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code
                }).ToListAsync();

            return Ok(data);
        }

        //Select usado en el app movil
        [HttpGet("frentes-proyecto")]
        public async Task<IActionResult> GetWorkFronts(Guid? projectId = null)
        {
            if (!projectId.HasValue)
                return Ok(new List<SelectViewModel>());

            var data = await _context.WorkFronts
                .Where(x => x.ProjectId.Value == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code
                }).ToListAsync();

            return Ok(data);
        }


        [HttpGet("frentes-formula")]
        public async Task<IActionResult> GetWorkFrontsByProjectFormula(Guid? projectFormulaId = null)
        {
            var pId = GetProjectId();

            var query = await _context.WorkFrontProjectPhases
                    .Include(x => x.ProjectPhase)
                    .Include(x => x.WorkFront)
                .Where(x => x.WorkFront.ProjectId == pId)
                .ToListAsync();

            if (projectFormulaId.HasValue)
            {
                query = await _context.WorkFrontProjectPhases.Where(x => x.ProjectPhase.ProjectFormulaId == projectFormulaId).ToListAsync();
            }

            var workFronts = query.GroupBy(x => x.WorkFrontId);

            var workFrontsAux = new List<WorkFront>();

            foreach (var front in workFronts)
            {
                workFrontsAux.Add(front.FirstOrDefault().WorkFront);
            }

            var data = workFrontsAux
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code
                }).ToList();

            return Ok(data);
        }

        [HttpPost("frentes-formulas")]
        public async Task<IActionResult> GetWorkFrontsByProjectFormulas(List<Guid> projectFormulas)
        {
            var pId = GetProjectId();

            var query = await _context.WorkFrontProjectPhases
                .Include(x => x.ProjectPhase)
                .Include(x => x.WorkFront)
                .Where(x => x.WorkFront.ProjectId == pId)
                .Where(x => projectFormulas.Contains((Guid)x.ProjectPhase.ProjectFormulaId))
                .ToListAsync();

            var workFronts = query.GroupBy(x => x.WorkFrontId);

            var workFrontsAux = new List<WorkFront>();

            foreach (var front in workFronts)
            {
                workFrontsAux.Add(front.FirstOrDefault().WorkFront);
            }

            var data = workFrontsAux
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code
                }).ToList();

            return Ok(data);
        }

        [HttpGet("cuadrillas")]
        public async Task<IActionResult> GetSewerGroups(string reportDate, Guid? projectId = null, Guid? workFrontHeadId = null
            , bool? isActive = null)
        {
            var reportDateDt = DateTime.Today;
            if (!string.IsNullOrEmpty(reportDate))
                reportDateDt = reportDate.ToDateTime();

            if (!projectId.HasValue)
                return Ok(new List<SelectViewModel>());

            var query = _context.SewerGroupPeriods
                .Include(x => x.SewerGroup)
                .Where(x => x.SewerGroup.ProjectId == projectId.Value &&
                            (workFrontHeadId.HasValue ? x.WorkFrontHeadId == workFrontHeadId.Value : true) &&
                            (x.DateStart.Date <= reportDateDt.Date &&
                                x.DateEnd.HasValue ? x.DateEnd.Value.Date >= reportDateDt.Date : true)
                                && (isActive.HasValue ? x.IsActive == isActive.Value : true))
                .AsNoTracking()
                .AsQueryable();

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.SewerGroupId,
                    Text = x.SewerGroup.Code
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("cuadrillas-dashboard")]
        public async Task<IActionResult> GetSewerGroupsDashBoard()
        {
            var cuadrillas = await _context.SewerGroups
                .Where(x=>x.ProjectId == GetProjectId())
                .ToListAsync();

            var dailyTask = await _context.WorkerDailyTasks
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => x.SewerGroupId)
                .Distinct()
                .ToListAsync();

            var data = new List<SelectViewModel>();

            foreach (var item in cuadrillas)
                if (dailyTask.Contains(item.Id))
                    data.Add(new SelectViewModel
                    {
                        Id = item.Id,
                        Text = item.Code
                    });

            return Ok(data);
        }


        [HttpGet("cuadrillas-maquinarias")]
        public async Task<IActionResult> GetSewerGroupsMachs(string reportDate, Guid? projectId = null, Guid? workFrontHeadId = null)
        {
            var reportDateDt = DateTime.Today;
            if (!string.IsNullOrEmpty(reportDate))
                reportDateDt = reportDate.ToDateTime();

            if (!projectId.HasValue)
                return Ok(new List<SelectViewModel>());

            var query = _context.SewerGroupPeriods
                .Include(x => x.SewerGroup)
                .Where(x => x.SewerGroup.ProjectId == projectId.Value &&
                            (workFrontHeadId.HasValue ? x.WorkFrontHeadId == workFrontHeadId.Value : true) &&
                            (x.DateStart.Date <= reportDateDt.Date &&
                                x.DateEnd.HasValue ? x.DateEnd.Value.Date >= reportDateDt.Date : true))
                .AsNoTracking()
                .AsQueryable();

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.SewerGroupId,
                    Text = x.SewerGroup.Code
                }).ToListAsync();

            var data2 = await _context.SewerGroups.Where(x => x.Id == Guid.Parse("754e607d-6283-489f-b38a-9cf491aafb85")).
                Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code
                }
                ).ToListAsync();
            return Ok(data.Union(data2));
        }


        [HttpGet("cuadrillas-semana")]
        public async Task<IActionResult> GetSewerGroupsByWorkFront(Guid? projectId = null, Guid? workFrontHeadId = null, Guid? weekId = null)
        {
            if (!projectId.HasValue)
                return Ok(new List<SelectViewModel>());

            var wStart = DateTime.Now;
            var wEnd = DateTime.Now;
            if (weekId.HasValue)
            {
                var week = await _context.ProjectCalendarWeeks.FirstOrDefaultAsync(x => x.Id == weekId.Value);
                wStart = week.WeekStart;
                wEnd = week.WeekEnd;
            }

            var query = _context.SewerGroupPeriods
                .Include(x => x.SewerGroup)
                .Where(x => x.SewerGroup.ProjectId == projectId.Value &&
                            (workFrontHeadId.HasValue ? x.WorkFrontHeadId == workFrontHeadId.Value : true) &&
                            (x.DateStart.Date <= wEnd.Date &&
                                x.DateEnd.HasValue ? x.DateEnd.Value.Date >= wStart.Date : true))
                .AsNoTracking()
                .AsQueryable();

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.SewerGroupId,
                    Text = x.SewerGroup.Code
                }).ToListAsync();

            data = data.Distinct().ToList();

            return Ok(data);
        }

        [HttpGet("cuadrillas-frente")]
        public async Task<IActionResult> GetSewerGroupsByWorkFront(Guid? projectId = null, Guid? workFrontId = null)
        {
            //if (!projectId.HasValue)
              //  return Ok(new List<SelectViewModel>());

            var query = _context.WorkFrontSewerGroups
                .Include(x => x.SewerGroupPeriod)
                .Include(x => x.SewerGroupPeriod.SewerGroup)
                .Where(x => (projectId.HasValue ? x.SewerGroupPeriod.SewerGroup.ProjectId == projectId.Value : true)  &&
                            (workFrontId.HasValue ? x.WorkFrontId == workFrontId.Value : true))
                .AsNoTracking()
                .AsQueryable();

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.SewerGroupPeriod.SewerGroupId,
                    Text = x.SewerGroupPeriod.SewerGroup.Code
                }).ToListAsync();

            data = data.Distinct().ToList();

            return Ok(data);
        }
        [HttpGet("cuadrillas-proyecto")]
        public async Task<IActionResult> GetSewerGroupsByProjects()
        {
            var PId = GetProjectId();

            var data =await  _context.SewerGroups
                .Where(x=>x.ProjectId==PId)
                .Select(x => new SelectViewModel
                {
                    
                    Id=x.Id,
                    Text=x.Code,
                }).ToListAsync();

                return Ok(data);
        }

        [HttpGet("cuadrillas-jefe-frente")]
        public async Task<IActionResult> GetSewerGroupsByWorkFrontHead(Guid? workFrontHeadId = null)
        {
            var projectId = GetProjectId();

            var query = _context.SewerGroupPeriods
                .Include(x => x.SewerGroup)
                .Where(x => x.SewerGroup.ProjectId == projectId &&
                            (x.WorkFrontHeadId == workFrontHeadId))
                            .AsNoTracking()
                            .AsQueryable();

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.SewerGroupId,
                    Text = x.SewerGroup.Code
                }).ToListAsync();

            data = data.Distinct().ToList();

            return Ok(data);
        }
        

        [HttpGet("for05-cuadrillas")]
        public async Task<IActionResult> GetSewerGroupsFor05()
        {
            var for05Todos = await _context.SewerManifoldFor05s
                .Include(x => x.SewerManifold)
                .Include(x => x.SewerManifold.ProductionDailyPart)
                .ToListAsync();
            var sewerGroupTodos = await _context.SewerGroups.ToListAsync();

            var list = new List<SelectViewModel>();
            var count = 0;

            foreach (var group in sewerGroupTodos)
            {
                foreach (var for05 in for05Todos)
                {
                    if (group.Id == for05.SewerManifold.ProductionDailyPart.SewerGroupId && count == 0)
                    {
                        var select = new SelectViewModel
                        {
                            Id = group.Id,
                            Text = group.Code
                        };
                        list.Add(select);
                        count++;
                    }
                }
                count = 0;
            }

            return Ok(list);
        }

        [HttpGet("for37A-cuadrillas")]
        public async Task<IActionResult> GetSewerGroupsFor37A()
        {
            var for37ATodos = await _context.SewerManifoldFor37As
                .Include(x => x.SewerManifold)
                .Include(x => x.SewerManifold.ProductionDailyPart)
                .ToListAsync();
            var sewerGroupTodos = await _context.SewerGroups.ToListAsync();

            var list = new List<SelectViewModel>();
            var count = 0;

            foreach (var group in sewerGroupTodos)
            {
                foreach (var for37A in for37ATodos)
                {
                    if (group.Id == for37A.SewerManifold.ProductionDailyPart.SewerGroupId && count == 0)
                    {
                        var select = new SelectViewModel
                        {
                            Id = group.Id,
                            Text = group.Code
                        };
                        list.Add(select);
                        count++;
                    }
                }
                count = 0;
            }

            return Ok(list);
        }

        [HttpGet("pdpf7-cuadrillas")]
        public async Task<IActionResult> GetSewerGroupsPdpF7()
        {
            var pdpf7Todos = await _context.ProductionDailyParts
                .ToListAsync();
            var sewerGroupTodos = await _context.SewerGroups.ToListAsync();

            var list = new List<SelectViewModel>();
            var count = 0;


            foreach (var group in sewerGroupTodos)
            {
                foreach (var pdpf7 in pdpf7Todos)
                {
                    if (group.Id == pdpf7.SewerGroupId && count == 0)
                    {
                        var select = new SelectViewModel
                        {
                            Id = group.Id,
                            Text = group.Code
                        };
                        list.Add(select);
                        count++;
                    }
                }
                count = 0;
            }

            return Ok(list);
        }

        [HttpGet("cuadrillas-f5")]
        public async Task<IActionResult> GetF5SewerGroups()
        {
            var query = _context.SewerGroups
                .Where(x => x.Code.StartsWith("F5"))
                .AsQueryable();

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code
                }).ToListAsync();
            return Ok(data);
        }
        

        [HttpGet("pdpf7-jefes-de-frente")]
        public async Task<IActionResult> GetWorkHeadsPdpF7()
        {
            var pdpf7Todos = await _context.ProductionDailyParts
                .ToListAsync();
            var workFrontHeadTodos = await _context.WorkFrontHeads
                .Include(x => x.User)
                .ToListAsync();

            var list = new List<SelectViewModel>();
            var count = 0;


            foreach (var head in workFrontHeadTodos)
            {
                foreach (var pdpf7 in pdpf7Todos)
                {
                    if (head.Id == pdpf7.WorkFrontHeadId && count == 0)
                    {
                        var select = new SelectViewModel
                        {
                            Id = head.Id,
                            Text = head.User.FullName
                        };
                        list.Add(select);
                        count++;
                    }
                }
                count = 0;
            }

            return Ok(list);
        }
        
        [HttpGet("habilitaciones")]
        public async Task<IActionResult> GetQualificationZones(Guid? projectId = null)
        {
            var query = _context.QualificationZones.AsQueryable();
            if (projectId.HasValue)
                query = query.Where(x => x.ProjectId == projectId.Value);

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Code} - {x.Name}"
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("tramos")]
        public async Task<IActionResult> GetSewerLines(Guid? projectId = null, Guid? sewerGroupId = null, int? sewerGroupType = null)
        {
            var query = _context.SewerLines.AsQueryable();
            if (projectId.HasValue)
                query = query.Where(x => x.SewerGroup.WorkFront.SystemPhase.ProjectId == projectId.Value);
            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerGroupId == sewerGroupId.Value);
            if (sewerGroupType.HasValue)
                query = query.Where(x => x.SewerGroup.Type == sewerGroupType.Value);

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("buzones")]
        public async Task<IActionResult> GetSewerBoxes(Guid? projectId = null, Guid? sewerGroupId = null)
        {
            var query = _context.SewerBoxes.AsQueryable();
            //if (projectId.HasValue)
            //    query = query.Where(x => x.SewerGroup.WorkFront.SystemPhase.ProjectId == projectId.Value);
            //if (sewerGroupId.HasValue)
            //    query = query.Where(x => x.SewerGroupId == sewerGroupId.Value);
            var data = await query
                //.Where(x => x.Stage == ConstantHelpers.Stage.REAL)
                .OrderBy(x => x.Code)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("buzones-tipos")]
        public async Task<IActionResult> GetSewerboxTypes()
        {
            var query = new List<SelectIntViewModel>();

            foreach (var item in ConstantHelpers.Sewer.Box.Type.VALUES)
            {
                query.Add(new SelectIntViewModel
                {
                    Id = item.Key,
                    Text = item.Value
                });
            }

            return Ok(query);
        }

        [HttpGet("buzones-rangos")]
        public async Task<IActionResult> GetSewerboxRange()
        {
            var query = new List<SelectIntViewModel>();

            foreach (var item in ConstantHelpers.Sewer.Box.Footage.Ranges.VALUES)
            {
                query.Add(new SelectIntViewModel
                {
                    Id = item.Key,
                    Text = item.Value
                });
            }

            return Ok(query);
        }

        [HttpGet("canteras")]
        public async Task<IActionResult> GetQuarries(Guid? projectId = null)
        {
            var query = _context.Quarries.AsNoTracking().AsQueryable();
            if (projectId.HasValue)
                query = query.Where(x => x.ProjectId == projectId.Value);
            var data = await query
                .OrderBy(x => x.Name)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("pruebas-de-laboratorio-de-rellenos")]
        public async Task<IActionResult> GetFillingLaboratoryTests(Guid? projectId = null)
        {
            var query = _context.FillingLaboratoryTests.AsNoTracking().AsQueryable();
            //if (projectId.HasValue)
            //    query = query.Where(x => x.ProjectId == projectId.Value);
            var data = await query
                .OrderBy(x => x.RecordNumber)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.RecordNumber
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("pruebas-de-laboratorio-de-rellenos/{id}")]
        public async Task<IActionResult> FoldingFor05(Guid id)
        {
            var data = await _context.FoldingFor05s
                .Include(x => x.FillingLaboratoryTest)
                .Where(x => x.Id == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.FillingLaboratoryTest.RecordNumber
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("emisores-de-certificados")]
        public async Task<IActionResult> GetCertificateIssuers(Guid? projectId = null)
        {
            var query = _context.CertificateIssuers.AsNoTracking().AsQueryable();
            //if (projectId.HasValue)
            //    query = query.Where(x => x.ProjectId == projectId.Value);
            var data = await query
                .OrderBy(x => x.Name)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("cartas")]
        public async Task<IActionResult> GetLetters(int? type = null)
        {
            var projectId = GetProjectId();

            var query = _context.Letters.AsQueryable();
            if (type.HasValue)
                query = query.Where(x => x.Type == type.Value);

            query = query.Where(x => x.ProjectId == projectId);
            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("cartas-referencias")]
        public async Task<IActionResult> GetLettersReferences(Guid id)
        {
            var query = await _context.LetterLetters
                .Include(x => x.Reference)
                .Where(x => x.LetterId == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.ReferenceId,
                    Text = x.Reference.Name
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("cartas-respondidas")]
        public async Task<IActionResult> GetAnsweredLetters(Guid id)
        {
            var query = await _context.LetterLetters
                .Include(x => x.Letter)
                .Where(x => x.ReferenceId == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.LetterId,
                    Text = x.Letter.Name
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("cartas-caracteristicas-doc")]
        public async Task<IActionResult> GetLetterDocCharac()
        {
            var projectId = GetProjectId();

            var query = await _context.LetterDocumentCharacteristics
                .Where(x => x.ProjectId == projectId)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(query);
        }


        [HttpGet("entidades-emisoras-receptoras-de-cartas")]
        public async Task<IActionResult> GetIssuerTargets()
        {
            var projectId = GetProjectId();

            var data = await _context.IssuerTargets
                .Where(x => x.ProjectId == projectId)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("grupos-de-interes")]
        public async Task<IActionResult> GetInterestGroups()
        {
            var projectId = GetProjectId();

            var data = await _context.InterestGroups
                .Where(x => x.ProjectId == projectId)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return Ok(data);
        }


        [HttpGet("empleados2")]
        public async Task<IActionResult> GetEmployees2()
        {
            var data = await _context.Employees
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.FullName
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("empleados2/{id}")]
        public async Task<IActionResult> GetEmployees2(Guid id)
        {
            var data = await _context.Employees
                .Where(x => x.Id == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.FullName
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("empleados")]
        public async Task<IActionResult> GetEmployees()
        {
            var data = await _context.Users
                .Select(x => new SelectViewModel<string>
                {
                    Id = x.Id,
                    Text = x.FullName
                }).ToListAsync();
            return Ok(data);
        }


        [HttpGet("empleados-equipos")]
        public async Task<IActionResult> GetEmployeesEquipmentModule()
        {
            var data = await _context.UserProjects
                .Include(x=>x.User)
                .Where(x=>x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel<string>
                {
                    Id = x.UserId,
                    Text = x.User.FullName
                }).ToListAsync();
            return Ok(data);
        }


        [HttpGet("empleados/{id}")]
        public async Task<IActionResult> GetEmployees(string id)
        { 
            var data = await _context.Users
                .Where(x => x.Id == id)
                .Select(x => new SelectViewModel<string>
                {
                    Id = x.Id,
                    Text = x.FullName
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("obras")]
        public async Task<IActionResult> GetBiddingWorks()
        {
            var data = await _context.BiddingWorks
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = x.Name
                            }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("cuadernos-de-obra")]
        public async Task<IActionResult> GetWorkbooks()
        {

            var data = await _context.Workbooks
                .Where(x=>x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"Libro N° {x.Number}"
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("tipos-de-cuaderno")]
        public async Task<IActionResult> GetWorkbooksTypes()
        {
            var data = await _context.WorkbookTypes
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Description}"
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsers()
        {
            var data = await _context.Users
                .Select(x => new SelectViewModel
                {
                    Id = Guid.Parse(x.Id),
                    Text = $"{x.PaternalSurname} {x.MaternalSurname}, {x.Name}"
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("usuarios-proyecto")]
        public async Task<IActionResult> GetUsersByProject(Guid? projectId = null)
        {
            if (!projectId.HasValue)
                return Ok(new List<SelectViewModel>());

            var data = await _context.UserProjects
                .Include(x => x.User)
                .Where(x => (projectId.HasValue ? x.ProjectId == projectId.Value : true))
                .Select(x => new SelectViewModel
                {
                    Id = Guid.Parse(x.User.Id),
                    Text = $"{x.User.PaternalSurname} {x.User.MaternalSurname}, {x.User.Name}"
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("garantes")]
        public async Task<IActionResult> GetGuarantors()
        {
            var data = await _context.BondAdds
                .Include(x=>x.BondGuarantor)
                //.Where(x=>x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.BondGuarantorId,
                    Text = $"{x.BondGuarantor.Name}"
                }).Distinct()
                .ToListAsync();
            return Ok(data);
        }



        [HttpGet("empresas")]
        public async Task<IActionResult> GetBusinesses()
        {
            var data = await _context.Businesses
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.BusinessName}"
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("empresa-seleccionado")]
        public async Task<IActionResult> GetBusinessFoldingParticipation(Guid? businessId = null)
        {
            if (businessId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.BusinessParticipationFoldings
                .Include(x => x.Business)
                .Where(x => x.BusinessId == businessId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.IvcParticipation}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("empresas-comercial")]
        public async Task<IActionResult> GetBusinessesTradeName()
        {
            var data = await _context.Businesses
                .Select(x => new SelectViewModel<string>
                {
                    Id = x.Tradename,
                    Text = $"{x.Tradename}"
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("tipos-de-fianza")]
        public async Task<IActionResult> GetBonds()
        {
            var data = await _context.BondTypes
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Name}"
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("tipos-de-documentos-legal")]
        public async Task<IActionResult> GetLegalDocuments()
        {
            var data = await _context.LegalDocumentationTypes
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("profesiones-lista")]
        public async Task<IActionResult> GetProfession()
        {
            var data = await _context.Professions
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("tipos-de-obra-lista")]
        public async Task<IActionResult> GetWorkType()
        {
            var data = await _context.BiddingWorkTypes
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("componentes-de-obra-lista")]
        public async Task<IActionResult> GetBiddingWorkComponent()
        {
            var data = await _context.BiddingWorkComponents
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("tipo-de-certificado-lista")]
        public async Task<IActionResult> GetEquipmentCertificateType()
        {
            var data = await _context.EquipmentCertificateTypes
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.CertificateTypeName
                }).ToListAsync();
            return Ok(data);
        }
        [HttpGet("entidad-certificadora-lista")]
        public async Task<IActionResult> GetEquipmentCertifyingEntity()
        {
            var data = await _context.EquipmentCertifyingEntities
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.CertifyingEntityName
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("patrones-por-fecha")]
        public async Task<IActionResult> GetPatternsRenewalsEquipment(/*string initDate, string endDate*/)
        {
            //var startDate = await _context.EquipmentCertificateRenewals.FirstOrDefaultAsync();
            //var endDate = await _context.EquipmentCertificateRenewals.FirstOrDefaultAsync();

            var data = await _context.PatternCalibrationRenewals
                .Include(x=>x.PatternCalibration) 
                .Include(x=>x.EquipmentCertifyingEntity)
                .Where(x=>x.PatternCalibration.ProjectId == GetProjectId())
                //.Where(x=>x.CreateDate > initDate.ToDateTime() && x.EndDate < endDate.ToDateTime() )
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.PatternCalibration.Name}-{x.ReferenceNumber}-{x.EquipmentCertifyingEntity.CertifyingEntityName}-{x.CreateDate.Date.ToDateString()}-{x.EndDate.Date.ToDateString()}"
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("operadores")]
        public async Task<IActionResult> GetEquipmentCertificateOperator()
        {
            var data = await _context.EquipmentCertificateUserOperators
                .Where(x=>x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Operator
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("operadores-equipos")]
        public async Task<IActionResult> GetEquipmentOperator()
        {
            var data = await _context.EquipmentMachineryOperators
                .Include(x => x.EquipmentMachineryType)
                .Where(x => x.EquipmentMachineryType.Description == "Transporte")
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.ActualName

                    
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("entidades-aseguradoras")]
        public async Task<IActionResult> GetInsuranceEntities()
        {
            var data = await _context.InsuranceEntity
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("operador-transporte-seleccionado")]
        public async Task<IActionResult> GetFilteredOperatorsTransport(Guid? opId = null)
        {
            if (opId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachineryOperators
                .Include(x=>x.Worker)
                .Where(x => x.EquipmentMachineryTypeTransportId == opId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.ActualName
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("operador-transporte-parte-seleccionado")]
        public async Task<IActionResult> GetFilteredOperatorsTransportPart(Guid? opId = null)
        {
            if (opId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachineryTransportPartFoldings
                .Include(x => x.EquipmentMachineryOperator.Worker)
                .Include(x => x.EquipmentMachineryOperator)
                .Where(x => x.EquipmentMachineryTransportPartId == opId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.EquipmentMachineryOperatorId.Value,
                    Text = x.EquipmentMachineryOperator.ActualName
                }).Distinct().ToListAsync();

            return Ok(query);
        }



        [HttpGet("operador-maquinaria-parte-seleccionado")]
        public async Task<IActionResult> GetFilteredOperatorsMachPart(Guid? opId = null)
        {
            if (opId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachPartFoldings
                .Include(x => x.EquipmentMachineryOperator.Worker)
                .Include(x => x.EquipmentMachineryOperator)
                .Where(x => x.EquipmentMachPartId == opId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.EquipmentMachineryOperatorId,
                    Text = x.EquipmentMachineryOperator.ActualName

                }).Distinct().ToListAsync();

            return Ok(query);
        }



        [HttpGet("operadores-maquinaria")]
        public async Task<IActionResult> GetOperatorsMachs()
        {

            var query = await _context.EquipmentMachineryOperators
                .Where(x => x.EquipmentMachineryTypeTypeId != null || x.ActualName == "No Asignado")
                .Where(x=>x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.ActualName
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("operador-maquinaria-seleccionado")]
        public async Task<IActionResult> GetFilteredOperatorsMach(Guid? opId = null)
        {
            if (opId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachineryOperators
                .Include(x=>x.Worker)
                .Where(x => x.EquipmentMachineryTypeTypeId == opId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.FromOtherName + "" + x.OperatorName + "" +x.Worker.Name+" "+x.Worker.PaternalSurname +" "+x.Worker.MaternalSurname
                }).ToListAsync();

            return Ok(query);
        }


        [HttpGet("propietarios-lista")]
        public async Task<IActionResult> GetEquipmentOwners()
        {
            var data = await _context.EquipmentOwners
                .Where(x=>x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.OwnerName
                }).ToListAsync();
            return Ok(data);
        }

        /*
        [HttpGet("renovaciones")]
        public async Task<IActionResult> GetBondRenovations()
        {
            var data = await _context.BondRenovations
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Name}"
                }).ToListAsync();
            return Ok(data);
        }*/


        [HttpGet("cargos")]
        public async Task<IActionResult> GetPositions()
        {
            var data = await _context.Positions                            
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = x.Name
                            }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("cargos-laborales/{id}")]
        public async Task<IActionResult> GetWorkPositions(int id)
        {
            var data = await _context.WorkPositions
                            .Where(x => x.Type == id)
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = x.Name
                            }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("obreros")]
        public async Task<IActionResult> GetWorkers(Guid? projectId = null)
        {
            if (!projectId.HasValue)
                return Ok(new List<SelectViewModel>());

            var data = await _context.WorkerWorkPeriods
                .Include(x => x.Worker)
                .Where(x => x.ProjectId == projectId.Value &&
                            x.IsActive)
                .Select(x => new SelectViewModel
                {
                    Id = x.WorkerId.Value,
                    Text = x.Worker.RawFullName
                }).ToListAsync();

            data = data.Distinct().ToList();

            return Ok(data);
        }

        [HttpGet("obreros-tareo")]
        public async Task<IActionResult> GetWorkers(string dateTask, Guid? projectId = null)
        {
            if (string.IsNullOrEmpty(dateTask))
                return Ok(new List<SelectViewModel>());
            if (!projectId.HasValue)
                return Ok(new List<SelectViewModel>());

            var dt = dateTask.ToDateTime();

            var data = await _context.WorkerWorkPeriods
                .Include(x => x.Worker)
                .Where(x => x.ProjectId == projectId.Value && x.EntryDate.Value.Date <= dt.Date &&
                                    (x.CeaseDate.HasValue ? x.CeaseDate.Value.Date >= dt.Date : true))
                .Select(x => new SelectViewModel
                {
                    Id = x.WorkerId.Value,
                    Text = x.Worker.RawFullName
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("obreros-semana")]
        public async Task<IActionResult> GetWorkers(Guid? projectId = null, Guid? weekId = null)
        {
            if (!projectId.HasValue)
                return Ok(new List<SelectViewModel>());

            var wStart = DateTime.Now;
            var wEnd = DateTime.Now;
            if (weekId.HasValue)
            {
                var week = await _context.ProjectCalendarWeeks.FirstOrDefaultAsync(x => x.Id == weekId.Value);
                wStart = week.WeekStart;
                wEnd = week.WeekEnd;
            }

            var data = await _context.WorkerWorkPeriods
                .Include(x => x.Worker)
                .Where(x => x.ProjectId == projectId.Value && 
                            x.EntryDate.Value.Date <= wEnd.Date &&
                           (x.CeaseDate.HasValue ? x.CeaseDate.Value.Date >= wStart.Date : true))
                .Select(x => new SelectViewModel
                {
                    Id = x.WorkerId.Value,
                    Text = x.Worker.RawFullName
                }).ToListAsync();

            data = data.Distinct().ToList();

            return Ok(data);
        }

        [HttpGet("obreros-operadores")]
        public async Task<IActionResult> GetWorkersOperators()
        {
            var data = await _context.Workers
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = x.RawFullName
                            }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("obreros-empleados")]
        public async Task<IActionResult> GetWorkersAndEmployees(Guid? projectId = null)
        {
            var workers = await _context.WorkerWorkPeriods
                                .Include(x => x.Worker)
                                .Where(x => x.EntryDate.Value.Date <= DateTime.Now.Date &&
                                            (x.CeaseDate.HasValue ? x.CeaseDate.Value.Date >= DateTime.Now.Date : true) &&
                                            (projectId.HasValue ? x.ProjectId == projectId.Value : x.ProjectId == GetProjectId()))
                                .Select(x => new SelectViewModel
                                {
                                    Id = x.WorkerId.Value,
                                    Text = x.Worker.RawFullName
                                }).ToListAsync();
            var employees = await _context.EmployeeWorkPeriods
                                .Include(x => x.Employee)
                                .Where(x => x.EntryDate.Date <= DateTime.Now.Date &&
                                            (x.CeaseDate.HasValue ? x.CeaseDate.Value.Date >= DateTime.Now.Date : true) &&
                                            (projectId.HasValue ? x.ProjectId == projectId.Value : x.ProjectId == GetProjectId()))
                                .Select(x => new SelectViewModel
                                {
                                    Id = x.EmployeeId,
                                    Text = x.Employee.RawFullName
                                }).ToListAsync();
            var data = workers.Concat(employees);
            return Ok(data);
        }

        [HttpGet("fases")]
        public async Task<IActionResult> GetSystemPhases()
        {
            var data = await _context.SystemPhases
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = x.Code
                            }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("fases-proyecto")]
        public async Task<IActionResult> GetProjectPhases(Guid? projectId = null)
        {
            if (!projectId.HasValue)
                return Ok(new List<SelectViewModel>());

            var data = await _context.ProjectPhases
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code + " - " + x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("fases-proyecto-maquinaria")]
        public async Task<IActionResult> GetMachineryPhases()
        {

            var data = await _context.MachineryPhases
                .Include(x=>x.ProjectPhase)
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = x.ProjectPhase.Code + " - " + x.ProjectPhase.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("fases-proyecto-maquinaria-app")]
        public async Task<IActionResult> GetMachineryPhasesApp(Guid? projectId = null)
        {

            var data = await _context.MachineryPhases
                .Include(x => x.ProjectPhase)
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.ProjectPhase.Code + " - " + x.ProjectPhase.Description
                }).ToListAsync();

            return Ok(data);
        }


        [HttpGet("fases-proyecto-transporte")]
        public async Task<IActionResult> GetTransportPhases()

        {

            var data = await _context.TransportPhases
                .Include(x => x.ProjectPhase)
                .Where(x=>x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.ProjectPhase.Code + " - " + x.ProjectPhase.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("fases-proyecto-formula")]
        public async Task<IActionResult> GetProjectPhasesByFormula(string fIds)
        {
            if (string.IsNullOrEmpty(fIds))
                return Ok(new List<SelectViewModel>());

            var lFids = new List<Guid>();

            foreach (var item in fIds.Split(","))
            {
                lFids.Add(Guid.Parse(item));
            }

            var data = await _context.ProjectPhases
                .Where(x => lFids.Contains(x.ProjectFormulaId.Value) && x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code + " - " + x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("fases-por-insumo-meta")]
        public async Task<IActionResult> GetProjectPhasesByWorkFrontAndProjectFormula(Guid? goalBudgetInputId = null)
        {
            var insumo = await _context.GoalBudgetInputs
                .FirstOrDefaultAsync(x => x.Id == goalBudgetInputId);

            if (insumo == null)
                return Ok(new List<SelectViewModel>());

            var query = await _context.WorkFrontProjectPhases
                .Include(x => x.ProjectPhase)
                .Where(x => x.ProjectPhase.ProjectFormulaId != null
                && x.ProjectPhase.ProjectFormulaId == insumo.ProjectFormulaId
                && x.WorkFrontId == insumo.WorkFrontId)
                .ToListAsync();

            var data = query
                .Select(x => new SelectViewModel
                {
                    Id = x.ProjectPhase.Id,
                    Text = x.ProjectPhase.Code + " - " + x.ProjectPhase.Description
                }).ToList();

            return Ok(data);
        }

        [HttpGet("fases-proyecto-frente-trabajo")]
        public async Task<IActionResult> GetProjectPhasesByWorkFrontAndProjectFormula(Guid? workFrontId = null, Guid? projectFormulaId = null)
        {
            var pId = GetProjectId();

            var query = await _context.WorkFrontProjectPhases
                .Include(x => x.ProjectPhase)
                .Where(x => x.ProjectPhase.ProjectId == pId && x.ProjectPhase.ProjectFormulaId != null)
                .ToListAsync();

            var projectPhases = await _context.ProjectPhases.Where(x => x.ProjectId == pId).ToListAsync();

            if (workFrontId.HasValue)
            {
                query = query.Where(x => x.WorkFrontId == workFrontId).ToList();
            }

            if(projectFormulaId.HasValue)
            {
                query = query.Where(x => x.ProjectPhase.ProjectFormulaId == projectFormulaId).ToList();
            }

            var data = query
                .Select(x => new SelectViewModel
                {
                    Id = x.ProjectPhase.Id,
                    Text = x.ProjectPhase.Code + " - " + x.ProjectPhase.Description
                }).ToList();

            return Ok(data);
        }

        [HttpGet("fases-por-proyecto-frente-trabajo")]
        public async Task<IActionResult> GetProjectPhasesByWorkFrontAndProjectFormulaProject(Guid? projectId,Guid workFrontId)
        {


            var query = await _context.WorkFrontProjectPhases
                .Include(x => x.ProjectPhase)
                .Where(x => x.ProjectPhase.ProjectId == projectId && x.ProjectPhase.ProjectFormulaId != null)
                .ToListAsync();

                query = query.Where(x => x.WorkFrontId == workFrontId).ToList();

            var data = query
                .Select(x => new SelectViewModel
                {
                    Id = x.ProjectPhase.Id,
                    Text = x.ProjectPhase.Code + " - " + x.ProjectPhase.Description
                }).ToList();

            return Ok(data);
        }


        [HttpGet("fases-proyecto-cuadrilla")]
        public async Task<IActionResult> GetProjectPhasesBySewerGroup(Guid? sgId = null)
        {
            if (sgId == null)
                return Ok(new List<SelectViewModel>());

            var sg = await _context.SewerGroups.FirstOrDefaultAsync(x => x.Id == sgId.Value);

            var formula = sg.Code.Substring(1, sg.Code.IndexOf("-")-1);
            var formulas = formula.Split("/");

            var query = await _context.RdpItems
                            .Include(x => x.ProjectPhase)
                            .Select(x => new SelectViewModel
                            {
                                Id = x.ProjectPhase.Id,
                                Text = x.ProjectPhase.Code + " - " + x.ProjectPhase.Description
                            }).Distinct().ToListAsync();

            var data = new List<SelectViewModel>();

            foreach (var frm in formulas)
            {
                data.AddRange(query.Where(x => x.Text.Substring(0, 2).Equals($"0{frm}")).ToList());
            }

            return Ok(data);
        }

        [HttpGet("fases-formula-cuadrilla")]
        public async Task<IActionResult> GetProjectPhasesByFormulaAndSewerGroup(Guid? sgId = null)
        {
            if (sgId == null)
                return Ok(new List<SelectViewModel>());

            var formulas = await _context.ProjectFormulaSewerGroups
                .Where(x => x.SewerGroupId == sgId.Value)
                .ToListAsync();

            var data = new List<SelectViewModel>();

            foreach (var item in formulas)
            {
                var phases = await _context.ProjectPhases
                    .Where(x => x.ProjectFormulaId == item.ProjectFormulaId)
                    .ToListAsync();
                if (phases.Count > 0)
                {
                    data.AddRange(
                        phases.Select(x => new SelectViewModel
                        {
                            Id = x.Id,
                            Text = x.Code + " - " + x.Description.Trim()
                        }).ToList()
                    );
                }
            }

            return Ok(data);
        }

        [HttpGet("colaboradores")]
        public async Task<IActionResult> GetCollaborators(Guid? providerId = null, Guid? projectId = null)
        {
            if (!providerId.HasValue)
                return Ok(new List<SelectViewModel>());

            var query = await _context.ProjectCollaborators.AsQueryable()
                .Where(x => x.ProviderId == providerId.Value &&
                            (projectId.HasValue ? x.ProjectId == projectId.Value : x.ProjectId == GetProjectId()))
                .Select(x => new SelectViewModel
                {   
                    Id = x.Id,
                    Text = x.FullName
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("variables-planilla")]
        public async Task<IActionResult> GetPayrollVariables()
        {
            var data = await _context.PayrollVariables
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = x.Description
                            }).ToListAsync();

            return Ok(data);
        }



        [HttpGet("semanas")]
        public async Task<IActionResult> GetProjectWeeks(int month = 0, Guid? projectId = null, int? year = null)
        {
            if (projectId == null)
                projectId = GetProjectId();

            if (year == null)
                year = DateTime.Now.Year;

            var projectcalendar = await _context.ProjectCalendars
                .FirstOrDefaultAsync(x => x.ProjectId == projectId.Value && x.Year == year.Value && x.IsWeekly);

            if (projectcalendar == null)
                return Ok(new List<SelectViewModel>());

            var data = await _context.ProjectCalendarWeeks
                            .Where(x => x.ProjectCalendarId == projectcalendar.Id
                            && (month > 0 ? x.WeekEnd.Month == month : true))
                            .OrderBy(x => x.WeekStart)
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = x.Description + " - Del " + x.WeekStart.ToDateString() + " Al " + x.WeekEnd.ToDateString()
                            }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("semanas-equipos")]
        public async Task<IActionResult> GetProjectEquipmentWeeks(int month = 0, Guid? projectId = null, int? year = null)
        {


            if (projectId == null)
                projectId = GetProjectId();

            if (year == null)
                year = DateTime.Now.Year;

            var projectcalendar = await _context.ProjectCalendars
                .FirstOrDefaultAsync(x => x.ProjectId == projectId.Value && x.Year == year.Value && x.IsWeekly);

            if (projectcalendar == null)
                return Ok(new List<SelectViewModel>());

            var data = await _context.ProjectCalendarWeeks
                            .Where(x => x.ProjectCalendarId == projectcalendar.Id
                            && (month > 0 ? x.WeekEnd.Month == month : true))
                            .OrderBy(x => x.WeekStart)
                            .Select(x => new SelectViewModel<int>
                            {
                                Id = x.WeekNumber,
                                Text = x.Description + " - Del " + x.WeekStart.ToDateString() + " Al " + x.WeekEnd.ToDateString()
                            }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("años-equipos")]
        public async Task<IActionResult> GetProjectEquipmentYears()
        {


            var data = await _context.ProjectCalendarWeeks
                            .OrderBy(x => x.Year)
                            .Select(x => new SelectViewModel<int>
                            {
                                Id = x.Year,
                                Text = x.Year.ToString()
                            }).Distinct().ToListAsync();

            return Ok(data);
        }


        [HttpGet("todo-semanas")]
        public async Task<IActionResult> GetAllWeeks()
        {
            var data = await _context.ProjectCalendarWeeks
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description + " - Del " + x.WeekStart.ToLocalDateFormat() + " Al " + x.WeekEnd.ToLocalDateFormat()
                }).AsNoTracking()
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("semanas-futuras")]
        public async Task<IActionResult> GetProjectFutureWeeks()
        {
            var today = DateTime.Today;

            var projectcalendar = await _context.ProjectCalendars
                .FirstOrDefaultAsync(x => x.ProjectId == GetProjectId() && 
                                          x.Year == today.Year && 
                                          x.IsWeekly);

            if (projectcalendar == null)
                return Ok(new List<SelectViewModel>());

            var data = await _context.ProjectCalendarWeeks
                            .Where(x => x.ProjectCalendarId == projectcalendar.Id &&
                                        x.WeekStart.Date >= today.Date)
                            .OrderBy(x => x.WeekStart)
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = x.Description + " - Del " + x.WeekStart.ToLocalDateFormat() + " Al " + x.WeekEnd.ToLocalDateFormat()
                            }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("anios-proyecto")]
        public async Task<IActionResult> GetProjectCalendars()
        {
            var anios = await _context.ProjectCalendars
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x=> new SelectIntViewModel
                {
                    Id = x.Year,
                    Text = x.Year.ToString()
                })
                .ToListAsync();

            return Ok(anios);
        }

        [HttpGet("administradoras-pensiones")]
        public async Task<IActionResult> GetPensionFundAdministrators()
        {
            var data = await _context.PensionFundAdministrators
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = x.Name
                            }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("unidades-de-medida")]
        public async Task<IActionResult> GetMeasurementUnits()
        {
            var data = await _context.MeasurementUnits
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = x.Abbreviation
                            }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("familias-de-insumos")]
        public async Task<IActionResult> GetSupplyFamilies()
        {
            var data = await _context.SupplyFamilies
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = $"{x.Code} - {x.Name}"
                            }).AsNoTracking().ToListAsync();

            return Ok(data);
        }

        [HttpGet("grupos-de-insumos-familia")]
        public async Task<IActionResult> GetSupplyGroups(Guid familyId)
        {
            var data = await _context.SupplyGroups
                            .Where(x => x.SupplyFamilyId == familyId)
                            .Select(x => new SelectViewModel
                            
                            {
                                Id = x.Id,
                                Text = $"{x.Code} - {(string.IsNullOrEmpty(x.Name) ? "LIBRE" : x.Name)}"
                            })
                            .ToListAsync();

            return Ok(data);
        }

        [HttpGet("grupos-de-insumos")]
        public async Task<IActionResult> GetSupplyGroups(Guid? familyId = null)
        {
            var data = await _context.SupplyGroups
                            .Where(x => (familyId.HasValue ? x.SupplyFamilyId == familyId.Value : true))
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = $"{x.Code} - {(string.IsNullOrEmpty(x.Name) ? "LIBRE" : x.Name)}"
                            }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("grupos-de-insumos-filtro")]
        public async Task<IActionResult> GetSupplyGroupsFiltered(Guid? supplyfamilyId = null)
        {
            var query = _context.SupplyGroups.Where(x => x.SupplyFamilyId != null);

            if (supplyfamilyId.HasValue)
                query = query.Where(x => x.SupplyFamilyId == supplyfamilyId);

            var data = await query
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = $"{x.Code} - {(string.IsNullOrEmpty(x.Name) ? "LIBRE" : x.Name)}"
                            }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("insumos")]
        public async Task<IActionResult> GetSupplies()
        {
            var data = await _context.Supplies
                            .Include(x => x.SupplyGroup)
                            .Include(x => x.SupplyFamily)
                            .Select(x => new SelectViewModel
                            {
                                Id = x.Id,
                                Text = $"{x.FullCode} - {x.Description}"
                            }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("titulos-de-presupuesto")]
        public async Task<IActionResult> GetBudgetTitles()
        {
            var data = await _context.BudgetTitles
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        

        [HttpGet("general-expenses-titulos-de-presupuesto")]
        public async Task<IActionResult> GetBudgetTitlesGeneralExpenses()
        {
            var data = await _context.BudgetTitles
                .Where(x => x.ProjectId == GetProjectId())
                .Where(x => x.Name.Contains("Adicional") || x.Name.Contains("Original"))
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("tipos-de-presupuesto")]
        public async Task<IActionResult> GetBudgetTypes()
        {
            var data = await _context.BudgetTypes
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("bancos")]
        public async Task<IActionResult> GetBanks(bool? existRegister = null)
        {
            var query = _context.Banks.AsQueryable();

            if (existRegister.HasValue)
                if (existRegister.Value == true)
                    query = query.Where(x => _context.BondAdds.Any(a => a.BankId == x.Id));

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }



        [HttpGet("insumos-de-presupuesto")]
        public async Task<IActionResult> GetBudgetInputs(Guid? familyId = null, Guid? groupId = null)
        {
            var data = await _context.BudgetInputs
                .Where(x => x.ProjectId == GetProjectId() &&
                            (familyId.HasValue ? x.SupplyFamilyId == familyId.Value : true) &&
                            (groupId.HasValue ? x.SupplyGroupId == groupId.Value : true))
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.FullName
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("insumos-de-meta")]
        public async Task<IActionResult> GetGoalBudgetInputs(Guid? workFrontId = null, Guid? groupId = null, Guid? projectFormulaId = null)
        {
            var data = await _context.GoalBudgetInputs
                .Include(x => x.Supply)
                .Include(x => x.WorkFront)
                .Include(x => x.ProjectFormula)
                .Include(x => x.Supply.SupplyGroup)
                .Where(x => x.ProjectFormula.ProjectId == GetProjectId() &&
                            (workFrontId.HasValue ? x.WorkFrontId == workFrontId.Value : true) &&
                            (groupId.HasValue ? x.Supply.SupplyGroupId == groupId.Value : true) && x.Parcial >= 0 &&
                            (projectFormulaId.HasValue ? x.ProjectFormulaId == projectFormulaId.Value : true))
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Supply.Description
                })
                .OrderBy(x => x.Text)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("insumos-de-meta-pedidos-campo")]
        public async Task<IActionResult> GetGoalBudgetInputsByFieldRequests(Guid fieldRequestId,Guid? workFrontId = null, Guid? supplyFamilyId = null)
        {
            var formulas = await _context.FieldRequestProjectFormulas
                .Where(x => x.FieldRequestId == fieldRequestId)
                .Select(x => x.ProjectFormulaId)
                .ToListAsync();

            var data = await _context.GoalBudgetInputs
                .Include(x => x.ProjectFormula)
                .Include(x => x.Supply)
                .Where(x => x.ProjectFormula.ProjectId == GetProjectId() &&
                            (workFrontId.HasValue ? x.WorkFrontId == workFrontId.Value : true) &&
                            (supplyFamilyId.HasValue ? x.Supply.SupplyFamilyId == supplyFamilyId.Value : true) && x.Parcial >= 0 &&
                            (formulas.Contains((Guid)x.ProjectFormulaId)))
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Supply.Description + " - " + x.ProjectFormula.Code
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("insumos-de-meta-pedidos-campo-app")]
        public async Task<IActionResult> GetGoalBudgetInputsByFieldRequestsApp(Guid fieldRequestId, Guid projectId, Guid? workFrontId = null, Guid? supplyFamilyId = null)
        {
            var formulas = await _context.FieldRequestProjectFormulas
                .Where(x => x.FieldRequestId == fieldRequestId)
                .Select(x => x.ProjectFormulaId)
                .ToListAsync();

            var data = await _context.GoalBudgetInputs
                .Include(x => x.ProjectFormula)
                .Include(x => x.Supply)
                .Where(x => x.ProjectFormula.ProjectId == projectId &&
                            (workFrontId.HasValue ? x.WorkFrontId == workFrontId.Value : true) &&
                            (supplyFamilyId.HasValue ? x.Supply.SupplyFamilyId == supplyFamilyId.Value : true) && x.Parcial >= 0 &&
                            (formulas.Contains((Guid)x.ProjectFormulaId)))
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Supply.Description + " - " + x.ProjectFormula.Code
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("insumos-de-meta-app")]
        public async Task<IActionResult> GetGoalBudgetInputsApp(Guid? workFrontId = null, Guid? groupId = null, Guid? projectFormulaId = null,Guid? projectId = null)
        {
            var data = await _context.GoalBudgetInputs
                .Include(x => x.ProjectFormula)
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Where(x => x.ProjectFormula.ProjectId == projectId.Value &&
                            (workFrontId.HasValue ? x.WorkFrontId == workFrontId.Value : true) &&
                            (groupId.HasValue ? x.Supply.SupplyGroupId == groupId.Value : true) && x.Parcial >= 0 &&
                            (projectFormulaId.HasValue ? x.ProjectFormulaId == projectFormulaId.Value : true))
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Supply.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("proveedores")]
        public async Task<IActionResult> GetProviders(string familyCode, string groupCode)
        {
            var data = new List<SelectViewModel>();

            if (!string.IsNullOrEmpty(familyCode))
                data.AddRange(
                        await _context.ProviderSupplyFamilies
                        .Include(x => x.Provider)
                        .Include(x => x.SupplyFamily)
                        .Where(x => x.SupplyFamily.Code == familyCode)
                        .Select(x => new SelectViewModel
                        {
                            Id = x.ProviderId,
                            Text = string.IsNullOrEmpty(x.Provider.Tradename)
                            ? x.Provider.BusinessName : x.Provider.Tradename
                        }).ToListAsync()
                    );

            if (!string.IsNullOrEmpty(groupCode))
                data.AddRange(
                        await _context.ProviderSupplyGroups
                        .Include(x => x.Provider)
                        .Include(x => x.SupplyGroup)
                        .Where(x => x.SupplyGroup.Code == groupCode)
                        .Select(x => new SelectViewModel
                        {
                            Id = x.ProviderId,
                            Text = string.IsNullOrEmpty(x.Provider.Tradename)
                            ? x.Provider.BusinessName : x.Provider.Tradename
                        }).ToListAsync()
                    );

            var noDupesData = data.GroupBy(x => x.Id).Select(x => x.First()).ToList();

            return Ok(noDupesData);
        }

        [HttpPost("proveedores-requerimientos")]
        public async Task<IActionResult> GetProvidersByRequests(List<Guid> reqIds)
        {
            var data = new List<SelectViewModel>();

            foreach(var id in reqIds)
            {
                var req = await _context.Requests
                    //.Include(x => x.SupplyFamily)
                    .FirstAsync(x => x.Id == id);

                /*
                data.AddRange(
                            await _context.ProviderSupplyFamilies
                            .Include(x => x.Provider)
                            .Include(x => x.SupplyFamily)
                            .Where(x => x.SupplyFamily.Code == req.SupplyFamily.Code)
                            .Select(x => new SelectViewModel
                            {
                                Id = x.ProviderId,
                                Text = string.IsNullOrEmpty(x.Provider.Tradename)
                                ? x.Provider.BusinessName : x.Provider.Tradename
                            }).ToListAsync());
                */

                var folding = _context.RequestItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .AsEnumerable().Where(x => x.RequestId == id)
                  .GroupBy(x => x.Supply.SupplyGroupId).Where(x => x.Count() > 0);

                foreach (var item in folding)
                {
                    data.AddRange(
                            await _context.ProviderSupplyGroups
                            .Include(x => x.Provider)
                            .Include(x => x.SupplyGroup)
                            .Where(x => x.SupplyGroup.Code == item.FirstOrDefault().Supply.SupplyGroup.Code)
                            .Select(x => new SelectViewModel
                            {
                                Id = x.ProviderId,
                                Text = string.IsNullOrEmpty(x.Provider.Tradename)
                                ? x.Provider.BusinessName : x.Provider.Tradename
                            }).ToListAsync()
                        );
                }

            }

            var noDupesData = data.GroupBy(x => x.Id).Select(x => x.First()).ToList();

            return Ok(noDupesData);
        }

        [HttpGet("requerimientos")]
        public async Task<IActionResult> GetRequests(int reqType, int type = 0)
        {
            var requests = _context.Requests
                .Include(x => x.Project)
                .Include(x => x.BudgetTitle)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.ProjectFormula)
                .Where(x => (x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED
                || x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ORDER_C
                || x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ORDER_S) && 
                (x.AttentionStatus == ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING
                || x.AttentionStatus == ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL))
                .Where(x => x.RequestType == reqType && x.ProjectId == GetProjectId())
                .ToList();

            var requestItems = await _context.RequestItems
                  .Include(x => x.Supply)
                  .Include(x => x.Supply.SupplyGroup)
                  .Where(x => requests.Select(y => y.Id).Contains(x.RequestId))
                  .ToListAsync();

            var users = await _context.Users.ToListAsync();

            //if (type == 1)
            //    requests = requests.Where(x => x.AttentionStatus != ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL);

            var data = new List<SelectViewModel>();

            foreach (var item in requests)
            {
                var requestTypeStr = "";

                var groups = "";

                if (item.RequestType == 1)
                    requestTypeStr = "Compra";
                else
                    requestTypeStr = "Servicio";

                var folding = requestItems
                  .AsEnumerable().Where(x => x.RequestId == item.Id)
                  .GroupBy(x => x.Supply.SupplyGroupId).Where(x => x.Count() > 0);

                var userString = users.FirstOrDefault(x => x.Id == item.IssuedUserId).FullName;
                //var reqUsers = requestUsers.Where(x => x.RequestId == item.Id).ToList();
                //foreach (var reqUser in reqUsers)
                //{
                //    var last = reqUsers.Last();
                //    var user = users.FirstOrDefault(x => x.Id == reqUser.UserId);
                //    if (reqUser == last)
                //        userString = userString + user.FullName;
                //    else
                //        userString = userString + user.FullName + " - ";
                //}

                foreach (var itemF in folding)
                {
                    groups += itemF.FirstOrDefault().Supply.SupplyGroup.Name + " ";
                }

                data.Add(new SelectViewModel
                {
                    Id = item.Id,
                    Text = item.CorrelativePrefix + "-" + item.CorrelativeCode.ToString("D4") + " | Tipo: " + requestTypeStr + " | F. Entrega: " + item.DeliveryDate.Value.ToDateString()
                    + " | Grupos: " + groups + " | Solicitantes: " + userString
                    //+ " | Familia: " + item.SupplyFamily.Name + " | Grupos: " + groups + " | Solicitantes: " + userString
                });
            }

            return Ok(data);
        }

        [HttpGet("requerimientos/{id}/elementos")]
        public async Task<IActionResult> GetRequestItems(Guid id)
        {
            var data = await _context.RequestItems
                .Where(x => x.RequestId == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Supply.Description
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("requerimientos/archivos/{id}")]
        public async Task<IActionResult> GetRequestFiles(Guid id)
        {
            var files = await _context.RequestFiles
                .Where(x => x.RequestId == id)
                .Select(x => new SelectViewModel<Uri>
                {
                    Id = x.FileUrl,
                    Text = System.IO.Path.GetFileName(x.FileUrl.LocalPath)
                }).ToListAsync();

            return Ok(files);
        }

        [HttpGet("requerimientos/archivos-tspec/{id}")]
        public async Task<IActionResult> GetRequestFilesAndThecnicalSpecs(Guid id)
        {
            var files = new List<SelectViewModel<Uri>>();

            var requests = await _context.RequestFiles.Where(x => x.RequestId == id).ToListAsync();

            foreach (var item in requests)
            {
                files.Add(new SelectViewModel<Uri>
                {
                    Id = item.FileUrl,
                    Text = System.IO.Path.GetFileName(item.FileUrl.LocalPath)
                });
            }

            var thecnicalSpecs = await _context.TechnicalSpecs
                .ToListAsync();

            var folding = _context.RequestItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .AsEnumerable().Where(x => x.RequestId == id)
                  .GroupBy(x => x.Supply.SupplyGroupId).Where(x => x.Count() > 0);

            foreach(var item in folding)
            {
                var thecnicalSpec = thecnicalSpecs
                    .FirstOrDefault(x => x.SupplyFamilyId == item.FirstOrDefault().Supply.SupplyFamilyId
                    & x.SupplyGroupId == item.FirstOrDefault().Supply.SupplyGroupId);

                if (thecnicalSpec != null)
                    files.Add(new SelectViewModel<Uri>
                    {
                        Id = thecnicalSpec.FileUrl,
                        Text = "Especificación Técnica - " + item.FirstOrDefault().Supply.SupplyGroup.Name
                    });
            }

            var data = files.ToList();

            return Ok(data);
        }

        [HttpGet("requerimientos/thecnical-spec/{id}")]
        public async Task<IActionResult> GetRequestThecnicalSpecs(Guid id)
        {
            var files = new List<SelectViewModel<Uri>>();

            var folding = _context.RequestItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .AsEnumerable().Where(x => x.RequestId == id)
                  .GroupBy(x => x.Supply.SupplyGroupId).Where(x => x.Count() > 0);

            foreach (var item in folding)
            {
                var thecnicalSpec = await _context.TechnicalSpecs
                    .FirstOrDefaultAsync(x => x.SupplyFamilyId == item.FirstOrDefault().Supply.SupplyFamilyId
                    & x.SupplyGroupId == item.FirstOrDefault().Supply.SupplyGroupId);

                if (thecnicalSpec != null)
                    files.Add(new SelectViewModel<Uri>
                    {
                        Id = thecnicalSpec.FileUrl,
                        Text = "Especificación Técnica - " + item.FirstOrDefault().Supply.SupplyGroup.Name
                    });
            }

            var data = files.ToList();

            return Ok(data);
        }


        [HttpGet("pre-requerimientos")]
        public async Task<IActionResult> GetPreRequests(int type = 0)
        {
            var pId = GetProjectId();
            
            var requests = await _context.PreRequests
                .Include(x => x.Project)
                .Include(x => x.BudgetTitle)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectId == pId && x.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED 
                && x.AttentionStatus != ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL &&
                (type != 0 ? (type == 1 ? x.RequestType == ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE : 
                x.RequestType == ConstantHelpers.Logistics.RequestOrder.Type.SERVICE):true))
                .ToListAsync();

            var preItems = await _context.PreRequestItems
                  .Include(x => x.Supply)
                  .Include(x => x.Supply.SupplyGroup)
                  .ToListAsync();

            var data = new List<SelectViewModel>();

            foreach(var item in requests)
            {
                if (preItems.FirstOrDefault(x => x.PreRequestId == item.Id
                && x.SupplyId == null) != null)
                    continue;

                var requestTypeStr = "";

                var groups = "";

                if (item.RequestType == 1)
                    requestTypeStr = "Compra";
                else
                    requestTypeStr = "Servicio";

                var folding = preItems
                  .AsEnumerable().Where(x => x.PreRequestId == item.Id
                  && x.SupplyId != null)
                  .GroupBy(x => x.Supply.SupplyGroupId).Where(x => x.Count() > 0);

                foreach (var itemF in folding)
                {
                    groups += itemF.FirstOrDefault().Supply.SupplyGroup.Name + " ";
                }

                var userString = _context.Users.FirstOrDefault(x => x.Id == item.IssuedUserId).FullName;
                //var reqUsers = await _context.PreRequestUsers.Where(x => x.PreRequestId == item.Id).ToListAsync();
                //foreach (var reqUser in reqUsers)
                //{
                //    var last = reqUsers.Last();
                //    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == reqUser.UserId);
                //    if (reqUser == last)
                //        userString = userString + user.FullName;
                //    else
                //        userString = userString + user.FullName + " - ";
                //}

                data.Add(new SelectViewModel
                {
                    Id = item.Id,
                    Text = item.CorrelativePrefix + item.CorrelativeCode.ToString("D4") + " | Tipo: " + requestTypeStr + " | F. Entrega: " + item.DeliveryDate.Value.ToDateString()
                    + " | Grupos: " + groups + " | Solicitantes: " + userString
                    //+ " | Familia: " + item.SupplyFamily.Name + " | Grupos: " + groups + " | Solicitantes: " + userString
                });
            }

            return Ok(data);
        }

        [HttpGet("pre-requerimientos/archivos/{id}")]
        public async Task<IActionResult> GetPreRequestFiles(Guid id)
        {
            var files = await _context.PreRequestFiles
                .Where(x => x.PreRequestId == id)
                .Select(x => new SelectViewModel<Uri>
                {
                    Id = x.FileUrl,
                    Text = System.IO.Path.GetFileName(x.FileUrl.LocalPath)
                }).ToListAsync();

            return Ok(files);
        }

        [HttpGet("pre-requerimientos/archivos-tspec/{id}")]
        public async Task<IActionResult> GetPreRequestFilesAndThecnicalSpecs(Guid id)
        {
            var files = new List<SelectViewModel<Uri>>();

            var requests = await _context.PreRequestFiles.Where(x => x.PreRequestId == id).ToListAsync();

            foreach (var item in requests)
            {
                files.Add(new SelectViewModel<Uri>
                {
                    Id = item.FileUrl,
                    Text = System.IO.Path.GetFileName(item.FileUrl.LocalPath)
                });
            }

            var data = files.ToList();

            return Ok(data);
        }

        [HttpGet("pre-requerimientos/thecnical-spec/{id}")]
        public async Task<IActionResult> GetPreRequestThecnicalSpecs(Guid id)
        {
            var files = new List<SelectViewModel<Uri>>();

            var folding = _context.PreRequestItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .AsEnumerable().Where(x => x.PreRequestId == id
                && x.SupplyId != null)
                .GroupBy(x => x.Supply.SupplyGroupId).Where(x => x.Count() > 0);

            foreach (var item in folding)
            {
                var thecnicalSpec = await _context.TechnicalSpecs
                    .FirstOrDefaultAsync(x => x.SupplyFamilyId == item.FirstOrDefault().Supply.SupplyFamilyId
                    & x.SupplyGroupId == item.FirstOrDefault().Supply.SupplyGroupId);

                if (thecnicalSpec != null)
                    files.Add(new SelectViewModel<Uri>
                    {
                        Id = thecnicalSpec.FileUrl,
                        Text = "Especificación Técnica - " + item.FirstOrDefault().Supply.SupplyGroup.Name
                    });
            }

            var data = files.ToList();

            return Ok(data);
        }

        [HttpGet("ordenes")]
        public async Task<IActionResult> GetOrders(int orderType, int type = 0)
        {
            var orders = _context.Orders
                //.Include(x => x.Project)
                //.Include(x => x.BudgetTitle)
                //.Include(x => x.SupplyFamily)
                //.Include(x => x.ProjectFormula)
                .Include(x => x.Provider)
                .Include(x => x.Warehouse.WarehouseType)
                .Include(x => x.Project)
                .Where(x => x.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED)
                .Where(x => x.Type == orderType && x.ProjectId == GetProjectId()).ToList();

            var usuarios = await _context.Users
                .Include(x => x.WorkAreaEntity)
                .ToListAsync();

            var userRoles = await _context.UserRoles
                .Include(x => x.Role)
                .ToListAsync();

            var ordItems = await _context.OrderItems
                    .Include(x => x.Supply)
                    .Include(x => x.Supply.SupplyGroup)
                    .ToListAsync();

            var data = new List<SelectViewModel>();

            foreach (var item in orders)
            {
                var orderTypeStr = "";

                var groups = "";

                var ob = "";

                if (item.Type == 1)
                    orderTypeStr = "Compra";
                else
                    orderTypeStr = "Servicio";

                var folding = ordItems
                    .AsEnumerable().Where(x => x.OrderId == item.Id)
                    .GroupBy(x => x.Supply.SupplyGroupId).Where(x => x.Count() > 0);

                /*
                var userString = "";
                var reqUsers = await _context.RequestUsers.Where(x => x.RequestId == item.Req.Id).ToListAsync();
                foreach (var reqUser in reqUsers)
                {
                    var last = reqUsers.Last();
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == reqUser.UserId);
                    if (reqUser == last)
                        userString = userString + user.FullName;
                    else
                        userString = userString + user.FullName + " - ";
                }
                */
                foreach (var itemF in folding)
                {
                    groups += itemF.FirstOrDefault().Supply.SupplyGroup.Name + " ";
                }

                if (usuarios.FirstOrDefault(x => x.Id == item.IssuedUserId).WorkAreaEntity.Name == "Logística"
                        && userRoles.FirstOrDefault(x => x.Role.Name == "Oficina Técnica" && x.UserId == item.IssuedUserId) != null)
                    ob += "-OB";

                data.Add(new SelectViewModel
                {
                    Id = item.Id,
                    Text = item.Project.CostCenter + "-" + item.CorrelativeCode.ToString("D4")
                    + item.CorrelativeCodeSuffix + ob + "-" + item.ReviewDate.Value.Year.ToString()
                    + " | Tipo: " + orderTypeStr + " | Proveedor: " + item.Provider.Tradename + " | Grupos: " + groups
                });
            }

            return Ok(data);
        }

        [HttpGet("Facturas")]
        public async Task<IActionResult> GetInvoices()
        {
            var data = await _context.Invoices
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Serie
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("presupuestos")]
        public async Task<IActionResult> GetBudgets()
        {
            var data = await _context.Budgets
                //.Where(x => x.BudgetFormula.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("cartas-fianza/renovaciones/{id}")]
        public async Task<IActionResult> GetBondRenovationFiles(Guid id)
        {
            var data = await _context.BondRenovations
                .Include(x => x.BondAdd)
                .Where(x => x.BondAddId == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.BondAdd.BondNumber}-{x.BondOrder}"
                }).ToListAsync();

            data = data.OrderByDescending(x => x.Text).ToList();

            return Ok(data);
        }

        [HttpGet("permisos/renovaciones/{id}")]
        public async Task<IActionResult> GetPermissionRenovationFiles(Guid id)
        {
            var data = await _context.PermissionRenovations
                .Include(x => x.Permission)
                .Where(x => x.PermissionId == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Order}"
                }).ToListAsync();

            data = data.OrderByDescending(x => x.Text).ToList();

            return Ok(data);
        }

        [HttpGet("documentos-legal/renovaciones/{id}")]
        public async Task<IActionResult> GetLegalDocumentationRenovationFiles(Guid id)
        {
            var data = await _context.LegalDocumentationRenovations
                .Include(x => x.LegalDocumentation)
                .Include(x => x.LegalDocumentation.LegalDocumentationType)
                .Where(x => x.LegalDocumentationId == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.LegalDocumentation.LegalDocumentationType.Name}-{x.LegalDocumentationOrder}"
                }).ToListAsync();

            data = data.OrderByDescending(x => x.Text).ToList();

            return Ok(data);
        }

        [HttpGet("habilidades/renovaciones/{id}")]
        public async Task<IActionResult> GetSkillsRenovationFiles(Guid id)
        {
            var data = await _context.SkillRenovations
                .Include(x => x.Skill)
                .Include(x => x.Skill.Professional)
                .Where(x => x.SkillId == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Skill.Professional.FullName}-{x.SkillOrder}"
                }).ToListAsync();

            data = data.OrderByDescending(x => x.Text).ToList();

            return Ok(data);
        }
        [HttpGet("equipos-certificados/renovaciones/{id}")]
        public async Task<IActionResult> GetEquipmentRenewalsFiles(Guid id)
        {
            var data = await _context.EquipmentCertificateRenewals
                .Include(x => x.EquipmentCertificate)
                .Where(x => x.EquipmentCertificateId == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentCertificate.Name}-{x.RenewalOrder}"
                }).ToListAsync();

            data = data.OrderByDescending(x => x.Text).ToList();

            return Ok(data);
        }

        [HttpGet("patron-calibracion/renovaciones/{id}")]
        public async Task<IActionResult> GetPatternRenewalsFiles (Guid id)
        {
            var data = await _context.PatternCalibrationRenewals
                .Include(x => x.PatternCalibration)
                .Where(x => x.PatternCalibrationId == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.PatternCalibration.Name}-{x.RenewalOrder}"
                }).ToListAsync();

            data = data.OrderByDescending(x => x.Text).ToList();

            return Ok(data);
        }

        [HttpGet("materiales")]
        public async Task<IActionResult> GetStocks()
        {
            var data = await _context.Stocks
                .Select(x => new SelectViewModel
                {
                    Id = x.Id
                }).OrderBy(x => x.Text)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("profesionales-lista")]
        public async Task<IActionResult> GetProfessionals()
        {
            var data = await _context.Professionals
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.PaternalSurname} {x.MaternalSurname} {x.Name}"
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("colectores-descarga")]
        public async Task<IActionResult> GetSewerManifolds()
        {
            var data = await _context.SewerManifolds
                .Include(x => x.SewerBoxStart)
                .Where(x => x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.REVIEW)
                .OrderBy(x => x.SewerBoxStart.SewerOrderNumber)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("colectores-descarga-carta-solicitudes")]
        public async Task<IActionResult> GetSewerManifoldRequestLetters(Guid? smId = null)
        {
            if (!smId.HasValue)
                return Ok(new List<SelectViewModel>());

            var letters = await _context.SewerManifoldLetters
                .Include(x => x.Letter)
                .Where(x => x.SewerManifoldId == smId.Value 
                    && x.LetterType == ConstantHelpers.Sewer.Manifolds.Letters.REQUEST)
                .Select(x => new SelectViewModel
                {
                    Id = x.LetterId,
                    Text = x.Letter.Name
                }).ToListAsync();

            return Ok(letters);
        }

        [HttpGet("colectores-descarga-carta-aprobaciones")]
        public async Task<IActionResult> GetSewerManifoldApprovalLetters(Guid? smId = null)
        {
            if (!smId.HasValue)
                return Ok(new List<SelectViewModel>());

            var letters = await _context.SewerManifoldLetters
                .Include(x => x.Letter)
                .Where(x => x.SewerManifoldId == smId.Value
                    && x.LetterType == ConstantHelpers.Sewer.Manifolds.Letters.APPROVAL)
                .Select(x => new SelectViewModel
                {
                    Id = x.LetterId,
                    Text = x.Letter.Name
                }).ToListAsync();

            return Ok(letters);
        }

        [HttpGet("colectores-descarga-ejecucion-for47")]
        public async Task<IActionResult> GetExecutionSewerManifoldsFor47()
        {
            var data = await _context.SewerManifolds
                .Where(x => x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.EXECUTION && !x.HasFor47)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("colectores-descarga-ejecucion-for47/{id}")]
        public async Task<IActionResult> GetExecutionSewerManifoldsFor47Id(Guid id)
        {
            var data = await _context.SewerManifoldFor47s
                .Include(x => x.SewerManifold)
                .Where(x => x.Id == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.SewerManifoldId,
                    Text = x.SewerManifold.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("colectores-descarga-ejecucion-for05-hasfor01")]
        public async Task<IActionResult> GetExecutionSewerManifoldHasFor01()
        {
            var data = await _context.SewerManifolds
                .Where(x => x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.EXECUTION && x.HasFor01 && !x.HasFor05)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("colectores-descarga-ejecucion-for05/{id}")]
        public async Task<IActionResult> GetExecutionSewerManifoldsFor05Id(Guid id)
        {
            var data = await _context.SewerManifoldFor05s
                .Include(x => x.SewerManifold)
                .Where(x => x.Id == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.SewerManifoldId,
                    Text = x.SewerManifold.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("colectores-descarga-ejecucion-for37A-hasfor01")]
        public async Task<IActionResult> GetExecutionSewerManifoldFor37AHasFor01()
        {
            var data = await _context.SewerManifolds
                .Where(x => x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.EXECUTION && !x.HasFor37A)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("colectores-descarga-ejecucion-for37A/{id}")]
        public async Task<IActionResult> GetExecutionSewerManifoldsFor37AId(Guid id)
        {
            var data = await _context.SewerManifoldFor37As
                .Include(x => x.SewerManifold)
                .Where(x => x.Id == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.SewerManifoldId,
                    Text = x.SewerManifold.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("colectores-descarga-ejecucion-for29/{id}")]
        public async Task<IActionResult> GetExecutionSewerManifoldsFor29Id(Guid id)
        {
            var data = await _context.SewerManifoldFor29s
                .Include(x => x.SewerManifold)
                .Where(x => x.Id == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.SewerManifoldId,
                    Text = x.SewerManifold.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("colectores-descarga-ejecucion-for29-hasfor01")]
        public async Task<IActionResult> GetExecutionSewerManifoldFor29HasFor01()
        {
            var data = await _context.SewerManifolds
                .Where(x => x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.EXECUTION && x.HasFor01 && !x.HasFor29)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("colectores-descarga-ejecucion")]
        public async Task<IActionResult> GetExecutionSewerManifolds()
        {
            var data = await _context.SewerManifolds
                .Where(x => x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.EXECUTION && !x.HasFor01)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        } 

        [HttpGet("colectores-descarga-ejecucion/{id}")]
        public async Task<IActionResult> GetExecutionSewerManifoldsId(Guid id)
        {
            var data = await _context.DischargeManifolds
                .Include(x => x.SewerManifold)
                .Where(x => x.Id == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.SewerManifoldId,
                    Text = x.SewerManifold.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("series-certificado-equipo")]
        public async Task<IActionResult> GetSerialEQuipmentCertificate()
        {
            var data = await _context.EquipmentCertificates
                .Include(x=>x.EquipmentCertificateType)
                .Where(x=>x.EquipmentCertificateType.CertificateTypeName == "Topografía")
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Serial
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("patrones-referencias")]
        public async Task<IActionResult> GetPatternRenewalReferences(Guid id)
        {
            var query = await _context.EquipmentCertificateRenewals
                .Include(x => x.PatternCalibrationRenewal)
                .Where(x => x.Id == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.PatternCalibrationRenewalId.Value,
                    Text = x.PatternCalibrationRenewal.ReferenceNumber
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("origins-type-laboratory")]
        public async Task<IActionResult> GetOriginsType()
        {
            var data = await _context.OriginTypeFillingLaboratories
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.OriginTypeFLName
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("sig-process")]
        public async Task<IActionResult> GetSIGProcess()
        {
            var data = await _context.NewSIGProcesses
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.ProcessName
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("tipo-de-cambio-licitaciones")]
        public async Task<IActionResult> GetBiddingCurrencyType()
        {
            var data = await _context.BiddingCurrencyTypes
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Currency.ToString()
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("todos-colectores-descarga-ejecucion")]
        public async Task<IActionResult> GetAllExecutionSewerManifolds()
        {
            var data = await _context.SewerManifolds
                .Where(x => x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.EXECUTION)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("tipos-de-maquinaria-mixto")]
        public async Task<IActionResult> GetAllEquipmentMachineryTypes()
        {
            var data = await _context.EquipmentMachineryTypeSofts
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();
            var data2 = await _context.EquipmentMachineryTypeTypes
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = x.Description
                 }).ToListAsync();

            var data3 = await _context.EquipmentMachineryTypeTransports

                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();
            return Ok(data.Union(data2).Union(data3));
        }

        [HttpGet("tipos-de-maquinaria-mixto-por-clase")]
        public async Task<IActionResult> GetAllEquipmentMachineryTypesByClass(Guid? equipmentMachineryTypeId = null)
        {

            if (equipmentMachineryTypeId == null)
                return Ok(new List<SelectViewModel>());


            var data = await _context.EquipmentMachineryOperators
                .Include(x=>x.EquipmentMachineryTypeSoft)
                .Where(x=>x.EquipmentMachineryTypeId == equipmentMachineryTypeId.Value && x.EquipmentMachineryTypeSoftId != null).
                Select(x => new SelectViewModel
            {
                Id = x.EquipmentMachineryTypeSoftId.Value,
                Text = x.EquipmentMachineryTypeSoft.Description
            }).Distinct().ToListAsync();

            var data2 = await _context.EquipmentMachineryOperators
     .Include(x => x.EquipmentMachineryTypeType)
     .Where(x => x.EquipmentMachineryTypeId == equipmentMachineryTypeId.Value && x.EquipmentMachineryTypeTypeId != null).
     Select(x => new SelectViewModel
     {
         Id = x.EquipmentMachineryTypeTypeId.Value,
         Text = x.EquipmentMachineryTypeType.Description
     }).Distinct().ToListAsync();

            var data3 = await _context.EquipmentMachineryOperators
.Include(x => x.EquipmentMachineryTypeTransport)
.Where(x => x.EquipmentMachineryTypeId == equipmentMachineryTypeId.Value && x.EquipmentMachineryTypeTransportId != null).
Select(x => new SelectViewModel
{
Id = x.EquipmentMachineryTypeTransportId.Value,
Text = x.EquipmentMachineryTypeTransport.Description
}).Distinct().ToListAsync();


            return Ok(data.Union(data2).Union(data3));
        }



        [HttpGet("tipos-de-maquinaria-mixto-por-clase-folding")]
        public async Task<IActionResult> GetAllEquipmentMachineryTypesByClassProviderFolding(Guid? equipmentMachineryTypeId = null)
        {

            if (equipmentMachineryTypeId == null)
                return Ok(new List<SelectViewModel>());


            var data = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentMachineryTypeSoft)
                .Where(x => x.EquipmentMachineryTypeId == equipmentMachineryTypeId.Value && x.EquipmentMachineryTypeSoftId != null).
                Select(x => new SelectViewModel
                {
                    Id = x.EquipmentMachineryTypeSoftId.Value,
                    Text = x.EquipmentMachineryTypeSoft.Description
                }).Distinct().ToListAsync();

            var data2 = await _context.EquipmentProviderFoldings
     .Include(x => x.EquipmentMachineryTypeType)
     .Where(x => x.EquipmentMachineryTypeId == equipmentMachineryTypeId.Value && x.EquipmentMachineryTypeTypeId != null).
     Select(x => new SelectViewModel
     {
         Id = x.EquipmentMachineryTypeTypeId.Value,
         Text = x.EquipmentMachineryTypeType.Description
     }).Distinct().ToListAsync();

            var data3 = await _context.EquipmentProviderFoldings
.Include(x => x.EquipmentMachineryTypeTransport)
.Where(x => x.EquipmentMachineryTypeId == equipmentMachineryTypeId.Value && x.EquipmentMachineryTypeTransportId != null).
Select(x => new SelectViewModel
{
    Id = x.EquipmentMachineryTypeTransportId.Value,
    Text = x.EquipmentMachineryTypeTransport.Description
}).Distinct().ToListAsync();


            return Ok(data.Union(data2).Union(data3));
        }

        [HttpGet("tipos-de-maquinaria-liviano")]
        public async Task<IActionResult> GetEquipmentMachineryTypeSoft()
        {
            var data = await _context.EquipmentMachineryTypeSofts
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("tipos-de-maquinaria-transporte")]
        public async Task<IActionResult> GetEquipmentMachineryTypeTransport()
        {
            var data = await _context.EquipmentMachineryTypeTransports
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("tipos-de-maquinaria-maquinaria")]
        public async Task<IActionResult> GetAllEquipmentMachineryTypeType()
        {
            var data2 = await _context.EquipmentMachineryTypeTypes
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = x.Description
                 }).ToListAsync();

            return Ok(data2);
        }

        [HttpGet("tipos-de-maquinaria-lista")]
        public async Task<IActionResult> GetAllEquipmentMachineryType()
        {
            var data2 = await _context.EquipmentMachineryTypes
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = x.Description
                 }).ToListAsync();

            return Ok(data2);
        }



        [HttpGet("habilitaciones-proyecto")]
        public async Task<IActionResult> GetAllProjectHabilitations(Guid? projectId = null)
        {
            if (projectId == null)
                projectId = GetProjectId();

            var query = await _context.ProjectHabilitations
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                })
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("habilitaciones-cuadrilla")]
        public async Task<IActionResult> GetProjectHabilitationBySewerGroup(Guid? sewerGroupId = null)
        {
            if (sewerGroupId == null)
                return Ok(new List<SelectViewModel>());

            var query = await _context.SewerGroupProjectHabilitations
                .Include(x => x.ProjectHabilitation)
                .Where(x => x.SewerGroupId == sewerGroupId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.ProjectHabilitation.Id,
                    Text = x.ProjectHabilitation.LocationCode + "-" + x.ProjectHabilitation.Description
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("actividades-cuadrilla")]
        public async Task<IActionResult> GetFormulaActivitiesBySewerGroup(Guid? sewerGroupId = null)
        {
            if (sewerGroupId == null)
                return Ok(new List<SelectViewModel>());

            var formulaIds = await _context.ProjectFormulaSewerGroups
                .Where(x => x.SewerGroupId == sewerGroupId.Value)
                .Select(x => x.ProjectFormulaId)
                .ToListAsync();

            var query = await _context.ProjectFormulaActivities
                .Include(x => x.MeasurementUnit)
                .Where(x => formulaIds.Contains(x.ProjectFormulaId))
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Description} - {x.MeasurementUnit.Abbreviation}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("lista-proveedores")]
        public async Task<IActionResult> GetAllProviders()
        {
            var data2 = await _context.Providers
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = $"{x.Tradename}"
                 }).ToListAsync();

            return Ok(data2);
        }

        [HttpGet("lista-proveedores-ordenes")]
        public async Task<IActionResult> GetAllProvidersInOrder()
        {
            var data = await _context.Orders
                 .Include(x=>x.Provider)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.ProviderId,
                     Text = x.Provider.Tradename
                 })
                 .Distinct()
                 .ToListAsync();

            return Ok(data);
        }


        [HttpGet("lista-proveedores-grupos")]
        public async Task<IActionResult> GetAllProvidersSupplyGroup(Guid? supplyGroupId = null)
        {
            var data = await _context.ProviderSupplyGroups
                 .Include(x => x.Provider)
                 .Where(x => supplyGroupId.HasValue ? x.SupplyGroupId == supplyGroupId : true)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.ProviderId,
                     Text = x.Provider.Tradename
                 })
                 .Distinct()
                 .ToListAsync();

            return Ok(data);
        }

        [HttpGet("lista-proveedores-ingreso-materiales")]
        public async Task<IActionResult> GetAllProvidersSupplyEntry()
        {
            var data = await _context.SupplyEntries
                .Include(x => x.Order.Provider)
                .Include(x => x.Warehouse.WarehouseType)
                .Where(x => x.Warehouse.WarehouseType.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Order.ProviderId,
                    Text = $"{x.Order.Provider.Tradename}"
                })
                .Distinct()
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("ingreso-materiales")]
        public async Task<IActionResult> GetAllUnvaluedSupplyEntry()
        {
            var data = await _context.SupplyEntries
                .Include(x => x.SupplyEntryItems)
                .Where(x => x.Warehouse.WarehouseType.ProjectId == GetProjectId() && x.SupplyEntryItems.Any(y => y.IsValued == false) == true
                && x.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.RemissionGuide}"
                })
                .Distinct()
                .ToListAsync();

            return Ok(data);
        }

        //[HttpGet("asignados-equipos")]
        //public async Task<IActionResult> GetAssignedUsers()
        //{
        //    var data = await _context.EquipmentMachineryAssignedUsers
        //        .Include(x=>x.WorkFrontHead)
        //        .Where(x=>x.ProjectId == GetProjectId())
        //        .Select(x => new SelectViewModel
        //        {
        //            Id = x.Id,
        //            Text = !string.IsNullOrEmpty(x.WorkFrontHead.UserId)
        //                ? $"{x.WorkFrontHead.User.FullName} ({x.WorkFrontHead.Code})" : $"No Asignado ({x.WorkFrontHead.Code})"
        //        }).Distinct()
        //        .ToListAsync();

        //    return Ok(data);
        //}

        [HttpGet("proveedores-de-equipos")]
        public async Task<IActionResult> GetEquipmentProviders()
        {
            var data2 = await _context.EquipmentProviders
                .Include(x => x.Provider)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = $"{x.Provider.BusinessName}"
                 }).ToListAsync();

            return Ok(data2);
        }


        [HttpGet("proveedores-de-combustible")]
        public async Task<IActionResult> GetFuelProviders()
        {
            var data2 = await _context.FuelProviders
                .Include(x => x.Provider)
                .Where(x=>x.ProjectId == GetProjectId())
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = $"{x.Provider.Tradename}"
                 }).ToListAsync();

            return Ok(data2);
        }

        [HttpGet("proveedores-de-combustible-placas")]
        public async Task<IActionResult> GetFuelProvidersPlates(Guid? providerId = null)
        {
            var data2 = await _context.FuelProviderFoldings
                .Where(x=>x.FuelProviderId == providerId.Value)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = $"{x.CisternPlate}"
                 }).ToListAsync();

            return Ok(data2);
        }

        [HttpGet("proveedores-de-combustible-precios")]
        public async Task<IActionResult> GetFuelProvidersPrices(Guid? providerId = null)
        {
            var data2 = await _context.FuelProviderPriceFoldings
                .Where(x => x.FuelProviderId == providerId.Value)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = $"{x.Price}"
                 }).ToListAsync();

            return Ok(data2);
        }


        [HttpGet("proveedores-de-equipos-liviano")]
        public async Task<IActionResult> GetEquipmentProvidersSoft()
        {


            var data2 = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentProvider)
                .Include(x => x.EquipmentMachineryTypeSoft)
                .Where(x => x.EquipmentMachineryTypeSoftId != null)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.EquipmentProviderId,
                     Text = $"{x.EquipmentProvider.Provider.Tradename}"
                 }).Distinct().ToListAsync();

            return Ok(data2);
        }

        [HttpGet("proveedores-de-equipos-transporte")]
        public async Task<IActionResult> GetEquipmentProvidersTransport()
        {


            var data2 = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentProvider)
                .Include(x => x.EquipmentMachineryTypeTransport)
                .Where(x => x.EquipmentMachineryTypeTransportId.Value != null)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.EquipmentProviderId,
                     Text = $"{x.EquipmentProvider.Provider.Tradename}"
                 }).Distinct().ToListAsync();

            return Ok(data2);
        }



        [HttpGet("proveedores-de-equipos-maquinaria")]
        public async Task<IActionResult> GetEquipmentProvidersMach()
        {


            var data2 = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentProvider)
                .Include(x => x.EquipmentMachineryTypeType)
                .Where(x => x.EquipmentMachineryTypeTypeId.Value != null)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.EquipmentProviderId,
                     Text = $"{x.EquipmentProvider.Provider.Tradename}"
                 }).Distinct().ToListAsync();

            return Ok(data2);
        }


        [HttpGet("foldings-proveedor-seleccionado")]
        public async Task<IActionResult> GetFilteredEquipmentMachineryTypeSoft(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentMachineryTypeSoft)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value && x.EquipmentMachineryType.Description =="Equipos Menores")
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentMachineryTypeSoft.Description}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("foldings-empresa-seleccionada")]
        public async Task<IActionResult> GetFilteredBiddingWork(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.BiddingWorks
                .Include(x => x.Business)
                .Where(x => x.BusinessId == equipmentProviderId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Name}"
                }).ToListAsync();

            return Ok(query);
        }



        [HttpGet("foldings-proveedor-seleccionado-transporte")]
        public async Task<IActionResult> GetFilteredEquipmentMachineryTypeTransport(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentMachineryTypeTransport)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value && x.EquipmentMachineryType.Description == "Transporte")
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentMachineryTypeTransport.Description}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("foldings-proveedor-seleccionado-maquinaria")]
        public async Task<IActionResult> GetFilteredEquipmentMach(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentMachineryTypeType)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value && x.EquipmentMachineryType.Description == "Maquinaria")
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentMachineryTypeType.Description}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("foldings-proveedor-seleccionado-liviano")]
        public async Task<IActionResult> GetFilteredEquipmentMachinerySoft(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentMachineryTypeSoft)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value && x.EquipmentMachineryType.Description == "Equipos Menores")
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentMachineryTypeSoft.Description}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("proveedor-seleccionado")]
        public async Task<IActionResult> GetEquipmentMachineryTypeSoftSelected(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentMachineryTypeSoft)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value && x.EquipmentMachineryType.Description == "Equipos Menores")
                .Select(x => new SelectViewModel
                {
                    Id = x.EquipmentMachineryTypeSoft.Id,
                    Text = $"{x.EquipmentMachineryTypeSoft.Description}"
                }).ToListAsync();

            return Ok(query);
        }


        [HttpGet("proveedor-seleccionado-transporte")]
        public async Task<IActionResult> GetEquipmentMachineryTypeTransportSelected(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentMachineryTypeTransport)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value && x.EquipmentMachineryType.Description == "Transporte")
                .Select(x => new SelectViewModel
                {
                    Id = x.EquipmentMachineryTypeTransport.Id,
                    Text = $"{x.EquipmentMachineryTypeTransport.Description}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("proveedor-seleccionado-maquinaria")]
        public async Task<IActionResult> GetEquipmentMachineryTypeTypeSelected(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentMachineryTypeType)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value && x.EquipmentMachineryType.Description == "Maquinaria")
                .Select(x => new SelectViewModel
                {
                    Id = x.EquipmentMachineryTypeType.Id,
                    Text = $"{x.EquipmentMachineryTypeType.Description}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("equipos-liviano-filtro")]
        public async Task<IActionResult> GetEquipmentProvidersSoftFiltered()
        {


            var data2 = await _context.EquipmentMachineryTypeSofts
                .Where(x => x.Id != null)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = x.Description
                 }).Distinct().ToListAsync();

            return Ok(data2);
        }


        [HttpGet("equipos-transporte-filtro")]
        public async Task<IActionResult> GetEquipmentProvidersTransportFiltered()
        {


            var data2 = await _context.EquipmentMachineryTypeTransports
                .Where(x => x.Id != null)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = x.Description
                 }).Distinct().ToListAsync();

            return Ok(data2);
        }



        [HttpGet("equipos-maquinaria-filtro")]
        public async Task<IActionResult> GetEquipmentProvidersFiltered()
        {


            var data2 = await _context.EquipmentMachineryTypeTypes
                .Where(x => x.Id != null)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = x.Description
                 }).Distinct().ToListAsync();

            return Ok(data2);
        }

        [HttpGet("equipos-maquinaria-filtro-proovedor-folding")]
        public async Task<IActionResult> GetEquipmentProvidersFilteredByFolding()
        {


            var data2 = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentMachineryTypeType)
                .Where(x => x.Id != null)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = x.EquipmentMachineryTypeType.Description
                 }).Distinct().ToListAsync();

            return Ok(data2);
        }

        [HttpGet("equipos-menores-softs")]
        public async Task<IActionResult> GetEQSofts()
        {


            var data2 = await _context.EquipmentMachinerySofts
                .Include(x=>x.EquipmentProvider.Provider)
                .Where(x => x.Id != null)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = $"{x.EquipmentProvider.Provider.Tradename} - {x.Brand}  - {x.Model} - {x.EquipmentPlate}"
                 }).Distinct().ToListAsync();

            return Ok(data2);
        }

        [HttpGet("actividades-equipos-menores")]
        public async Task<IActionResult> GetSoftsActivities()
        {


            var data2 = await _context.EquipmentMachineryTypeSoftActivites
                .Include(x => x.EquipmentMachineryTypeSoft)
                .Where(x => x.Id != null)
                 .Select(x => new SelectViewModel
                 {
                     Id = x.Id,
                     Text = $"{x.EquipmentMachineryTypeSoft.Description} - {x.Description} "
                 }).Distinct().ToListAsync();

            return Ok(data2);
        }

        [HttpGet("actividad-menor-seleccionado")]
        public async Task<IActionResult> GetFilteredEquipmentMachineryTypeSoftActivitiebysoft(Guid? spId = null)
        {
            if (spId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachinerySoftPartPlusUltra
                .Include(x => x.EquipmentMachinerySoftPartFolding)
                .Include(x => x.EquipmentMachineryTypeSoftActivity)
                .Where(x => x.EquipmentMachinerySoftPartFolding.EquipmentMachinerySoftPartId == spId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.EquipmentMachineryTypeSoftActivityId,
                    Text = $"{x.EquipmentMachineryTypeSoftActivity.Description}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("actividad-menor-seleccionado-folding")]
        public async Task<IActionResult> GetFilteredEquipmentMachineryTypeSoftActivitiebysoftFolding(Guid? tsId = null)
        {
            if (tsId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachineryTypeSoftActivites
                .Where(x => x.EquipmentMachineryTypeSoftId == tsId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Description}"
                }).ToListAsync();

            return Ok(query);
        }


        [HttpGet("actividad-transporte-seleccionado-folding")]
        public async Task<IActionResult> GetFilteredEquipmentMachineryTypeTransportActivitiebysoftFolding(Guid? tsId = null)
        {
            if (tsId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachineryTypeTransportActivities
                .Where(x => x.EquipmentMachineryTypeTransportId == tsId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Description}"
                }).ToListAsync();

            return Ok(query);
        }


        [HttpGet("actividad-maquinaria-seleccionado-folding")]
        public async Task<IActionResult> GetFilteredEquipmentMachineryTypeTypeActivitiebysoftFolding(Guid? tsId = null)
        {
            if (tsId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachineryTypeTypeActivities
                .Where(x => x.EquipmentMachineryTypeTypeId == tsId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Description}"
                }).ToListAsync();

            return Ok(query);
        }


        //[HttpGet("operador-padre")]
        //public async Task<IActionResult> GetFilteredEquipmentMachineryOperatorByFather(Guid? oId = null)
        //{
        //    if (oId == null)
        //        return Ok(new List<SelectViewModel>());


        //    var query = await _context.EquipmentMachineryOperators
        //        .Include(x=>x.EquipmentMachineryType)
        //        .Where(x => x.ProviderId == oId.Value)
        //        .Where(x => x.EquipmentMachineryType.Description == "Transporte")
        //        .Select(x => new SelectViewModel
        //        {
        //            Id = x.Id,
        //            Text = x.FromOtherName + "" + x.OperatorName
        //        }).ToListAsync();

        //    return Ok(query);
        //}

        [HttpGet("proveedor-seleccionado-eqsoft")]
        public async Task<IActionResult> GetEquipmentMachinerySoftSelected(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachinerySofts
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentProvider.Provider.Tradename} - {x.Brand}  - {x.Model} - {x.EquipmentPlate}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("proveedor-seleccionado-eqtransport")]
        public async Task<IActionResult> GetEquipmentMachineryTransportSelected(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());

            var query = await _context.EquipmentMachineryTransports
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProviderFolding)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value)
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentProvider.Provider.Tradename} - {x.Brand}  - {x.Model} - {x.EquipmentPlate}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("proveedor-seleccionado-eqtransport2")]
        public async Task<IActionResult> GetEquipmentMachineryTransportSelected2(Guid? equipmentProviderId = null, Guid ? equipmentTypeId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());
            if (equipmentTypeId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachineryTransports
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProviderFolding)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value)
                .Where(x => x.EquipmentProviderFolding.EquipmentMachineryTypeTransportId == equipmentTypeId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentProvider.Provider.Tradename} - {x.Brand}  - {x.Model} - {x.EquipmentPlate}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("parte-transporte")]
        public async Task<IActionResult> GetEquipmentPartTransport()
        {
            var query = await _context.EquipmentMachineryTransportParts
                .Include(x => x.EquipmentMachineryTypeTransport)
                .Include(x=>x.EquipmentMachineryTransport)
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentMachineryTypeTransport.Description} - {x.EquipmentMachineryTransport.Brand}  - {x.EquipmentMachineryTransport.Model} - {x.EquipmentMachineryTransport.EquipmentPlate}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("parte-transporte-filtro")]
        public async Task<IActionResult> GetEquipmentPartTransportFilter()
        {
            var query = await _context.EquipmentMachineryTransportParts
                .Include(x => x.EquipmentMachineryTypeTransport)
                .Include(x => x.EquipmentMachineryTransport)
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentMachineryTransport.Brand}  - {x.EquipmentMachineryTransport.Model} - {x.EquipmentMachineryTransport.EquipmentPlate}"
                }).ToListAsync();

            return Ok(query);
        }




        [HttpGet("parte-maquinaria-filtro")]
        public async Task<IActionResult> GetEquipmentPartMachFilter()
        {
            var query = await _context.EquipmentMachParts
                .Include(x => x.EquipmentMachineryTypeType)
                .Include(x => x.EquipmentMach)
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentMach.Brand}  - {x.EquipmentMach.Model} - {x.EquipmentMach.Plate}"
                }).ToListAsync();

            return Ok(query);
        }


        [HttpGet("parte-transporte-seleccionado")]
        public async Task<IActionResult> GetEquipmentPartTransportProvider(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachineryTransportParts
                .Include(x => x.EquipmentMachineryTypeTransport)
                .Include(x => x.EquipmentMachineryTransport)
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentMachineryTypeTransport.Description} - {x.EquipmentMachineryTransport.Brand}  - {x.EquipmentMachineryTransport.Model} - {x.EquipmentMachineryTransport.EquipmentPlate} - Año {x.Year} - Mes {x.Month} "
                }).ToListAsync();

            return Ok(query);
        }


        [HttpGet("parte-maquinaria-seleccionado")]
        public async Task<IActionResult> GetEquipmentPartMachProvider(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachParts
                .Include(x => x.EquipmentMachineryTypeType)
                .Include(x => x.EquipmentMach)
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value && x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentMachineryTypeType.Description} - {x.EquipmentMach.Brand}  - {x.EquipmentMach.Model} - {x.EquipmentMach.Plate} -{x.Month} -{x.Year}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("proveedor-seleccionado-eqtype")]
        public async Task<IActionResult> GetEquipmentMachSelected(Guid? equipmentProviderId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachs
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value)
                .Where(x=>x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentProvider.Provider.Tradename} - -{x.Brand}  - {x.Model} - {x.Plate} -{x.SerieNumber}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("proveedor-seleccionado-eqtype2")]
        public async Task<IActionResult> GetEquipmentMachSelected2(Guid? equipmentProviderId = null,Guid? equipmentTypeId = null)
        {
            if (equipmentProviderId == null)
                return Ok(new List<SelectViewModel>());
            if (equipmentTypeId == null)
                return Ok(new List<SelectViewModel>());


            var query = await _context.EquipmentMachs
                .Include(x => x.EquipmentProviderFolding)
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.EquipmentProviderId == equipmentProviderId.Value)
                .Where(x => x.EquipmentProviderFolding.EquipmentMachineryTypeTypeId == equipmentTypeId.Value)
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.EquipmentProvider.Provider.Tradename} - -{x.Brand}  - {x.Model} - {x.Plate} -{x.SerieNumber}"
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("insumos-acero")]
        public async Task<IActionResult> GetSteelSupplies(Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            var query = _context.Supplies
                .Include(x => x.SupplyFamily)
                .Include(x => x.SupplyGroup)
                .Where(x => x.Id != null);

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.SupplyFamilyId == supplyFamilyId);

            if (supplyGroupId.HasValue)
                query = query.Where(x => x.SupplyGroupId == supplyGroupId);

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.FullCode + " - " + x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("modulos")]
        public async Task<IActionResult> GetModules(Guid? projectId = null)
        {
            var data = await _context.AreaModules
                
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("areas-trabajo")]
        public async Task<IActionResult> GetWorkAreas()
        {
            var data = await _context.WorkAreas
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("elementos-de-trabajo")]
        public async Task<IActionResult> GetWorkAreaItems(string search = null, Guid? workAreaId = null)
        {
            var query = _context.WorkAreaItems.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Contains(search));

            if (workAreaId.HasValue)
                query = query.Where(x => x.WorkAreaId == workAreaId.Value);

            var data = await query.Select(x => new SelectViewModel
            {
                Id = x.Id,
                Text = x.Name
            }).AsNoTracking().ToListAsync();

            return Ok(data);
        }

        [HttpGet("roles-de-trabajo")]
        public async Task<IActionResult> GetWorkRoles()
        {
            var data = await _context.WorkRoles
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("tipos-de-documento")]
        public async Task<IActionResult> GetDocumenTypes(Guid? projectId = null)
        {
            var data = await _context.DocumentTypes
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Type
                }).ToListAsync();

            return Ok(data);
        }
        //AppMovil
        //---------------------------------------
        //---------------------------------------
        //---------------------------------------

        [HttpGet("versiones")]
        public async Task<IActionResult> GetVersions(Guid? projectId = null)
        {
            var data = await _context.TechnicalVersions
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("versiones-planos")]
        public async Task<IActionResult> GetVersionsBluePrints(Guid? projectId = null)
        {
            var data = await _context.TechnicalVersions
                .Where(x => x.ProjectId == projectId.Value && (x.Description == "Contractual" || x.Description == "Aprobada"))
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("formulas-proyecto-filtro")]
        public async Task<IActionResult> GetProjectFormulasFilter(Guid? projectId = null)
        {
            var data = await _context.ProjectFormulas
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Code}-{x.Name}"
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("frentes-formula-todos")]
        public async Task<IActionResult> GetAllWorkFronts(Guid projectFormulaId, Guid? projectId = null)
        {
            var query = await _context.WorkFrontProjectPhases
                    .Include(x => x.ProjectPhase)
                    .Include(x => x.WorkFront)
                    .Where(x => x.WorkFront.ProjectId == projectId.Value)
                .ToListAsync();


            query = await _context.WorkFrontProjectPhases
            .Where(x => x.ProjectPhase.ProjectFormulaId == projectFormulaId).ToListAsync();


            var workFront = await _context.WorkFronts.ToListAsync();

            int count = 0;

            var workFrontsAux = new List<WorkFront>();

            foreach (var front in workFront)
            {
                foreach (var item in query)
                {
                    if (item.WorkFrontId == front.Id && count == 0)
                    {
                        workFrontsAux.Add(front);
                        count++;
                    }
                }
                count = 0;
            }

            var data = workFrontsAux
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code
                }).ToList();

            return Ok(data);
        }

        [HttpGet("titulos-de-presupuesto-area-tecnica")]
        public async Task<IActionResult> GetBudgetTitlesTechnicalArea(Guid? projectId = null)
        {
            var data = await _context.BudgetTitles
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("especialidades-formula")]
        public async Task<IActionResult> GetSpecialities(Guid? projectFormulaId = null, Guid? projectId = null)
        {
            if(!projectFormulaId.HasValue)
            return Ok(new List<SelectViewModel>());

            if (!projectId.HasValue)
                return Ok(new List<SelectViewModel>());


            var data = await _context.SpecFormulas
                .Include(x=>x.Speciality)
                .Where(x => x.Speciality.ProjectId == projectId.Value)
                .Where(x => x.ProjectFormulaId == projectFormulaId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Speciality.Id,
                    Text = x.Speciality.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("especialidades")]
        public async Task<IActionResult> GetSpecialities(Guid? projectId = null)
        {
            var data = await _context.Specialities
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("especialidades-todas")]
        public async Task<IActionResult> GetAllSpecialities()
        {
            var data = await _context.Specialities
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).Distinct().ToListAsync();

            return Ok(data);
        }

        [HttpGet("diseños")]
        public async Task<IActionResult> GetDesigns(Guid? projectId = null)
        {
            var data = await _context.DesignTypes
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("tipos-de-cemento")]
        public async Task<IActionResult> GetCementTypes(Guid? projectId = null)
        {
            var data = await _context.CementTypes
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("procedimiento/archivos/{id}")]
        public async Task<IActionResult> GetProcedureFiles(Guid id)
        {
            var files = await _context.ProcedureFiles
                .Where(x => x.ProcedureId == id)
                .Select(x => new SelectViewModel<Uri>
                {
                    Id = x.FileUrl,
                    Text = System.IO.Path.GetFileName(x.FileUrl.LocalPath)
                }).ToListAsync();

            return Ok(files);
        }

        [HttpGet("tipos-de-planos")]
        public async Task<IActionResult> GetBPTypes(Guid? projectId = null)
        {
            var data = await _context.BlueprintTypes
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("tipos-de-agregado")]
        public async Task<IActionResult> GetAggregateTypes(Guid? projectId = null)
        {
            var data = await _context.AggregateTypes
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("uso-de-concreto")]
        public async Task<IActionResult> GetConcreteUses(Guid? projectId = null)
        {
            var data = await _context.ConcreteUses
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Description
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("procesos")]
        public async Task<IActionResult> GetProcesses(Guid? projectId = null)
        {
            var data = await _context.Processes

                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.ProcessName
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("cartas-recibidas")]
        public async Task<IActionResult> GetLettersReceived(Guid? projectId = null)
        {

            var query = _context.Letters.AsQueryable();

            query = query.Where(x => x.ProjectId == projectId && x.Type == 2);
            
            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("formulas-por-proyecto")]
        public async Task<IActionResult> GetProjectFormulasByProject(Guid? projectId = null)
        {
            var data = await _context.ProjectFormulas
                .Where(x => x.ProjectId == projectId.Value)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = $"{x.Code}-{x.Name}"
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("tipos-de-almacen")]
        public async Task<IActionResult> GetWarehouseTypes()
        {
            var data = await _context.WarehouseTypes
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("almacenes")]
        public async Task<IActionResult> GetWarehouses(Guid? warehouseTypeId = null)
        {
            var query = _context.Warehouses
                .Include(x => x.WarehouseType)
                .Where(x => x.WarehouseType.ProjectId == GetProjectId());

            if (warehouseTypeId.HasValue)
                query = query.Where(x => x.WarehouseTypeId == warehouseTypeId);

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Address
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("conceptos-fijos")]
        public async Task<IActionResult> GetConcepts(int laborRegime)
        {
            var data = await _context.PayrollConceptFormulas
                .Include(x => x.PayrollConcept)
                .Where(x => x.LaborRegimeId == laborRegime && x.Formula == "0" &&
                                x.PayrollConcept.CategoryId == 1)
                .Select(x => new SelectViewModel
                {
                    Id = x.PayrollConceptId,
                    Text = x.PayrollConcept.Description
                }).ToListAsync();

            return Ok(data);
        }


        [HttpGet("tipo-generacion-pre-reque")]
        public async Task<IActionResult> GetPreRequeTypes()
        {
            var query = new List<SelectIntViewModel>();

            foreach (var item in ConstantHelpers.Logistics.RequestOrder.Type.VALUES)
            {
                query.Add(new SelectIntViewModel
                {
                    Id = item.Key,
                    Text = item.Value
                });
            }

            return Ok(query);
        }

        [HttpGet("frentes-formula-app")]
        public async Task<IActionResult> GetWorkFrontsByProjectFormulaAppGuid(Guid? projectFormulaId = null, Guid? projectId = null)
        {


            var query = await _context.WorkFrontProjectPhases
                    .Include(x => x.ProjectPhase)
                    .Include(x => x.WorkFront)
                .Where(x => x.WorkFront.ProjectId == projectId.Value)
                .ToListAsync();

            if (projectFormulaId.HasValue)
            {
                query = await _context.WorkFrontProjectPhases.Where(x => x.ProjectPhase.ProjectFormulaId == projectFormulaId).ToListAsync();
            }

            var workFronts = query.GroupBy(x => x.WorkFrontId);

            var workFrontsAux = new List<WorkFront>();

            foreach (var front in workFronts)
            {
                workFrontsAux.Add(front.FirstOrDefault().WorkFront);
            }

            var data = workFrontsAux
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code
                }).ToList();

            return Ok(data);
        }

        [HttpPost("frentes-formulas-app")]
        public async Task<IActionResult> GetWorkFrontsByProjectFormulasApp(List<Guid> projectFormulas, Guid projectId)
        {
            var pId = projectId;

            var query = await _context.WorkFrontProjectPhases
                .Include(x => x.ProjectPhase)
                .Include(x => x.WorkFront)
                .Where(x => x.WorkFront.ProjectId == pId)
                .Where(x => projectFormulas.Contains((Guid)x.ProjectPhase.ProjectFormulaId))
                .ToListAsync();

            var workFronts = query.GroupBy(x => x.WorkFrontId);

            var workFrontsAux = new List<WorkFront>();

            foreach (var front in workFronts)
            {
                workFrontsAux.Add(front.FirstOrDefault().WorkFront);
            }

            var data = workFrontsAux
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Code
                }).ToList();

            return Ok(data);
        }

        [HttpGet("categorias-de-capacitaciones")]
        public async Task<IActionResult> GetTrainingCategories()
        {
            var data = await _context.TrainingCategories
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpGet("archivos-de-capacitaciones")]
        public async Task<IActionResult> GetTrainingTopicFiles(Guid id)
        {
            var data = await _context.TrainingTopicFiles
                .Where(x => x.TrainingTopicId == id)
                .AsNoTracking().ToListAsync();
            var result = data.Select(x => new SelectViewModel<string>{
                    Id = x.Url.ToString(),
                    Text = x.Url.ToString().Split('/').LastOrDefault()
                }).ToList();
            return Ok(result);
        }

        [HttpGet("temas-de-capacitaciones")]
        public async Task<IActionResult> GetTrainingTopics(Guid? trainingCategoryId = null)
        {
            var projectId = GetProjectId();
            var query = _context.TrainingTopics
                .Where(x => x.ProjectId == projectId)
                .AsQueryable();

            if (trainingCategoryId.HasValue)
                query = query.Where(x => x.TrainingCategoryId == trainingCategoryId.Value);
            
            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Name
                }).AsNoTracking().ToListAsync();
            
            return Ok(data);
        }

        // ------------------------------------------------------
        // App Select Method's
        // ------------------------------------------------------
        [HttpGet("app/cuadrillas-jefe-frente")]
        public async Task<IActionResult> GetAppSewerGroupsByWorkFrontHead(Guid? workFrontHeadId = null)
        {
            var projectId = GetProjectId();

            var query = _context.SewerGroupPeriods
                .Include(x => x.SewerGroup)
                .Where(x => x.SewerGroup.ProjectId == projectId &&
                            (x.WorkFrontHeadId == workFrontHeadId))
                            .AsNoTracking()
                            .AsQueryable();

            var data = await query
                .Select(x => new SelectViewModel
                {
                    Id = x.SewerGroupId,
                    Text = x.SewerGroup.Code
                }).ToListAsync();

            data = data.Distinct().ToList();

            return Ok(data);
        }
    }
}
