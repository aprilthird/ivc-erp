using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IVC.PE.DATA.Seed
{
    public class DbInitializer
    {
        public static void Seed(IvcDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (roleManager.FindByNameAsync(ConstantHelpers.Roles.SUPERADMIN).Result == null)
            {
                roleManager.CreateAsync(new ApplicationRole(ConstantHelpers.Roles.SUPERADMIN)).Wait();
            }

            if (roleManager.FindByNameAsync(ConstantHelpers.Roles.TECHNICAL_OFFICE).Result == null)
            {
                roleManager.CreateAsync(new ApplicationRole(ConstantHelpers.Roles.TECHNICAL_OFFICE)).Wait();
            }

            if (userManager.FindByEmailAsync("superadmin@jicamarca.pe").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "superadmin@ivc.pe",
                    Email = "superadmin@ivc.pe",
                    Name = "Superadmin",
                    PaternalSurname = "Superadmin",
                    MaternalSurname = "Superadmin",
                    PhoneNumber = "999999999",
                    BelongsToMainOffice = true
                };

                var result = userManager.CreateAsync(user, "Admin.123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, ConstantHelpers.Roles.SUPERADMIN).Wait();
                }
            }

            if (userManager.FindByEmailAsync("oficinatecnica@jicamarca.pe").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "oficinatecnica@jicamarca.pe",
                    Email = "oficinatecnica@jicamarca.pe",
                    Name = "Oficina Tecnica",
                    PaternalSurname = "Oficina",
                    MaternalSurname = "Tecnica",
                    PhoneNumber = "999888999"
                };

                var result = userManager.CreateAsync(user, "OficinaTecnica.123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, ConstantHelpers.Roles.TECHNICAL_OFFICE).Wait();
                }
            }

            if (!context.Projects.Any())
            {
                context.Projects.AddRange(new List<Project>()
                {
                    new Project { Name = "ESQUEMA ANEXO 22 PAMPA JICAMARCA DE CANTO GRANDE - SECTORIZACION Y AMPLIACION  DE LOS SISTEMAS  DE AGUA Y ALCANTARILLADO - DISTRITO DE SAN ANTONIO DE HUAROCHIRI" }
                });

                context.SaveChanges();
            }

            if(!context.ProjectBudgetCategories.Any())
            {
                var project = context.Projects.FirstOrDefault();
                context.ProjectBudgetCategories.AddRange(new List<ProjectBudgetCategory>()
                {
                    new ProjectBudgetCategory { Code = 1, ProjectId = project.Id, Name = "Obras Civiles", Description = "" },
                    new ProjectBudgetCategory { Code = 2, ProjectId = project.Id, Name = "Equipamiento Hidráulico", Description = "" },
                    new ProjectBudgetCategory { Code = 3, ProjectId = project.Id, Name = "Líneas de Agua Potable", Description = "" },
                    new ProjectBudgetCategory { Code = 4, ProjectId = project.Id, Name = "Líneas de Alcantarillado", Description = "" },
                    new ProjectBudgetCategory { Code = 5, ProjectId = project.Id, Name = "Redes y Conexiones de Agua Potable", Description = "" },
                    new ProjectBudgetCategory { Code = 6, ProjectId = project.Id, Name = "Redes y Conexiones de Alcantarillado", Description = "" },
                    new ProjectBudgetCategory { Code = 7, ProjectId = project.Id, Name = "Colector de Descarga Anexo 22", Description = "" }
                });
                context.SaveChanges();
            }

            if (!ConstantHelpers.Seed.SEWER_LINES)
                return;

            if(!context.SystemPhases.Any())
            {
                var project = context.Projects.FirstOrDefault();
                context.SystemPhases.AddRange(new List<SystemPhase>()
                {
                    new SystemPhase { ProjectId = project.Id, Code = "FS-1" }
                });
                context.SaveChanges();
            }

            if(!context.WorkFronts.Any())
            {
                var user = context.Users.FirstOrDefault(x => x.Email == "jefedefrente@jicamarca.pe");
                var systemPhases = context.SystemPhases.ToList();
                context.WorkFronts.AddRange(new List<WorkFront>()
                {
                    new WorkFront { Code = "FD-1", SystemPhaseId = systemPhases.FirstOrDefault().Id }
                });
                context.SaveChanges();
            }
            
            if (!context.Foremen.Any())
            {
                context.Foremen.AddRange(new List<Foreman>()
                {
                    new Foreman { Name = "Capataz", PaternalSurname = "Capataz", MaternalSurname = "Capataz", DocumentType = ConstantHelpers.DocumentType.ID_CARD, Document = "66677788", Role = "Test" }
                });
                context.SaveChanges();
            }

            if (!context.QualificationZones.Any())
            {
                var project = context.Projects.FirstOrDefault();
                context.QualificationZones.AddRange(new List<QualificationZone>()
                {
                    new QualificationZone { Name = "GRUPO VALLE SAGRADO", Code = "GVS", ProjectId = project.Id }
                });
            }

            if (!context.SewerGroups.Any())
            {
                var project = context.Projects.FirstOrDefault();
                var workFront = context.WorkFronts.FirstOrDefault();
                var foreman = context.Foremen.FirstOrDefault();
                context.SewerGroups.AddRange(new List<SewerGroup>()
                {
                    new SewerGroup { Code = "CD-1", Name = "Cuadrilla 1", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-2", Name = "Cuadrilla 2", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-3", Name = "Cuadrilla 3", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-4", Name = "Cuadrilla 4", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-5", Name = "Cuadrilla 5", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-6", Name = "Cuadrilla 6", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-7", Name = "Cuadrilla 7", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-8", Name = "Cuadrilla 8", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-9", Name = "Cuadrilla 9", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-10", Name = "Cuadrilla 10", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-11", Name = "Cuadrilla 11", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-12", Name = "Cuadrilla 12", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-13", Name = "Cuadrilla 13", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-14", Name = "Cuadrilla 14", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-15", Name = "Cuadrilla 15", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-16", Name = "Cuadrilla 16", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-17", Name = "Cuadrilla 17", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-18", Name = "Cuadrilla 18", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-19", Name = "Cuadrilla 19", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-20", Name = "Cuadrilla 20", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "CD-21", Name = "Cuadrilla 21", Type = ConstantHelpers.Sewer.Group.Type.DRAINAGE, WorkFrontId = workFront.Id },

                    new SewerGroup { Code = "COLD JT", Name = "Colector Julio C. Tello", Type = ConstantHelpers.Sewer.Group.Type.MANIFOLD, WorkFrontId = workFront.Id },
                    new SewerGroup { Code = "OCD RRP-01", Name = "Obra Civil 1", Type = ConstantHelpers.Sewer.Group.Type.DRINKING_WATER, WorkFrontId = workFront.Id },
                });
                context.SaveChanges();
            }

            //if(!context.SewerBoxes.Any())
            //{
            //    var groups = context.SewerGroups.ToList();
            //    var lst = new List<SewerBox>()
            //    {
            //        new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "CD-1").Id, Code = "1", Stage = ConstantHelpers.Stage.CONTRACTUAL, Address = "Ca. Los Colibries", Cover = 947.7, Bottom = 946 },
            //        new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "CD-1").Id, Code = "1", Stage = ConstantHelpers.Stage.STAKING, Address = "Ca. Los Colibries", Cover = 948.54, Bottom = 946.84 },
            //        new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "CD-1").Id, Code = "1", Stage = ConstantHelpers.Stage.REAL, Address = "Ca. Los Colibries", Cover = 948.54, Bottom = 946.84 },
            //        new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "CD-1").Id, Code = "2", Stage = ConstantHelpers.Stage.CONTRACTUAL, Address = "Ca. Los Colibries", Cover = 940.53, Bottom = 938.83 },
            //        new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "CD-1").Id, Code = "2", Stage = ConstantHelpers.Stage.STAKING, Address = "Ca. Los Colibries", Cover = 940.96, Bottom = 939.26 },
            //        new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "CD-1").Id, Code = "2", Stage = ConstantHelpers.Stage.REAL, Address = "Ca. Los Colibries", Cover = 940.96, Bottom = 939.26 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "C-1").Id, Code = "3", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Ca. Los Colibries", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "C-1").Id, Code = "3", Stage = ConstantHelpers.Sewer.Line.Stage.STAKING, Address = "Ca. Los Colibries", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "C-1").Id, Code = "3", Stage = ConstantHelpers.Sewer.Line.Stage.REAL, Address = "Ca. Los Colibries", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "C-1").Id, Code = "4", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Ca. Los Colibries", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "C-1").Id, Code = "4", Stage = ConstantHelpers.Sewer.Line.Stage.STAKING, Address = "Ca. Los Colibries", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "C-1").Id, Code = "4", Stage = ConstantHelpers.Sewer.Line.Stage.REAL, Address = "Ca. Los Colibries", Cover = 0, InputOutput = 0, Bottom = 0 },

            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "COL JT").Id, Code = "19", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Av. Julio C. Tello", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "COL JT").Id, Code = "19", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Av. Julio C. Tello", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "COL JT").Id, Code = "19", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Av. Julio C. Tello", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "COL JT").Id, Code = "20", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Av. Julio C. Tello", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "COL JT").Id, Code = "20", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Av. Julio C. Tello", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "COL JT").Id, Code = "20", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Av. Julio C. Tello", Cover = 0, InputOutput = 0, Bottom = 0 },

            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "OC RRP-01").Id, Code = "BR-1", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Psj. Mita", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "OC RRP-01").Id, Code = "BR-1", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Psj. Mita", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "OC RRP-01").Id, Code = "BR-1", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Psj. Mita", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "OC RRP-01").Id, Code = "BR-2", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Psj. Mita", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "OC RRP-01").Id, Code = "BR-2", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Psj. Mita", Cover = 0, InputOutput = 0, Bottom = 0 },
            //        //new SewerBox { SewerGroupId = groups.FirstOrDefault(x => x.Code == "OC RRP-01").Id, Code = "BR-2", Stage = ConstantHelpers.Sewer.Line.Stage.CONTRACTUAL, Address = "Psj. Mita", Cover = 0, InputOutput = 0, Bottom = 0 }
            //    };
            //    lst.ForEach(x => x.Calculate());
            //    context.SewerBoxes.AddRange(lst);
            //    context.SaveChanges();
            //}

            //if(!context.SewerLines.Any())
            //{
            //    var groups = context.SewerGroups.ToList();
            //    var qZones = context.QualificationZones.ToList();
            //    var sewerBoxesC = context.SewerBoxes.Where(x => x.Stage == ConstantHelpers.Stage.CONTRACTUAL).ToList();
            //    var sewerBoxesS = context.SewerBoxes.Where(x => x.Stage == ConstantHelpers.Stage.STAKING).ToList();
            //    var sewerBoxesR = context.SewerBoxes.Where(x => x.Stage == ConstantHelpers.Stage.REAL).ToList();
            //    var lst = new List<SewerLine>()
            //    {
            //        new SewerLine { SewerGroupId = groups.FirstOrDefault(x => x.Code == "CD-1").Id, QualificationZoneId = qZones.FirstOrDefault(x => x.Code == "GVS").Id, InitialSewerBoxId = sewerBoxesC.FirstOrDefault(x => x.Code == "1").Id, InitialSewerBox = sewerBoxesC.FirstOrDefault(x => x.Code == "1"), FinalSewerBoxId = sewerBoxesC.FirstOrDefault(x => x.Code == "2").Id, FinalSewerBox = sewerBoxesC.FirstOrDefault(x => x.Code == "2"), Address = "Ca. Los Colibries", Stage = ConstantHelpers.Stage.CONTRACTUAL, HorizontalDistanceOnAxis = 20f, NominalDiameter = 200, PipelineType = 1, PipelineClass = 1, TerrainType = ConstantHelpers.Terrain.Type.ROCKY },
            //        new SewerLine { SewerGroupId = groups.FirstOrDefault(x => x.Code == "CD-1").Id, QualificationZoneId = qZones.FirstOrDefault(x => x.Code == "GVS").Id, InitialSewerBoxId = sewerBoxesS.FirstOrDefault(x => x.Code == "1").Id, InitialSewerBox = sewerBoxesC.FirstOrDefault(x => x.Code == "1"), FinalSewerBoxId = sewerBoxesS.FirstOrDefault(x => x.Code == "2").Id, FinalSewerBox = sewerBoxesC.FirstOrDefault(x => x.Code == "2"), Address = "Ca. Los Colibries", Stage = ConstantHelpers.Stage.STAKING, HorizontalDistanceOnAxis = 20f, NominalDiameter = 200, PipelineType = 1, PipelineClass = 1, TerrainType = ConstantHelpers.Terrain.Type.ROCKY },
            //        new SewerLine { SewerGroupId = groups.FirstOrDefault(x => x.Code == "CD-1").Id, QualificationZoneId = qZones.FirstOrDefault(x => x.Code == "GVS").Id, InitialSewerBoxId = sewerBoxesR.FirstOrDefault(x => x.Code == "1").Id, InitialSewerBox = sewerBoxesC.FirstOrDefault(x => x.Code == "1"), FinalSewerBoxId = sewerBoxesR.FirstOrDefault(x => x.Code == "2").Id, FinalSewerBox = sewerBoxesC.FirstOrDefault(x => x.Code == "2"), Address = "Ca. Los Colibries", Stage = ConstantHelpers.Stage.REAL, HorizontalDistanceOnAxis = 20.30f, NominalDiameter = 200, PipelineType = 1, PipelineClass = 1, TerrainType = ConstantHelpers.Terrain.Type.ROCKY },
            //    };
            //    lst.ForEach(x => x.Calculate(false));
            //    context.SewerLines.AddRange(lst);
            //    context.SaveChanges();
            //}
        }
    }
}
