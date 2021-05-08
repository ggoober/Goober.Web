using Goober.WebJobs.Example.Jobs.Services;
using Microsoft.Extensions.Logging;
using System;

namespace Goober.WebJobs.Example.Jobs
{
    class ListExampleJob : ListJob<int, IListExampleJobService>
    {
        public ListExampleJob(
            ILogger<ListExampleJob> logger, 
            IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }
    }
}
