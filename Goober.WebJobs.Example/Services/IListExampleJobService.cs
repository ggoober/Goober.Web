using Goober.WebJobs.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goober.WebJobs.Example.Services
{
    interface IListExampleJobService: IListJobService<int>
    {
    }
}
