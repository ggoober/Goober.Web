using Goober.Base.Attributes;
using Goober.Web.Filters;
using Goober.WebJobs.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Goober.WebJobs.Api.Models;
using Microsoft.Extensions.Logging;

namespace Goober.WebJobs.Controllers
{
    public class JobApiController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<JobApiController> _logger;

        public JobApiController(IServiceProvider serviceProvider, ILogger<JobApiController> logger)
        {
	        _serviceProvider = serviceProvider;
	        _logger = logger;
        }

        [HttpGet]
        [Route("api/job/ping")]
        [SwaggerHideInDocs]
        [BasicAuth]
        public virtual PingApiResponse Ping()
        {
	        var sw = new Stopwatch();
            sw.Start();
            var ret = new PingApiResponse();
            var jobs = _serviceProvider.GetServices<IHostedService>();
            foreach (var baseJob in jobs.OfType<BaseJob>())
            {
	            var newWorker = new WebJobPingModel
                {
                    IsEnabled = baseJob.IsEnabled,
                    IsRunning = baseJob.IsRunning,
                    IsExecuting = baseJob.IsExecuting,
                    State = baseJob.State,
                    ReasonForState = baseJob.ReasonForState,
                    PriorityCompareType = baseJob.PriorityCompareType,
                    ClusterNodePriority = baseJob.ClusterNodePriority,
                    IsCluster = baseJob.IsCluster,
                    Name = baseJob.GetType().FullName,
                    ServiceUpTimeInSec = Convert.ToInt64(baseJob.ServiceUpTime.TotalSeconds),
                    IsCancellationRequested = baseJob.IsCancellationRequested,
                    RetryDelayInMilliseconds = baseJob.RetryDelayInMilliseconds
                };

                switch (baseJob)
                {
	                case ISimpleJobMetrics simpleJob:
	                {
		                var simpleMetrics = GetSimpleJobPingModel(simpleJob);
		                newWorker.Simple = simpleMetrics;
		                break;
	                }
	                case IIterateJobMetrics iterateJob:
	                {
		                var iterateMetrics = GetIterateJobPingModel(iterateJob);
		                newWorker.Iterate = iterateMetrics;
		                break;
                        }
	                case IListJobMetrics listJob:
	                {
		                var listMetrics = GetListJobPingModel(listJob);
		                newWorker.List = listMetrics;
                            break;
	                }
                }

                ret.Services.Add(newWorker);
            }
            sw.Stop();
            _logger.LogDebug($"PingApi elapsed {sw.ElapsedMilliseconds}ms");
            return ret;
        }

        [HttpPost]
        [Route("api/job/start")]
        [SwaggerHideInDocs]
        [BasicAuth]
        public virtual async Task<StartJobResponse> StartJobAsync([FromBody]StartJobRequest request)
        {
            if (request == null)
                throw new InvalidOperationException("request can't be empty");

            if (string.IsNullOrEmpty(request.JobClassName) == true)
                throw new ArgumentNullException("request.JobClassName");

            var job = GetJobByClassName(request.JobClassName);

            if (job == null)
                throw new InvalidOperationException($"Can't find job by name = {request.JobClassName}");

            var isStarted = job.IsRunning == false;

            await job.ForceStartAsync(new CancellationToken());

            return new StartJobResponse { IsStarted = isStarted };
        }

        [HttpPost]
        [Route("api/job/stop")]
        [SwaggerHideInDocs]
        [BasicAuth]
        public virtual async Task<StopJobResponse> StopJobAsync([FromBody]StopJobRequest request)
        {
            if (request == null)
                throw new InvalidOperationException("request can't be empty");

            if (string.IsNullOrEmpty(request.JobClassName) == true)
                throw new ArgumentNullException("request.JobClassName");
            
            var job = GetJobByClassName(request.JobClassName);

            if (job == null)
                throw new InvalidOperationException($"Can't find job by name = {request.JobClassName}");

            var isStopped = job.IsRunning == true;

            await job.StopAsync(new CancellationToken());

            return new StopJobResponse { IsStopped = isStopped };
        }

        private BaseJob GetJobByClassName(string name)
        {
            return _serviceProvider
	            .GetServices<IHostedService>()
                .OfType<BaseJob>()
	            .FirstOrDefault(job => job.ClassName == name);
        }

        private static ListJobPingModel GetListJobPingModel(IListJobMetrics listJob)
        {
            var listMetrics = new ListJobPingModel
            {
                MaxDegreeOfParallelism = listJob.MaxDegreeOfParallelism,
                UseSemaphoreParallelism = listJob.UseSemaphoreParallelism,
                LastIterationListItemsCount = listJob.LastIterationListItemsCount,
                LastIterationListItemsProcessedCount = listJob.LastIterationListItemsProcessedCount,
                LastIterationListItemsSuccessProcessedCount = listJob.LastIterationListItemsSuccessProcessedCount,
                LastIterationListItemsLastDurationInMilliseconds = listJob.LastIterationListItemsLastDurationInMilliseconds,
                LastIterationListItemsAvgDurationInMilliseconds = listJob.LastIterationListItemsAvgDurationInMilliseconds,
                LastIterationListItemExecuteDateTime = listJob.LastIterationListItemExecuteDateTime,
                ListItemProcessingRetryCount = listJob.ListItemProcessingRetryCount
            };
            return listMetrics;
        }

        private static IterateJobPingModel GetIterateJobPingModel(IIterateJobMetrics iterateJob)
        {
            var iterateMetrics = new IterateJobPingModel
            {
                TaskDelayInMilliseconds = iterateJob.TaskDelayInMilliseconds,
                IteratedCount = iterateJob.IteratedCount,
                SuccessIteratedCount = iterateJob.SuccessIteratedCount,
                LastIterationStartDateTime = iterateJob.LastIterationStartDateTime,
                LastIterationFinishDateTime = iterateJob.LastIterationFinishDateTime,
                LastIterationDurationInMilliseconds = iterateJob.LastIterationDurationInMilliseconds,
                AvgIterationDurationInMilliseconds = iterateJob.AvgIterationDurationInMilliseconds
            };
            return iterateMetrics;
        }

        private static SimpleJobPingModel GetSimpleJobPingModel(ISimpleJobMetrics simpleJob)
        {
            var simpleMetrics = new SimpleJobPingModel
            {
                ExecutedCount = simpleJob.ExecutedCount,
                LastExecutedStartDateTime = simpleJob.LastExecutedStartDateTime,
                AvgExecutedDurationInMilliseconds = simpleJob.AvgExecutedDurationInMilliseconds,
                ErrorExecutedCount = simpleJob.ErrorExecutedCount,
                LastExecutedDurationInMilliseconds = simpleJob.LastExecutedDurationInMilliseconds,
                LastExecutedFinishDateTime = simpleJob.LastExecutedFinishDateTime,
                SuccessExecutedCount = simpleJob.SuccessExecutedCount,
            };
            return simpleMetrics;
        }
    }
}
