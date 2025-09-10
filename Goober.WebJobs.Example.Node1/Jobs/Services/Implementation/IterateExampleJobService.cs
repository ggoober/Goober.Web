using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Indusoft.WebJobs.Example.Jobs.Services.Implementation
{
    public class IterateExampleJobService : IIterateExampleJobService
    {
        private readonly ILogger<IterateExampleJobService> _logger;

        public IterateExampleJobService(ILogger<IterateExampleJobService> logger)
        {
            this._logger = logger;
        }

        public async Task ExecuteIterationAsync(CancellationToken stoppingToken)
        {
            _logger.LogTrace($"{GetType().Name}.{nameof(ExecuteIterationAsync)} start processing iteration");

            var random = new Random();

            var sleep = random.Next(50, 60) * 1000;

            await Task.Delay(millisecondsDelay: sleep);

            _logger.LogTrace($"{GetType().Name}.{nameof(ExecuteIterationAsync)} finish processing iteration");
        }
    }

	public class IterateExampleJobService1: IterateExampleJobService, IIterateExampleJobService1
    {
	    public IterateExampleJobService1(ILogger<IterateExampleJobService> logger) : base(logger)
	    {
	    }
    }public class IterateExampleJobService2: IterateExampleJobService, IIterateExampleJobService2
    {
	    public IterateExampleJobService2(ILogger<IterateExampleJobService> logger) : base(logger)
	    {
	    }
    }public class IterateExampleJobService3: IterateExampleJobService, IIterateExampleJobService3
    {
	    public IterateExampleJobService3(ILogger<IterateExampleJobService> logger) : base(logger)
	    {
	    }
    }public class IterateExampleJobService4: IterateExampleJobService, IIterateExampleJobService4
    {
	    public IterateExampleJobService4(ILogger<IterateExampleJobService> logger) : base(logger)
	    {
	    }
    }public class IterateExampleJobService5: IterateExampleJobService, IIterateExampleJobService5
    {
	    public IterateExampleJobService5(ILogger<IterateExampleJobService> logger) : base(logger)
	    {
	    }
    }public class IterateExampleJobService6: IterateExampleJobService, IIterateExampleJobService6
    {
	    public IterateExampleJobService6(ILogger<IterateExampleJobService> logger) : base(logger)
	    {
	    }
    }public class IterateExampleJobService7: IterateExampleJobService, IIterateExampleJobService7
    {
	    public IterateExampleJobService7(ILogger<IterateExampleJobService> logger) : base(logger)
	    {
	    }
    }public class IterateExampleJobService8: IterateExampleJobService, IIterateExampleJobService8
    {
	    public IterateExampleJobService8(ILogger<IterateExampleJobService> logger) : base(logger)
	    {
	    }
    }
}
