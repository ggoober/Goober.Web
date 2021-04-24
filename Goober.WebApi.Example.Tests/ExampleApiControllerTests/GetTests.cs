using Goober.Core.Extensions;
using Goober.Tests;
using Goober.WebApi.Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Goober.WebApi.Example.Tests.ExampleApiControllerTests
{
    public class GetTests
    {
        [Fact]
        public async Task GetTest()
        {
            var sut = TestUtils.GenerateSut();

            //arrange
            var expectedResult = new GetResponse 
            { 
                IntValue = sut.CreateFixture<int>(), 
                FloatValue = sut.CreateFixture<float>(), 
                StringValue = sut.CreateFixture<string>(), 
                DateValue = sut.CreateFixture<DateTime>(), 
                EnumValue = sut.CreateEnumFixture<ExampleEnum>(),
                IntList = sut.CreateManyFixture<int>(new Random().Next(3, 20))
            };

            var queryParameters = new List<KeyValuePair<string, string>>();
            queryParameters.Add(new KeyValuePair<string, string>("intValue", expectedResult.IntValue.ToString()));
            queryParameters.Add(new KeyValuePair<string, string>("floatValue", expectedResult.FloatValue.ToString()));
            queryParameters.Add(new KeyValuePair<string, string>("stringValue", expectedResult.StringValue));
            queryParameters.Add(new KeyValuePair<string, string>("dateValue", expectedResult.DateValue.ToString("o")));
            queryParameters.Add(new KeyValuePair<string, string>("enumValue", expectedResult.EnumValue.ToString()));
            if (expectedResult.IntList.Any() == true)
            {
                foreach (var iIntItem in expectedResult.IntList)
                {
                    queryParameters.Add(new KeyValuePair<string, string>("intList", iIntItem.ToString()));
                }
            }
            
            //act
            var res = await sut.ExecuteGetAsync<GetResponse>(urlPath: "api/example/get", queryParameters: queryParameters);

            //assert
            var strExpected = expectedResult.Serialize();

            Assert.Equal(expected: strExpected, actual: res.Serialize());
        }

    }
}
