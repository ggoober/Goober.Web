using System;
using Indusoft.WebJobs.Example.Jobs.Services;
using Microsoft.Extensions.Logging;

namespace Indusoft.WebJobs.Example.Jobs
{
    class IterateExampleJob : IterateJob<IIterateExampleJobService>
    {
        public IterateExampleJob(ILogger<IterateExampleJob> logger, 
            IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }
    }
    
    class IterateExampleJob1 : IterateJob<IIterateExampleJobService1>
    {
        public IterateExampleJob1(ILogger<IterateExampleJob1> logger, 
            IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }
    }class IterateExampleJob2 : IterateJob<IIterateExampleJobService>
    {
        public IterateExampleJob2(ILogger<IterateExampleJob2> logger, 
            IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }
    }class IterateExampleJob3 : IterateJob<IIterateExampleJobService>
    {
        public IterateExampleJob3(ILogger<IterateExampleJob3> logger, 
            IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }
    }class IterateExampleJob4 : IterateJob<IIterateExampleJobService>
    {
        public IterateExampleJob4(ILogger<IterateExampleJob4> logger, 
            IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }
    }class IterateExampleJob5 : IterateJob<IIterateExampleJobService>
    {
        public IterateExampleJob5(ILogger<IterateExampleJob5> logger, 
            IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }
    }class IterateExampleJob6 : IterateJob<IIterateExampleJobService>
    {
        public IterateExampleJob6(ILogger<IterateExampleJob6> logger, 
            IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }
    }class IterateExampleJob7 : IterateJob<IIterateExampleJobService>
    {
        public IterateExampleJob7(ILogger<IterateExampleJob7> logger, 
            IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }
    }class IterateExampleJob8 : IterateJob<IIterateExampleJobService>
    {
        public IterateExampleJob8(ILogger<IterateExampleJob8> logger, 
            IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }
    }
}
