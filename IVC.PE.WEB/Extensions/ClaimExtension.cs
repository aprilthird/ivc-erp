using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Extensions
{
    public static class ClaimExtension
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.Email).Value;
            }

            return "";
        }

        public static string GetFullName(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var a = user.Claims.ToList();
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.UserData).Value;
            }

            return "";
        }

        public static string GetPhone(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.HomePhone).Value;
            }

            return "";
        }

        public static bool HasMultipleProjects(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var mm = user.Claims.FirstOrDefault(v => v.Type == "BelongsToMainOffice").Value;
                return mm == "True";
            }

            return false;
        }

        public static bool HasNewRoleSystem(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var result = user.Claims.FirstOrDefault(v => v.Type == "NewRoleSystem").Value;
                return result == "True";
            }

            return false;
        }

        public static string[] GetRoles(this ClaimsPrincipal user)
        {
            if(user.Identity.IsAuthenticated)
            {
                //return string.Join(",", user.Claims.Where(v => v.Type == ClaimTypes.Role).ToList().Select(x => x.Value));
                //return user.Claims.Where(v => v.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();
                return user.Claims.LastOrDefault(v => v.Type == ClaimTypes.Role).Value.Split(",");
            }

            return null;
        }

        public static Project[] GetProjects(this ClaimsPrincipal user)
        {
            if(user.Identity.IsAuthenticated)
            {
                return JsonConvert.DeserializeObject<Project[]>(user.Claims.FirstOrDefault(v => v.Type == "Projects").Value);
            }

            return null;
        }

        public static List<MenuViewModel> GetMenu(this ClaimsPrincipal user)
        {
            var menuList = new List<MenuViewModel>();
            if (user.Identity.IsAuthenticated)
            {
                var menuStrClaim = user.Claims.FirstOrDefault(v => v.Type == "Menus").Value;
                if (!string.IsNullOrEmpty(menuStrClaim))
                    menuList = JsonConvert.DeserializeObject<List<MenuViewModel>>(menuStrClaim) ?? new List<MenuViewModel>();
            }

            return menuList;
        }

        public static bool IsNewAccount(this ClaimsPrincipal user)
        {
            if(user.Identity.IsAuthenticated)
            {
                var result = user.Claims.FirstOrDefault(v => v.Type == "NewAccount").Value;
                return result == "True";
            }

            return false;
        }

        public static bool IsInAnyRole(this ClaimsPrincipal user, string roleNames)
        {
            if (user.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(roleNames))
                {
                    var roleNamesList = roleNames.Split(",");
                    var roles = user.GetRoles();
                    var result = roles.Any(r => roleNamesList.Contains(r));
                    return result;
                }
            }

            return false;
        }

        public static bool IsInAnyRole(this ClaimsPrincipal user, List<string> roleNames)
        {
            if(user.Identity.IsAuthenticated)
            {
                var roles = user.GetRoles();
                return roles.Any(r => roleNames.Contains(r));
            }

            return false;
        }
    }
}
