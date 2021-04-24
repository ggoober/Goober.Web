using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Goober.Core.Extensions;

namespace Goober.WebApi.Example.Tests.ExampleApiControllerTests
{
    public class PostFormTests
    {
        [Fact]
        public async Task PostFormTest()
        {
            var sut = TestUtils.GenerateSut();

            //arrange
            var expectedResult = new 
            {
                id = sut.CreateFixture<int>(),
                name = sut.CreateFixture<string>(),
                date = sut.CreateFixture<DateTime>()
            };

            var formData = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("id", expectedResult.id.ToString()),
                new KeyValuePair<string, string>("name", expectedResult.name),
                new KeyValuePair<string, string>("date", expectedResult.date.ToString("O"))
            };

            //act
            var res = await sut.ExecutePostFormDataReturnStringAsync(urlPath: "/api/example/post-form/", 
                formData: formData);

            //assert
            Assert.Equal(expected: expectedResult.Serialize(), actual: res);
        }
    }
}
