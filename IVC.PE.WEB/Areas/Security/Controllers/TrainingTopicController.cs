using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Security;
using IVC.PE.WEB.Areas.Security.ViewModels.TrainingTopicViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using IVC.PE.WEB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Security.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Security.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.SECURITY)]
    [Route("seguridad/capacitaciones/temas")]
    public class TrainingTopicController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public TrainingTopicController(IvcDbContext context,
            ILogger<TrainingTopicController> logger,
            IOptions<CloudStorageCredentials> storageCredentials) 
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? trainingCategoryId = null)
        {
            var projectId = GetProjectId();

            var query = _context.TrainingTopics
                .Where(x => x.ProjectId == projectId)
                .AsQueryable();

            if(trainingCategoryId.HasValue)
                query = query.Where(x => x.TrainingCategoryId == trainingCategoryId.Value);

            var data = await query
                .Select(x => new TrainingTopicViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    TrainingCategoryId = x.TrainingCategoryId,
                    TrainingCategory = new ViewModels.TrainingCategoryViewModels.TrainingCategoryViewModel
                    {
                        Name = x.TrainingCategory.Name
                    }
                }).AsNoTracking().ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var topic = await _context.TrainingTopics
                .Where(x => x.Id == id)
                .Select(x => new TrainingTopicViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    TrainingCategoryId = x.TrainingCategoryId
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(topic);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(TrainingTopicViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var topic = new TrainingTopic
            {
                Name = model.Name,
                TrainingCategoryId = model.TrainingCategoryId,
                ProjectId = GetProjectId()
            };
            await _context.TrainingTopics.AddAsync(topic);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, TrainingTopicViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var topic = await _context.TrainingTopics.FindAsync(id);
            topic.Name = model.Name;
            topic.TrainingCategoryId = model.TrainingCategoryId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var topic = await _context.TrainingTopics.FindAsync(id);
            if (topic is null)
                return BadRequest($"Tema con Id '{id}' no encontrado.");
            _context.TrainingTopics.Remove(topic);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}/archivos")]
        public async Task<IActionResult> Files(Guid id)
        {
            var data = await _context.TrainingTopicFiles
                .Where(x => x.TrainingTopicId == id)
                .Select(x => new SelectViewModel
                {
                    Id = x.Id,
                    Text = x.Url.ToString()
                }).AsNoTracking().ToListAsync();

            return Ok(data);
        }

        [HttpPost("{id}/archivos")]
        public async Task<IActionResult> UploadFile(Guid id, IFormFile file)
        {
            var topic = await _context.TrainingTopics.FindAsync(id);
            if (topic == null)
                return BadRequest($"El tema de Id '{id}' no existe.");
            var topicFile = new TrainingTopicFile()
            {
                TrainingTopicId = topic.Id
            };
            var storage = new CloudStorageService(_storageCredentials);
            if (await storage.FileExists(ConstantHelpers.Storage.Containers.SECURITY, $"{ConstantHelpers.Storage.Blobs.TRAINING_TOPIC}/{file.FileName}"))
                return BadRequest($"Ya existe un archivo con el mismo nombre. Si desea sobreescribirlo, elimínelo y vuelva a cargar el archivo.");
            topicFile.Url = await storage.UploadFileWithExtension(file.OpenReadStream(),
                ConstantHelpers.Storage.Containers.SECURITY, file.FileName,
                ConstantHelpers.Storage.Blobs.TRAINING_TOPIC);
            await _context.TrainingTopicFiles.AddAsync(topicFile);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}/archivos/eliminar/{fid}")]
        public async Task<IActionResult> DeleteFile(Guid fid)
        {
            var file = await _context.TrainingTopicFiles.FirstOrDefaultAsync(x => x.Id == fid);

            if (file.Url != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                var fr = file.Url.AbsolutePath.Split('/').Last();
                var fileUrl = $"{ConstantHelpers.Storage.Blobs.TRAINING_TOPIC}/{file.Url.AbsolutePath.Split('/').Last()}";
                await storage.TryDelete(fileUrl, ConstantHelpers.Storage.Containers.SECURITY);
            }
            _context.TrainingTopicFiles.Remove(file);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
