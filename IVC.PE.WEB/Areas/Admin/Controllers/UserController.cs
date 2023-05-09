using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkAreaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkPositionViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkRoleViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/usuarios")]
    public class UserController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public UserController(IvcDbContext context,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<UserController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, userManager, roleManager, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(string roleId = null, Guid? workAreaId = null, Guid? workPositionId = null, Guid? projectId = null)
        {
            //var roleFilter = roleId.HasValue ? ConstantHelpers.Roles.VALUES[roleId.Value] : string.Empty;
            var roles = await _context.UserRoles
                .Include(x => x.Role)
                .ToListAsync();

            if (roleId == "Todos") roleId = null;

            var projects = await _context.UserProjects
                .Include(x => x.Project)
                .ToListAsync();
            /*
            var users2 = await _context.UserProjects
                .Include(x => x.User)
                .Include(x => x.User.WorkAreaEntity)
                .Include(x => x.User.WorkPosition)
                .OrderBy(x => x.User.PaternalSurname)
                .Where(x => (!string.IsNullOrEmpty(roleId) ? x.User.UserRoles.Count(y => y.Role.Id.Equals(roleId)) > 0 : true) &&
                    (workAreaId.HasValue ? x.User.WorkAreaId == workAreaId.Value : true) &&
                    (workPosition.HasValue ? x.User.WorkPositionId == workPosition.Value : true))
                .Select(x => new UserViewModel
                {
                    Id = x.User.Id,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    MiddleName = x.User.MiddleName,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    WorkArea = x.User.WorkArea,
                    WorkAreaEntity = new WorkAreaViewModel
                    {
                        IntValue = x.User.WorkAreaEntity.IntValue,
                    },
                    WorkPosition = new WorkPositionViewModel
                    {
                        Name = x.User.WorkPosition.Name
                    },
                    RoleNames = x.User.UserRoles.Select(ur => ur.Role.Name).ToList(),
                    BelongsToMainOffice = x.User.BelongsToMainOffice
                }).AsNoTracking()
                .ToListAsync();
            */

            var query = _context.Users
                .Include(x => x.WorkAreaEntity)
                .Include(x => x.WorkPosition)
                .Where(x => x.Id != null);

            if (workAreaId.HasValue)
                query = query.Where(x => x.WorkAreaId == workAreaId);

            if (workPositionId.HasValue)
                query = query.Where(x => x.WorkPositionId == workPositionId);

            if (projectId.HasValue)
                query = query.Where(x => x.UserProjects.Count(y => y.ProjectId == projectId) > 0);

            var users = await query
                .OrderBy(x => x.PaternalSurname)
                .Where(x => (!string.IsNullOrEmpty(roleId) ? x.UserRoles.Count(y => y.Role.Id.Equals(roleId)) > 0 : true))
                .Select(x => new UserViewModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       PaternalSurname = x.PaternalSurname,
                       MaternalSurname = x.MaternalSurname,
                       MiddleName = x.MiddleName,
                       Email = x.Email,
                       PhoneNumber = x.PhoneNumber,
                       WorkArea = x.WorkArea,
                       WorkAreaEntity = new WorkAreaViewModel
                       {
                           IntValue = x.WorkAreaEntity.IntValue,
                       },
                       WorkPosition = new WorkPositionViewModel
                       {
                           Name = x.WorkPosition.Name
                       },
                       WorkRole = x.WorkRoleId.HasValue ? new WorkRoleViewModel
                       {
                           Name = x.WorkRole.Name
                       } : null,
                       RoleNames = x.UserRoles.Select(ur => ur.Role.Name).ToList(),
                       BelongsToMainOffice = x.BelongsToMainOffice,
                       SignatureUrl = x.SignatureUrl
                   }).AsNoTracking()
                .ToListAsync();

            foreach (var item in users)
            {
                item.StringRoleNames = string.Join(" ,", roles.Where(x => x.UserId == item.Id).Select(x => x.Role.Name));
                item.StringProjectNames = string.Join(" , ", projects.Where(x => x.UserId == item.Id).Select(x => x.Project.Abbreviation));
            }

            return Ok(users);

            //var data = await _context.Users
            //    //.Where(x => !x.UserRoles.Any(ur => ur.Role.Name == ConstantHelpers.Roles.SUPERADMIN))
            //    .OrderBy(x => x.PaternalSurname)
            //    .Select(x => new UserViewModel
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        PaternalSurname = x.PaternalSurname,
            //        MaternalSurname = x.MaternalSurname,
            //        MiddleName = x.MiddleName,
            //        Email = x.Email,
            //        PhoneNumber = x.PhoneNumber,
            //        WorkArea = x.WorkArea,
            //        WorkPosition = new WorkPositionViewModel
            //        {
            //            Name = x.WorkPosition.Name
            //        },
            //        BelongsToMainOffice = x.BelongsToMainOffice
            //    }).AsNoTracking()
            //    .ToListAsync();

            //return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var project = await _context.Users
                .Include(x => x.WorkAreaEntity)
                .Include(x => x.WorkPosition)
                .Where(x => x.Id == id)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    MiddleName = x.MiddleName,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    Email = x.Email,
                    WorkArea = x.WorkArea,
                    WorkAreaId = x.WorkAreaId,
                    WorkRoleId = x.WorkRoleId,
                    WorkPositionId = x.WorkPositionId,
                    PhoneNumber = x.PhoneNumber,
                    BelongsToMainOffice = x.BelongsToMainOffice,
                    RoleIds = x.UserRoles.Select(ur => ur.RoleId).ToList(),
                    ProjectIds = x.UserProjects.Select(up => up.ProjectId).ToList(),
                    SignatureUrl = x.SignatureUrl
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(project);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (string.IsNullOrEmpty(model.Password))
                return BadRequest("El usuario necesita una contraseña al ser creado.");
            var user = new ApplicationUser
            {
                Name = model.Name,
                PaternalSurname = model.PaternalSurname,
                MaternalSurname = model.MaternalSurname,
                MiddleName = model.MiddleName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                WorkArea = model.WorkArea,
                WorkAreaId = model.WorkAreaId,
                WorkRoleId = model.WorkRoleId,
                WorkPositionId = model.WorkPositionId
            };
            await _context.UserProjects.AddRangeAsync(
                model.ProjectIds.Select(x => new ApplicationUserProject
                {
                    ProjectId = x,
                    User = user
                }).ToList());
            await _userManager.CreateAsync(user, model.Password);
            if (model.RoleIds != null)
            {
                var roles = await _roleManager.Roles.Where(x => model.RoleIds.Contains(x.Id)).ToListAsync();
                await _userManager.AddToRolesAsync(user, roles.Select(x => x.Name));
            }
            if (model.SignatureFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                user.SignatureUrl = await storage.UploadFile(model.SignatureFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.USERS,
                    System.IO.Path.GetExtension(model.SignatureFile.FileName),
                    ConstantHelpers.Storage.Blobs.SIGNATURE,
                    $"firma-{user.RawFullName}");
            }
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(string id, UserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.Users
                .Include(x => x.UserProjects)
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
            user.Name = model.Name;
            user.MiddleName = model.MiddleName;
            user.PaternalSurname = model.PaternalSurname;
            user.MaternalSurname = model.MaternalSurname;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Email;
            user.WorkArea = model.WorkArea;
            user.WorkAreaId = model.WorkAreaId;
            user.WorkRoleId = model.WorkRoleId;
            user.WorkPositionId = model.WorkPositionId;
            if(!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            if (user.UserRoles != null)
                await _userManager.RemoveFromRolesAsync(user, user.UserRoles.Select(ur => ur.Role.Name));
            if (model.RoleIds != null)
            {
                var roles = await _roleManager.Roles.Where(x => model.RoleIds.Contains(x.Id)).AsNoTracking().ToListAsync();
                await _userManager.AddToRolesAsync(user, roles.Select(x => x.Name));
            }
            if (user.UserProjects != null)
                _context.UserProjects.RemoveRange(user.UserProjects);
            await _context.UserProjects.AddRangeAsync(
                model.ProjectIds.Select(x => new ApplicationUserProject
                {
                    ProjectId = x,
                    UserId = user.Id
                }).ToList());
            if (model.SignatureFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (user.SignatureUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SIGNATURE}/{user.SignatureUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.USERS);
                user.SignatureUrl = await storage.UploadFile(model.SignatureFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.USERS,
                    System.IO.Path.GetExtension(model.SignatureFile.FileName),
                    ConstantHelpers.Storage.Blobs.SIGNATURE,
                    $"firma-{user.RawFullName}");
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("actualizar-estado-oficina/{id}")]
        public async Task<IActionResult> UpdateMainOfficeStatus(string id, bool belongsToMainOffice)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.BelongsToMainOffice = belongsToMainOffice;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest($"Usuario con Id '{id}' no encontrado.");
            var user = await _context.Users
                .Include(x => x.UserInterestGroups)
                .Include(x => x.UserProjects)
                .Include(x => x.UserRoles)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (user.UserProjects != null)
                _context.UserProjects.RemoveRange(user.UserProjects);
            if (user.UserInterestGroups != null)
                _context.UserInterestGroups.RemoveRange(user.UserInterestGroups);
            if (user.UserRoles != null)
                _context.UserRoles.RemoveRange(user.UserRoles);
            _context.Users.Remove(user);
            if (user.SignatureUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SIGNATURE}/{user.SignatureUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.USERS);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("importar")]  
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.First();
                    var counter = 3;
                    var userState = new List<bool>();
                    var users = new List<ApplicationUser>();
                    var workPositions = new List<WorkPosition>();
                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var emailString = workSheet.Cell($"F{counter}").GetString();
                        if (string.IsNullOrEmpty(emailString))
                        {
                            ++counter;
                            continue;
                        }
                        var user = await _userManager.FindByNameAsync(emailString);
                        userState.Add(user == null);
                        if (user == null)
                            user = new ApplicationUser();
                        user.Email = emailString;
                        user.NewAccount = true;
                        user.BelongsToMainOffice = emailString.Contains("ivc");
                        user.PaternalSurname = workSheet.Cell($"B{counter}").GetString();
                        user.MaternalSurname = workSheet.Cell($"C{counter}").GetString();
                        user.Name = workSheet.Cell($"D{counter}").GetString();
                        user.MiddleName = workSheet.Cell($"E{counter}").GetString();
                        user.WorkArea = ConstantHelpers.Employee.WorkArea.VALUES
                            .Where(x => x.Value.ToUpper() == workSheet.Cell($"G{counter}").GetString().ToUpper()).Select(x => x.Key).FirstOrDefault();
                        user.UserName = user.Email;
                        var workPositionStr = workSheet.Cell($"H{counter}").GetString();
                        if(!string.IsNullOrEmpty(workPositionStr))
                        {
                            var workPosition = await _context.WorkPositions.FirstOrDefaultAsync(x => x.Name == workPositionStr)
                                ?? workPositions.FirstOrDefault(x => x.Name == workPositionStr);
                            if (workPosition == null)
                            {
                                workPosition = new WorkPosition
                                {
                                    Name = workPositionStr
                                };
                                workPositions.Add(workPosition);
                            }
                            user.WorkPosition = workPosition;
                        }
                        users.Add(user);
                        ++counter;
                    }
                    await _context.WorkPositions.AddRangeAsync(workPositions);
                    await _context.SaveChangesAsync();
                    for (var i = 0; i < users.Count(); ++i)
                    {
                        if (userState[i])
                        {
                            users[i].NormalizedEmail = users[i].Email.ToUpperInvariant();
                            users[i].NormalizedUserName = users[i].UserName.ToUpperInvariant();
                            users[i].PasswordHash = _userManager.PasswordHasher.HashPassword(users[i], "Ivc.2020");
                            await _context.Users.AddAsync(users[i]);
                            await _context.SaveChangesAsync();
                        }
                        else
                            await _userManager.UpdateAsync(users[i]);
                    }
                }
                mem.Close();
            }
            return Ok();
        }
    }
}