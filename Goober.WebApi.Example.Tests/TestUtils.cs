using Goober.Tests;
using Goober.Tests.Utils;
using System.Collections.Generic;

namespace Goober.WebApi.Example.Tests
{
    class TestUtils
    {
        public static SystemUnderTest GenerateSut()
        {
            var sut = new SystemUnderTest(new List<string> {
                AssemblyUtils.GetAssemlbyName<Startup>(),
                AssemblyUtils.GetAssemlbyName<Goober.WebApi.Example.Api.Models.GetResponse>()
            });

            sut.Init<Startup>();

            return sut;
        }
    }
}
