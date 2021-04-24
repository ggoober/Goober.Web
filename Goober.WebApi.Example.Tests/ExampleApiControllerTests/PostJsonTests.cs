using AutoFixture;
using Goober.Core.Extensions;
using Goober.WebApi.Example.Models;
using Goober.WebApi.Example.Services;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Goober.WebApi.Example.Tests.ExampleApiControllerTests
{
    public class PostJsonTests
    {
        [Fact]
        public async Task PostJsonTest()
        {
            var sut = TestUtils.GenerateSut();

            //arrange
            var request = new PostJsonRequest
            {
                IntValue = sut.CreateFixture<int>(),
                FloatValue = sut.CreateFixture<float>(),
                DateValue = sut.CreateFixture<DateTime>(),
                EnumValue = sut.CreateEnumFixture<ExampleEnum>(),
                StringValue = sut.CreateFixture<string>(),
                IntList = sut.CreateManyFixture<int>(new Random().Next(3, 20))
            };

            //act
            var res = await sut.ExecutePostAsync<PostJsonResponse, PostJsonRequest>(
                urlPath: "/api/example/post-json",
                request: request);

            //assert
            var expectedResult = new PostJsonResponse
            {
                Message = $"post-json {request.Serialize()}"
            };

            Assert.Equal(expected: expectedResult.Serialize(), actual: res.Serialize());
        }

        [Fact]
        public async Task PostJsonExecuteThroughHttpTest()
        {
            var sut = TestUtils.GenerateSut();

            //arrange
            var request = new PostJsonRequest
            {
                IntValue = sut.CreateFixture<int>(),
                FloatValue = sut.CreateFixture<float>(),
                DateValue = sut.CreateFixture<DateTime>(),
                EnumValue = sut.CreateEnumFixture<ExampleEnum>(),
                StringValue = sut.CreateFixture<string>(),
                IntList = sut.CreateManyFixture<int>(new Random().Next(3, 20))
            };

            var expectedResult = new PostJsonResponse
            {
                Message = $"post-json {request.Serialize()}"
            };

            var exampleHttpService = sut.ServiceProviderAfterMocks.GetRequiredService<IExampleHttpService>();
            exampleHttpService
                .PostJsonAsync(request: Arg.Is<PostJsonRequest>(predicate: x => x.Serialize(null) == request.Serialize(null)), callerMethodName: Arg.Any<string>())
                .Returns(expectedResult);

            //act
            var res = await sut.ExecutePostAsync<PostJsonResponse, PostJsonRequest>(
                urlPath: "/api/example/post-json-through-http",
                request: request);

            //assert
            Assert.Equal(expected: expectedResult.Serialize(), actual: res.Serialize());
        }
    }
}
