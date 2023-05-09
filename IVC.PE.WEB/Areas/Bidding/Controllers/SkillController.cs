using System;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.SkillRenovationViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.SkillViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionalsViewModels;
using IVC.PE.WEB.Services;
using Microsoft.Extensions.Options;
using IVC.PE.WEB.Options;
using IVC.PE.ENTITIES.UspModels.Biddings;
using System.Data;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationTypeViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;

namespace IVC.PE.WEB.Areas.Bidding.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Bidding.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.BIDDING)]
    [Route("licitaciones/habilidades")]
    public class SkillController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public SkillController(IvcDbContext context,
        IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<SkillController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {

            var skills = await _context.Set<UspSkills>().FromSqlRaw("execute Biddings_uspSkills")
                .IgnoreQueryFilters()
                .ToListAsync();



            return Ok(skills);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var skill = await _context.Skills
                .Where(x => x.Id == id)
                .Select(x => new SkillViewModel
                {
                    Id = x.Id,
                    ProfessionalId = x.ProfessionalId,
                    Professional = new ProfessionalViewModel
                    {
                        Id = x.ProfessionalId,
                        PaternalSurname = x.Professional.PaternalSurname,
                        MaternalSurname = x.Professional.MaternalSurname,
                        Document = x.Professional.Document,
                        Email = x.Professional.Email,
                        CIPNumber = x.Professional.CIPNumber,
                        ProfessionId = x.Professional.ProfessionId
                    },
                    NumberOfRenovations = x.NumberOfRenovations,
                }).FirstOrDefaultAsync();

            var skillRenovation = await _context.SkillRenovations
                .Where(x => x.SkillId == skill.Id && x.SkillOrder == skill.NumberOfRenovations)
                .Select(x => new SkillRenovationViewModel
                {
                    Id = x.Id,
                    SkillId = x.SkillId,
                    SkillOrder = x.SkillOrder,
                    CreateDate = x.CreateDate.ToDateString(),
                    EndDate = x.EndDate.ToDateString(),
                    FileUrl = x.FileUrl,
                    IsTheLast = x.IsTheLast,
                }).FirstOrDefaultAsync();

            skill.SkillRenovation = skillRenovation;

            return Ok(skill);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SkillViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var skill = new Skill
            {
                ProfessionalId = model.ProfessionalId,
                NumberOfRenovations = 1,
            };

            var skillRenovation = new SkillRenovation
            {
                Skill = skill,
                SkillOrder = skill.NumberOfRenovations,
                CreateDate = model.SkillRenovation.CreateDate.ToDateTime(),
                EndDate = model.SkillRenovation.EndDate.ToDateTime(),
                Days15 = false,
                Days30 = false,
                IsTheLast = model.SkillRenovation.IsTheLast
            };




            await _context.SkillRenovations.AddAsync(skillRenovation);
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();

            if (model.SkillRenovation.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                skillRenovation.FileUrl = await storage.UploadFile(model.SkillRenovation.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.SkillRenovation.File.FileName),
                    ConstantHelpers.Storage.Blobs.SKILL,
                   $"habilidad_{skill.Id}_nro-{skillRenovation.SkillOrder}");
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SkillViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var skill = await _context.Skills.FirstOrDefaultAsync(x => x.Id == id);
            skill.ProfessionalId = model.ProfessionalId;

            var skillRen = await _context.SkillRenovations.FirstOrDefaultAsync(x => x.Id == model.SkillRenovation.Id);

            skillRen.CreateDate = model.SkillRenovation.CreateDate.ToDateTime();
            skillRen.EndDate = model.SkillRenovation.EndDate.ToDateTime();
            skillRen.IsTheLast = model.SkillRenovation.IsTheLast;

            if (model.SkillRenovation.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (skillRen.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SKILL}/{skillRen.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                skillRen.FileUrl = await storage.UploadFile(model.SkillRenovation.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.SkillRenovation.File.FileName),
                    ConstantHelpers.Storage.Blobs.SKILL,
                    $"habilidad_{skill.Id}_nro-{skillRen.SkillOrder}");
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var skill = await _context.Skills.FirstOrDefaultAsync(x => x.Id == id);

            var skillRenovations = await _context.SkillRenovations.Where(x => x.SkillId == id).ToListAsync();

            foreach (var renovation in skillRenovations)
            {
                if (renovation.FileUrl != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SKILL}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.BIDDING);
                }
            }

            _context.SkillRenovations.RemoveRange(skillRenovations);
            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
