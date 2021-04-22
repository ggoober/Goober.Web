using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using Goober.Core.Services;
using Goober.Http;
using Goober.Tests.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Goober.Tests
{
    public class SystemUnderTest
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public TestServer Server { get; private set; }

        public IFixture Fixture { get; private set; }

        public string SessionKey { get; private set; }

        public List<string> AssemblyNames = new List<string>();

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

        public T CreateFixture<T>()
        {
            return Fixture.Create<T>();
        }

        public ICustomizationComposer<T> BuildFixture<T>()
        {
            return Fixture.Build<T>();
        }

        public void Init<TStartup>(Action<IServiceCollection> configureServices, Action<IServiceCollection> configureTestServices)
            where TStartup : class
        {
            SessionKey = Guid.NewGuid().ToString();

            IServiceProvider serviceProvider = null;
            var server = new TestServer(WebHost.CreateDefaultBuilder()
                    .UseStartup<TStartup>()
                    .ConfigureServices(configureServices)
                    .ConfigureTestServices(services =>
                    {
                        MockRemoteCallServices(services, AssemblyNames);
                        //MockRabbitMqPublisher(services);
                        RemoveHostedServices(services);
                        MockDateTimeService(services);

                        configureTestServices(services);

                        serviceProvider = services.BuildServiceProvider();
                    }));

            ServiceProvider = serviceProvider;
            Server = server;
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

        #region private methods

        private void MockRemoteCallServices(IServiceCollection services, IEnumerable<string> assemblyNames)
        {
            var interfaceTypes = GetRemoteCallServiceSubclasses(assemblyNames);

            CreateMocks(services, Fixture, interfaceTypes);
        }

        private List<Type> GetRemoteCallServiceSubclasses(IEnumerable<string> assemblyNames)
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

        private static void CreateMocks(IServiceCollection services, IFixture fixture, IEnumerable<Type> implementTypes)
        {
            var context = new SpecimenContext(fixture);

            foreach (var iImplementType in implementTypes)
            {
                var interfaceName = "I" + iImplementType.Name;
                var interfaceType = iImplementType.GetInterface(interfaceName);
                if (interfaceType == null)
                    throw new InvalidOperationException($"Can't find interface ({interfaceName}) for class = {iImplementType.Name}");

                var mock = fixture.Create(interfaceType, context);

                services.Add(new ServiceDescriptor(serviceType: interfaceType, instance: mock));
            }
        }

        #endregion
    }
}
