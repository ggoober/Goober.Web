using System;
using Indusoft.WebJobs.Example.Jobs.Services;
using Microsoft.Extensions.Logging;

namespace Indusoft.WebJobs.Example.Jobs
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
