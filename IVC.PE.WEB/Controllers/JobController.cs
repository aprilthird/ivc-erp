using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Route("tareas")]
    public class JobController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public JobController(IvcDbContext context, 
            UserManager<ApplicationUser> userManager,
            ILogger<JobController> logger,
            IOptions<CloudStorageCredentials> storageCredentials) : base(context, userManager, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("usuarios/actualizar-contrasenas")]
        public async Task<IActionResult> UpdatePasswords()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var users = await _context.Users
                .Where(x => !x.UserRoles.Any(ur => ur.Role.Name == ConstantHelpers.Roles.SUPERADMIN))
                .ToListAsync();
            var employes = await _context.Employees.AsNoTracking().ToListAsync();
            foreach (var user in users)
            {
                var employee = employes.Where(x => x.Email == user.Email).FirstOrDefault();
                if(employee != null)
                {
                    user.NewAccount = true;
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, employee.Document);
                }
            }
            var u1 = users.Where(x => x.Email == "ivalle@ivc.pe").FirstOrDefault();
            if(u1 != null)
            {
                u1.NewAccount = true;
                u1.PasswordHash = _userManager.PasswordHasher.HashPassword(u1, "07640760");
            }
            var u2 = users.Where(x => x.Email == "rvelazco@ivc.pe").FirstOrDefault();
            if (u2 != null)
            {
                u2.NewAccount = true;
                u2.PasswordHash = _userManager.PasswordHasher.HashPassword(u2, "21419174");
            }
            var u3 = users.Where(x => x.Email == "pcuentas@ivc.pe").FirstOrDefault();
            if (u3 != null)
            {
                u3.NewAccount = true;
                u3.PasswordHash = _userManager.PasswordHasher.HashPassword(u3, "06198639");
            }
            var u4 = users.Where(x => x.Email == "htirado@ivc.pe").FirstOrDefault();
            if (u4 != null)
            {
                u4.NewAccount = true;
                u4.PasswordHash = _userManager.PasswordHasher.HashPassword(u4, "08674408");
            }
            var u5 = users.Where(x => x.Email == "logistica@ivc.pe").FirstOrDefault();
            if (u5 != null)
            {
                u5.NewAccount = true;
                u5.PasswordHash = _userManager.PasswordHasher.HashPassword(u5, "74208699");
            }
            var u6 = users.Where(x => x.Email == "cyanagui@ivc.pe").FirstOrDefault();
            if (u6 != null)
            {
                u6.NewAccount = true;
                u6.PasswordHash = _userManager.PasswordHasher.HashPassword(u6, "42978398");
            }
            await _context.SaveChangesAsync();
            stopWatch.Stop();
            var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds / 10);
            return Ok($"Completado en {elapsedTime}.");
        }

        [HttpGet("cartas-enviadas/actualizar-archivos")]
        public async Task<IActionResult> UpdateSentLetterFiles()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var data = await _context.Letters
                .Where(x => x.Type == ConstantHelpers.Letter.Type.SENT)
                .ToListAsync();
            var storage = new CloudStorageService(_storageCredentials);
            foreach (var letter in data)
                letter.FileUrl = await storage.GetFile(ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL, $"{ConstantHelpers.Storage.Blobs.LETTERS_SENT}/{letter.Code}.pdf");
            await _context.SaveChangesAsync();
            stopWatch.Stop();
            var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds / 10);
            return Ok($"Completado en {elapsedTime}.");
        }

        [HttpGet("cartas-recibidas/actualizar-archivos")]
        public async Task<IActionResult> UpdateReceivedLetterFiles()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var data = await _context.Letters
                .Where(x => x.Type == ConstantHelpers.Letter.Type.RECEIVED)
                .ToListAsync();
            var storage = new CloudStorageService(_storageCredentials);
            foreach (var letter in data)
                letter.FileUrl = await storage.GetFile(ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL, $"{ConstantHelpers.Storage.Blobs.LETTERS_RECEIVED}/{letter.Code}.pdf");
            await _context.SaveChangesAsync();
            stopWatch.Stop();
            var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds / 10);
            return Ok($"Completado en {elapsedTime}.");
        }

        [HttpGet("control-documentario/actualizar-estado-asientos")]
        public async Task<IActionResult> UpdateWorkbookSeatStatus(bool emptyValues = false)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var query = _context.WorkbookSeats.AsQueryable();
            if (emptyValues)
                query = query.Where(x => x.Status == 0);
            var data = await query.ToListAsync();
            foreach (var seat in data)
                seat.Status = ConstantHelpers.Workbook.Status.PENDING;
            await _context.SaveChangesAsync();
            stopWatch.Stop();
            var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds / 10);
            return Ok($"Completado en {elapsedTime}.");
        }
    }
}