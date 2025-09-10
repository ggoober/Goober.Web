using Goober.DependencyInjection.Extensions;
using Goober.Web.LoggingMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Goober.Web.Models;

namespace Goober.WebApi.Example
{
    public class Startup : Goober.Web.BaseStartup
    {
        public Startup() 
            :
            base(
                swaggerSettings: new BaseStartupSwaggerSettings()
                {
                    XmlCommentsFileNameList = new List<string>()
                    {
                        "WebApi.Example.xml"
                    }
                })
        {
           
        }
        
		class LoggingRequestBodyDelegate2 : LoggingRequestBodyAbstractDelegate
		{
			protected override string ParseBody(byte[] requestBodyBytes, HttpContext httpContext)
			{
				return "delegate: " + Encoding.UTF8.GetString(requestBodyBytes);
			}
		}


        protected override void ConfigurePipelineAfterRouting(IApplicationBuilder app)
        {
        }

        protected override void ConfigurePipelineAfterMvc(IApplicationBuilder app)
        {
        }

        protected override void ConfigureServiceCollections(IServiceCollection services)
        {
            services.RegisterAssemblyClasses<Startup>();
            services.RegisterAssemblyClasses<Goober.WebApi.Example.Api.Models.GetResponse>();

			services.AddTransient<LoggingRequestBodyAbstractDelegate, LoggingRequestBodyDelegate2>();
        }

        protected override void MapControllerRoutes(IEndpointRouteBuilder endpoints)
        {
        }

        protected override void ConfigurePipelineBeforeRouting(IApplicationBuilder app)
        {
        }

        protected override void ConfigureBeforeBaseService(IServiceCollection services)
        {
            base.ConfigureBeforeBaseService(services);
            CustomBaseServiceConfiguration();
        }

        private void CustomBaseServiceConfiguration()
        {
            var productGuid = Guid.Empty;
            var entryPointAssamblyName = System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Name;

            var chunkCount = 3;
            var chunkKeySize = entryPointAssamblyName.Length / chunkCount;
            var tempChunkKeys = new string[chunkCount];
            for (int i = 0; i < chunkCount; i++)
            {
                var startIndex = chunkKeySize * i;
                var length = i == 2
                    ? entryPointAssamblyName.Length - startIndex
                    : chunkKeySize;

                tempChunkKeys[i] = entryPointAssamblyName.Substring(startIndex, length);
            }

            var keys = new string[tempChunkKeys.Length];
            var count = tempChunkKeys.Length - 1;
            for (int i = 0; i <= count; i++)
            {
                var keyString = tempChunkKeys[i];
                for (int j = count; j >= 0; j--)
                {
                    if (j == i)
                        continue;

                    keyString += tempChunkKeys[j];
                }

                var sha256 = SHA256.Create();
                byte[] keyBytes = Encoding.UTF8.GetBytes(keyString);
                byte[] keyHash = sha256.ComputeHash(keyBytes);
                string key = Convert.ToBase64String(keyHash);
                keys[i] = key;
            }

            var privateKey = "<RSAKeyValue><Modulus>s8rAINdIqqoG22uI7xcqP4TvS/qaKfwp6HVGFZhxaFy/ZI2nvjoQCsLL6eqGPRgOUuaOiq5tmx36lPNzMSjykddo2dCkzOGFGww0Gp73MPIwX5APDf5CBrXXe6oY9RmY6JfIKCsARqxhs91OOyXJ5b1qLpaRHeteGgp8dWTiiPE=</Modulus><Exponent>AQAB</Exponent><P>5oQsJzxz0r283WyKM4BgEB7DDe4IVMUoVvqBtRSce5S4MTEjD9h7MEJW/4XUd/USskq1DPzmoAGtl9yB8+54Aw==</P><Q>x6sIWaQWNsGXeRSjtq0itsuSsbQ0y7ao6BkFBniFimO+mNUYCQUDJC6kMwu6SvVGYObEqXD4cYJzCF8u/nZK+w==</Q><DP>sLjVrMLgvMiveUWT8gXSH6mluhEpm+uGkJ/+Ppftm6SZTUUxbx1027uQPNcZ21ksGznA8ZMVL8f5kLoBesPwhw==</DP><DQ>tL1BZcN+yDodn2DrgSTgU4+bTnjNgcnqw45CWkUmvMrlcPsUMrXyzgHaaMqo68ly16yNQMQeYMGw11cx9u2lmQ==</DQ><InverseQ>gLIWo8WPqw8ufuDhAy0csC01O9qtDQlgcQvzISp+niAx+jP1sc9khrPHfoqakXbq159BYkHazlwqd/bfoC6MrQ==</InverseQ><D>E0fG2r67eDHLknQxtxIQ4cbrsoYpSh9Ujy0kWuSCJOLtU2cteeTYG49QNsGDq8GMp6850f1qm5vXZ2nEDJ2LhZImMGghb9mipPCPQ3sPtbHGTP8g7DUcMFeQJBAAxp3wMsQR685qFHn5wpPsr7/aoUcP1bXDkdXi3jBaybZNbpE=</D></RSAKeyValue>";
            byte[] data;
            byte[] signature;
            using (var rsa2 = new RSACryptoServiceProvider(1024))
            {
                rsa2.FromXmlString(privateKey);
                var sha512 = new SHA512Managed();
                data = Encoding.UTF8.GetBytes(productGuid.ToString().ToLower());
                byte[] hash = sha512.ComputeHash(data);
                signature = rsa2.SignData(data, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            }

            var signatureString = Convert.ToBase64String(signature);
            var value = signatureString;

            var values = new string[chunkCount];
            var chunkValueSize = value.Length / chunkCount;
            for (int i = 0; i < chunkCount; i++)
            {
                var startIndex = chunkValueSize * i;
                var length = i == 2
                    ? value.Length - startIndex
                    : chunkValueSize;

                values[i] = value.Substring(startIndex, length);
            }

            for (int i = 0; i < chunkCount; i++)
            {
                ApplicationBaseConfiguration.TryAdd(keys[i], values[i]);
            }
        }
    }
}
