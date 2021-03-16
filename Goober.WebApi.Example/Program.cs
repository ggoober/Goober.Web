using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goober.WebApi.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Goober.WebApi.ProgramUtils.RunWebhost<Startup>(args);
        }
    }
}
