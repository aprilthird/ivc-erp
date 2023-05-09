using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.HumanResources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Services
{
    public class WorkItem
    {
        public List<PayrollMovementDetail> Details { get; set; }
        public bool IsTheLast { get; set; }
        public bool IsTheFirst { get; set; }
    }

    public interface IPayrollQueuedBackground
    {
        void QueueBackgroundWorkItem(WorkItem workItem);
    }
    
    public sealed class PayrollQueuedBackground : BackgroundService, IPayrollQueuedBackground
    {
        private static readonly ConcurrentQueue<WorkItem> _workItems = new ConcurrentQueue<WorkItem>();
        private static readonly SemaphoreSlim _signal = new SemaphoreSlim(0);


        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public PayrollQueuedBackground(ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory)
        {
            _logger = loggerFactory.CreateLogger<PayrollQueuedBackground>();
            _scopeFactory = scopeFactory;
        }

        /// <summary>
		/// Transient method via IQueuedBackgroundService
		/// </summary>
        public void QueueBackgroundWorkItem(WorkItem workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
            _logger.LogInformation("Queued Hosted Service is starting. Id:" + workItem.Details[0].Id);
        }

        /// <summary>
		/// Long running task via BackgroundService
		/// </summary>
        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _signal.WaitAsync(cancellationToken);
                    var _workItem = _workItems.TryDequeue(out var workItem) ? workItem : null;
                    //var workItem = await TaskQueue.DequeueAsync(cancellationToken);
                    _logger.LogInformation("Getting workItem.");
                    if (_workItem != null)
                    {
                        _logger.LogInformation("Trying to create scope.");
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();                            
                            _logger.LogInformation("Checking dups");
                            var prevs = await _context.PayrollMovementDetails
                                .Where(x => x.WorkerId == workItem.Details.First().WorkerId &&
                                            x.PayrollMovementHeaderId == workItem.Details.First().PayrollMovementHeaderId)
                                .ToListAsync();
                            if (prevs.Count > 0)
                            {
                                _logger.LogInformation("Deleting dups");
                                _context.PayrollMovementDetails.RemoveRange(prevs);
                            }
                            _logger.LogInformation("Trying to save");
                            await _context.PayrollMovementDetails.AddRangeAsync(
                                _workItem.Details
                                    .Where(x => x.Value > 0.0M || x.PayrollConcept.Code.Contains("T"))
                                    .Select(x => new PayrollMovementDetail
                                        {
                                            PayrollConceptId = x.PayrollConceptId,
                                            PayrollMovementHeaderId = x.PayrollMovementHeaderId,
                                            Value = decimal.Round(x.Value,2),
                                            WorkerId = x.WorkerId
                                        }
                                    ).ToList()
                                );
                            if (_workItem.IsTheFirst)
                            {
                                var header = await _context.PayrollMovementHeaders.FirstOrDefaultAsync(x => x.Id == _workItem.Details[0].PayrollMovementHeaderId);
                                header.ProcessStatus = 1;
                            }
                            if (_workItem.IsTheLast)
                            {
                                var header = await _context.PayrollMovementHeaders.FirstOrDefaultAsync(x => x.Id == _workItem.Details[0].PayrollMovementHeaderId);
                                header.ProcessStatus = 2;
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                    //await workItem(cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                       "Error occurred executing PayrollQueued.");
                }
            }

            _logger.LogInformation("Queued Hosted Service is stopping.");
        }
    }

    
}
