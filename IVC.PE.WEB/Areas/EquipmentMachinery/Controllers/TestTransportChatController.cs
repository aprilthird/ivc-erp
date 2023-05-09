using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/prueba-grafico-transporte")]
    public class TestTransportChatController : BaseController
    {
        public TestTransportChatController(IvcDbContext context,
    ILogger<TestTransportChatController> logger)
    : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("barras-transporte")]
        public async Task<IActionResult> GetTransports(/*Guid? pId = null*/)
        {
            var usp = await _context.Set<UspTransportCounts>().FromSqlRaw("execute EquipmentMachinery_uspCountTransports")
                .IgnoreQueryFilters()
                .ToListAsync();

            var chart = new ChartViewModel();

            chart.cols = new List<ChartViewModel.Col>
            {
                {
                    new ChartViewModel.Col
                    {
                        label = "Transporte",
                        type = "string"
                    }                    
                },
                {
                    new ChartViewModel.Col
                    {
                        label = "Cantidad",
                        type = "number"
                    }
                }
                ,
                {
                    new ChartViewModel.Col
                    {
                        role="annotation"
                    }
                }
            };

            chart.rows = new List<ChartViewModel.Row>();
            foreach (var tr in usp)
            {
                chart.rows.Add(new ChartViewModel.Row
                {
                    c = new List<ChartViewModel.C>
                    {
                        {
                            new ChartViewModel.C
                            {
                                v = tr.Transport
                            }
                        },
                        {
                            new ChartViewModel.C
                            {
                                v = tr.Quantity
                            }
                        },
                        {
                            new ChartViewModel.C
                            {
                                v = tr.Quantity
                            }
                        }
                    }
                });
            }            

            return Ok(chart);
        }
    }
}
