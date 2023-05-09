using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Factories
{
    public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        private IvcDbContext _context;

        public ClaimsPrincipalFactory(
            IvcDbContext context,
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager, 
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
            _context = context;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var roles = await UserManager.GetRolesAsync(user);
            var projects = roles.Contains(ConstantHelpers.Roles.SUPERADMIN)
                ? await _context.Projects.ToListAsync()
                : await _context.UserProjects.Where(x => x.UserId == user.Id)
                    .Select(x => x.Project).ToListAsync();

            //Temporal
            WorkArea workArea = null;
            if (user.WorkAreaId.HasValue)
                 workArea = await _context.WorkAreas.FindAsync(user.WorkAreaId.Value);

            List<MenuViewModel> menuList = null;
            if (user.WorkRoleId.HasValue)
                menuList = await _context.WorkAreas
                    .Where(x => x.WorkAreaItems.Any(w => w.WorkRoleItems.Any(wr => wr.WorkRoleId == user.WorkRoleId.Value)))
                    .Select(x => new MenuViewModel
                    {
                        Name = x.Name,
                        Area = ConstantHelpers.Employee.WorkArea.CODE_VALUES[x.IntValue],
                        Items = x.WorkAreaItems
                        .Where(w => w.ParentId == null)
                        .Where(w => w.WorkRoleItems.Any(r => r.WorkRoleId == user.WorkRoleId.Value))
                        .Select(w => new MenuItemViewModel
                        {
                            Name = w.Name,
                            IsGrouping = w.IsItemGroup,
                            Action = w.Action,
                            Controller = w.Controller,
                            Items = w.IsItemGroup ? w.WorkAreaItems
                            .Where(i => i.WorkRoleItems.Any(r => r.WorkRoleId == user.WorkRoleId.Value))
                            .Select(i => new MenuItemViewModel
                            {
                                Name = i.Name,
                                IsGrouping = i.IsItemGroup,
                                Action = i.Action,
                                Controller = i.Controller,
                                Items = i.IsItemGroup ? i.WorkAreaItems
                                .Where(i2 => i2.WorkRoleItems.Any(r => r.WorkRoleId == user.WorkRoleId.Value))
                                .Select(i2 => new MenuItemViewModel
                                {
                                    Name = i2.Name,
                                    IsGrouping = i2.IsItemGroup,
                                    Action = i2.Action,
                                    Controller = i2.Controller,
                                    Items = i2.IsItemGroup ? i.WorkAreaItems.Where(i3 => i3.WorkRoleItems.Any(wri => wri.WorkRoleId == user.WorkRoleId))
                                    .Select(i3 => new MenuItemViewModel
                                    {
                                        Name = i3.Name,
                                        IsGrouping = i3.IsItemGroup,
                                        Action = i3.Action,
                                        Controller = i3.Controller,
                                    }).ToList() : null
                                }).ToList() : null
                            }).ToList() : null
                        }).ToList()
                    }).ToListAsync();

            //Putting our Property to Claims
            ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim(ClaimTypes.UserData, user.RawFullName),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("BelongsToMainOffice", user.BelongsToMainOffice.ToString()),
                new Claim("NewRoleSystem", user.NewRoleSystem.ToString()),
                new Claim(ClaimTypes.Role, string.Join(",", roles)),
                new Claim("Projects", JsonConvert.SerializeObject(projects)),
                new Claim("Menus", JsonConvert.SerializeObject(menuList)),
                new Claim("NewAccount", user.NewAccount.ToString())
            });

            return principal;
        }
    }
}
