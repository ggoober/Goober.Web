using Goober.BackgroundWorker.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Goober.BackgroundWorker
{
    public class UdpBackgroundWorker : BaseBackgroundWorker
    {
        public UdpBackgroundWorker(ILogger logger, 
            IServiceProvider serviceProvider, 
            IOptions<BackgroundWorkersOptions> optionsAccessor) 
            : base(logger, serviceProvider, optionsAccessor)
        {

        }
    }
}
