using System;

namespace Goober.WebScheduler
{
    internal interface IServiceDelegate
    {
        Delegate CreateServiceDelegate();
    }
}
