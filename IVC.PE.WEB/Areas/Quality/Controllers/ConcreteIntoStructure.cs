using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/vaciado-concreto-estructuras")]

    public class ConcreteIntoStructure : BaseController
    {
        public ConcreteIntoStructure(IvcDbContext context,
        ILogger<ConcreteIntoStructure> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();
    }
}
