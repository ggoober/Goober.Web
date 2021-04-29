using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using Goober.Core.Services;
using Goober.Http;
using Goober.Http.Services;
using Goober.Http.Utils;
using Goober.Tests.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Goober.Tests
{
    public class SystemUnderTest
    {
        #region fields

        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter(
                    namingStrategy: new CamelCaseNamingStrategy(processDictionaryKeys: true, overrideSpecifiedNames: false, processExtensionDataNames: true),
                    allowIntegerValues: true)
            },
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Culture = System.Globalization.CultureInfo.InvariantCulture,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTime
        };

        private const string ApplicationJsonContentTypeValue = "application/json";

        #endregion

        #region public properties

        public IServiceProvider ServiceProvider { get; private set; }

        public IServiceProvider ServiceProviderBeforeMocks { get; private set; }

        public TestServer Server { get; private set; }
        public HttpClient HttpClient { get; private set; }
        public IFixture Fixture { get; private set; }

        public string SessionKey { get; private set; }

        public List<string> AssemblyNames = new List<string>();

        #endregion

        #region ctor

        public SystemUnderTest(List<string> assemblyNames)
        {
            Fixture = new Fixture().AutoMock();

            AssemblyNames = assemblyNames.ToList();
        }

        public void Init<TStartup>()
            where TStartup : class
        {
            Init<TStartup>(configureServices: (services) => { }, configureTestServices: (services) => { });
        }

        public void Init<TStartup>(Action<IServiceCollection> configureServices)
            where TStartup : class
        {
            Init<TStartup>(configureServices: configureServices, configureTestServices: (services) => { });
        }

        public TService GetRequiredService<TService>()
        {
            return ServiceProvider.GetRequiredService<TService>();
        }

        public void Init<TStartup>(Action<IServiceCollection> configureServices, Action<IServiceCollection> configureTestServices)
            where TStartup : class
        {
            SessionKey = Guid.NewGuid().ToString();

            IServiceProvider localServiceProvider = null;

            var server = new TestServer(WebHost.CreateDefaultBuilder()
                    .UseStartup<TStartup>()
                    .ConfigureServices(configureServices)
                    .ConfigureTestServices(services =>
                    {
                        services.TryAddScoped<IHttpContextAccessor, HttpContextAccessor>();

                        RemoveServicesFromServiceCollection(services, typeof(IHttpClientFactory));

                        MockLogger(services);

                        MockHttpJsonHelperService(services, Fixture);

                        MockBaseHttpServices(services, AssemblyNames);

                        //MockRabbitMqPublisher(services);

                        RemoveHostedServices(services);

                        MockDateTimeService(services);

                        configureTestServices(services);

                        ServiceProvider = services.BuildServiceProvider();
                    }));

            Server = server;

            HttpClient = server.CreateClient();
        }

        public void RegisterInMemoryDatabase<TDbContext>(IServiceCollection services) where TDbContext : DbContext
        {
            var sp = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var dbName = typeof(TDbContext).FullName + "-" + SessionKey;

            services.AddDbContext<TDbContext>(options =>
            {
                options.ConfigureWarnings((wrn) =>
                {
                    wrn.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                });
                options.UseInMemoryDatabase(dbName);
                options.UseInternalServiceProvider(sp);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }


        #endregion

        #region fixture

        private HashSet<int> CreatedIds = new HashSet<int>();

        public int CreateUniqueIntIdFixture()
        {
            var newId = Fixture.Create<int>();
            const int maxIterations = 100;
            int currentIteration = 0;
            while (true)
            {
                if (CreatedIds.Contains(newId) == false)
                {
                    CreatedIds.Add(newId);
                    return newId;
                }

                currentIteration++;
                if (currentIteration >= maxIterations)
                {
                    throw new InvalidOperationException($"currentIteration >= {maxIterations}");
                }
            }
        }

        public T CreateFixture<T>()
        {
            return Fixture.Create<T>();
        }

        public List<T> CreateManyFixture<T>(int count = 3)
        {
            return Fixture.CreateMany<T>(count).ToList();
        }

        public TEnum CreateEnumFixture<TEnum>() where TEnum : Enum
        {
            var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
            if (enumValues.Any() == false)
            {
                throw new InvalidOperationException($"{typeof(TEnum).Name} is empty");
            }

            var random = new Random();
            var randomIndex = random.Next(0, enumValues.Count - 1);

            return enumValues[randomIndex];
        }

        public ICustomizationComposer<T> BuildFixture<T>()
        {
            return Fixture.Build<T>();
        }

        #endregion

        #region testserver http methods

        public async Task<TResponse> ExecutePostAsync<TResponse, TRequest>(
            string urlPath,
            TRequest request,
            long maxResponseContentLength = 300*1024)
        {
            var strRet = await ExecutePostReturnStringAsync<TRequest>(urlPath, request, maxResponseContentLength);

            if (string.IsNullOrEmpty(strRet) == true)
                return default;

            return Deserialize<TResponse>(strRet, _jsonSerializerSettings);
        }

        public async Task<TResponse> ExecutePostFormDataAsync<TResponse>(
            string urlPath,
            List<KeyValuePair<string, string>> formData,
            long maxResponseContentLength = 300 * 1024)
        {
            var strRet = await ExecutePostFormDataReturnStringAsync(urlPath: urlPath,
                formData: formData,
                maxResponseContentLength: maxResponseContentLength);

            if (string.IsNullOrEmpty(strRet) == true)
                return default;

            return Deserialize<TResponse>(strRet, _jsonSerializerSettings);
        }

        public async Task<string> ExecutePostFormDataReturnStringAsync(
            string urlPath,
            List<KeyValuePair<string, string>> formData,
            long maxResponseContentLength = 300 * 1024)
        {
            var content = new FormUrlEncodedContent(formData);

            var httpResponse = await HttpClient.PostAsync(requestUri: urlPath, content: content);

            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            var ret = await GetResponseStringAndProcessResponseStatusCodeAsync(
                httpResponse: httpResponse,
                maxResponseContentLength: maxResponseContentLength);

            return ret;
        }

        public async Task<string> ExecutePostReturnStringAsync<TRequest>(
            string urlPath,
            TRequest request,
            long maxResponseContentLength = 300*1024)
        {
            var strJsonContent = Serialize(request, _jsonSerializerSettings);

            var content = new StringContent(content: strJsonContent, Encoding.UTF8, mediaType: ApplicationJsonContentTypeValue);

            var httpResponse = await HttpClient.PostAsync(requestUri: urlPath, content: content);

            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            var ret = await GetResponseStringAndProcessResponseStatusCodeAsync(
                httpResponse: httpResponse,
                maxResponseContentLength: maxResponseContentLength);

            return ret;
        }

        public async Task<TResponse> ExecuteGetAsync<TResponse>(string urlPath,
            List<KeyValuePair<string, string>> queryParameters,
            long maxResponseContentLength = 300*1024)
        {
            var strRes = await ExecuteGetStringAsync(urlPath, queryParameters, maxResponseContentLength);

            if (string.IsNullOrEmpty(strRes) == true)
                return default;

            return Deserialize<TResponse>(strRes, _jsonSerializerSettings);
        }

        public async Task<string> ExecuteGetStringAsync(
            string urlPath, 
            List<KeyValuePair<string, string>> queryParameters, 
            long maxResponseContentLength = 300 * 1024)
        {
            var urlPathWithQueryParameters = HttpUtils.BuildUrlWithQueryParameters(urlWithoutQueryParameters: urlPath, queryParameters: queryParameters);

            var httpResponse = await HttpClient.GetAsync(urlPathWithQueryParameters);

            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            var ret = await GetResponseStringAndProcessResponseStatusCodeAsync(
                    httpResponse: httpResponse,
                    maxResponseContentLength: maxResponseContentLength);

            return ret;
        }

        #endregion
        
        #region private mocks and setups

        private void MockLogger(IServiceCollection services)
        {
            RemoveServicesFromServiceCollection(services, typeof(ILoggerFactory));
            services.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory>();
        }

        private void MockDateTimeService(IServiceCollection services)
        {
            var dateTimeServices = services.Where(x => x.ServiceType == typeof(IDateTimeService)).ToList();
            foreach (var iDateTimeService in dateTimeServices)
            {
                services.Remove(iDateTimeService);
            }

            services.Add(new ServiceDescriptor(serviceType: typeof(IDateTimeService), instance: Fixture.Create<IDateTimeService>()));
        }

        private static void RemoveHostedServices(IServiceCollection services)
        {
            var hostedServices = services.Where(descriptor => descriptor.ServiceType == typeof(IHostedService)).ToList();
            foreach (var iHostedService in hostedServices)
            {
                services.Remove(iHostedService);
            }
        }

        //private void MockRabbitMqPublisher(IServiceCollection services)
        //{
        //    var rabbitMqMessageProducerMock = Fixture.Create<IRabbitMqMessageProducer>();
        //    services.AddSingleton(rabbitMqMessageProducerMock);
        //}

        private void MockBaseHttpServices(IServiceCollection services, IEnumerable<string> assemblyNames)
        {
            var implementTypes = GetBaseHttpServiceImplementTypes(assemblyNames);

            var context = new SpecimenContext(Fixture);

            foreach (var iImplementType in implementTypes)
            {
                var interfaceName = "I" + iImplementType.Name;
                var interfaceType = iImplementType.GetInterface(interfaceName);
                if (interfaceType == null)
                    throw new InvalidOperationException($"Can't find interface ({interfaceName}) for class = {iImplementType.Name}");

                services = RemoveServicesFromServiceCollection(services, interfaceType);

                var mock = Fixture.Create(interfaceType, context);
                services.Add(new ServiceDescriptor(serviceType: interfaceType, instance: mock));
            }
        }

        private List<Type> GetBaseHttpServiceImplementTypes(IEnumerable<string> assemblyNames)
        {
            var ret = new List<Type>();

            var assemblies = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => assemblyNames.Any(z => x.GetName().Name == z));

            foreach (var iAssembly in assemblies)
            {
                var remoteCallServices = iAssembly.GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BaseHttpService))).ToList();

                foreach (var implementType in remoteCallServices)
                {
                    ret.Add(implementType);
                }
            }

            return ret;
        }

        private static void MockHttpJsonHelperService(IServiceCollection services,
            IFixture fixture)
        {
            var context = new SpecimenContext(fixture);

            var httpJsonHelperServiceType = typeof(IHttpJsonHelperService);
            services = RemoveServicesFromServiceCollection(services, httpJsonHelperServiceType);

            var mock = fixture.Create(httpJsonHelperServiceType, context);
            services.Add(new ServiceDescriptor(serviceType: httpJsonHelperServiceType, instance: mock));
        }

        private static IServiceCollection RemoveServicesFromServiceCollection(IServiceCollection services, Type interfaceType)
        {
            var servicesToRemove = services.Where(x => x.ServiceType == interfaceType).ToList();
            foreach (var iServiceToRemove in servicesToRemove)
            {
                services.Remove(iServiceToRemove);
            }

            return services;
        }

        #endregion

        #region private http methods

        private async Task<string> GetResponseStringAndProcessResponseStatusCodeAsync(HttpResponseMessage httpResponse,
            long maxResponseContentLength)
        {
            var responseStringResult = await ReadContentWithMaxSizeRetrictionAsync(httpResponse.Content,
                encoding: Encoding.UTF8,
                maxSize: maxResponseContentLength);

            if (httpResponse.StatusCode == HttpStatusCode.OK
                || httpResponse.StatusCode == HttpStatusCode.Accepted
                || httpResponse.StatusCode == HttpStatusCode.Created)
            {
                if (responseStringResult.IsReadToTheEnd == true)
                {
                    return responseStringResult.StringResult.ToString();
                }

                throw new WebException($"Response content length is grater then {maxResponseContentLength}");
            }

            var exception = new WebException($"Request fault with statusCode = {httpResponse.StatusCode}, response: {responseStringResult.StringResult}");

            if (responseStringResult.IsReadToTheEnd == false)
            {
                responseStringResult.StringResult.AppendLine();
                responseStringResult.StringResult.Append($"<<< NOT END, response size is greter than {maxResponseContentLength}");
            }

            throw exception;
        }

        private async Task<(bool IsReadToTheEnd, StringBuilder StringResult)> ReadContentWithMaxSizeRetrictionAsync(HttpContent httpContent,
                    Encoding encoding,
                    long maxSize,
                    int bufferSize = 1024)
        {
            using (var stream = await httpContent.ReadAsStreamAsync())
            {
                if (stream.CanRead == false)
                {
                    throw new InvalidOperationException("stream is not ready to ready");
                }

                var totalBytesRead = 0;

                var sbResult = new StringBuilder();

                byte[] buffer = new byte[bufferSize];
                var bytesRead = await stream.ReadAsync(buffer, 0, bufferSize);

                while (bytesRead > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead > maxSize)
                    {
                        return (false, sbResult);
                    }

                    sbResult.Append(encoding.GetString(bytes: buffer, index: 0, count: bytesRead));

                    bytesRead = await stream.ReadAsync(buffer, 0, bufferSize);
                }

                return (true, sbResult);
            }
        }

        private TTarget Deserialize<TTarget>(string value,
            JsonSerializerSettings jsonSerializerSettings)
        {
            try
            {
                return JsonConvert.DeserializeObject<TTarget>(value, jsonSerializerSettings ?? _jsonSerializerSettings);
            }
            catch (Exception exc)
            {
                throw new WebException(
                    message: $"Can't deserialize to type = \"{typeof(TTarget).Name}\", message = \"{exc.Message}\" from value = \"{value}\"",
                    innerException: exc);
            }
        }

        private string Serialize(object value, JsonSerializerSettings serializerSettings)
        {
            return JsonConvert.SerializeObject(value, serializerSettings);
        }

        #endregion
    }
}
