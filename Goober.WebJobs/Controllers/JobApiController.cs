using Goober.WebJobs.Abstractions;
using Goober.WebJobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs.Controllers
{
    public class JobApiController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        public JobApiController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Route("api/job/ping")]
        public virtual PingApiResponse Ping()
        {
            var ret = new PingApiResponse();

            var jobs = _serviceProvider.GetServices<IHostedService>();

            foreach (var iJob in jobs)
            {
                var baseJob = iJob as BaseJob;
                if (baseJob == null)
                    continue;

                var newWorker = new WebJobPingModel
                {
                    IsEnabled = baseJob.IsEnabled,
                    IsRunning = baseJob.IsRunning,
                    Name = baseJob.GetType().FullName,
                    ServiceUpTimeInSec = Convert.ToInt64(baseJob.ServiceUpTime.TotalSeconds),
                    IsCancellationRequested = baseJob.IsCancellationRequested
                };

                var iterateJob = baseJob as IIterateJobMetrics;

                if (iterateJob != null)
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
                    newWorker.Iterate = iterateMetrics;
                }

                var listJob = baseJob as IListJobMetrics;
                if (listJob != null)
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
                        LastIterationListItemExecuteDateTime = listJob.LastIterationListItemExecuteDateTime
                    };
                    newWorker.List = listMetrics;
                }

                ret.Services.Add(newWorker);
            }

            return ret;
        }

        [HttpPost]
        [Route("api/job/start")]
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

            await job.StartAsync(new CancellationToken());

            return new StartJobResponse { IsStarted = isStarted };
        }

        [HttpPost]
        [Route("api/job/stop")]
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
            var hostedServices = _serviceProvider.GetServices<IHostedService>();

            foreach (var iHostedService in hostedServices)
            {
                var baseJob = iHostedService as BaseJob;

                if (baseJob == null)
                    continue;

                if (baseJob.ClassName == name)
                    return baseJob;
            }

            return null;
        }
    }
}
