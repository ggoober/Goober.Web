using Goober.WebJobs.Example.Services;
using Microsoft.Extensions.Logging;
using System;

namespace Goober.WebJobs.Example.Jobs
{
    class IterateExampleJob : IterateJob<IIterateExampleJobService>
    {
        public IterateExampleJob(ILogger<IterateExampleJob> logger, 
            IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }
    }
}
