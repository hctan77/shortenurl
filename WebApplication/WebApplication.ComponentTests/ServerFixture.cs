namespace WebApplication.ComponentTests
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.PlatformAbstractions;
    using Moq;
    using System;
    using System.IO;
    using System.Net.Http;
    using WebApplication.Configuration;
    using WebApplication.Core;

    /// <summary>
    /// A class that start a web host for WebApplication.
    /// </summary>
    public sealed class ServerFixture : IDisposable
    {
        private readonly TestServer testServer;
        private bool isDisposed;

        public ServerFixture()
        {
            var contentRootPath = GetContentRootPath();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(contentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new WebHostBuilder()
                .UseContentRoot(contentRootPath)
                .UseConfiguration(configuration)
                .UseEnvironment("Development")
                .ConfigureServices(services => services.AddAutofac())
                .ConfigureTestContainer<ContainerBuilder>(
                    containerBuilder =>
                    {
                        SetupTestDependencies(containerBuilder, configuration);
                    })
                .UseStartup<Startup>();

            testServer = new TestServer(builder);
            Client = testServer.CreateClient();
        }

        public HttpClient Client { get; }

        public Mock<IShortenUrlGenerator> MockShortenUrlGenerator { get; } = new Mock<IShortenUrlGenerator>();

        public Mock<IRepository> MockRepository { get; } = new Mock<IRepository>();

        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }

            Client.Dispose();
            testServer.Dispose();
            this.isDisposed = true;
        }

        private void SetupTestDependencies(ContainerBuilder builder, IConfiguration configuration)
        {
            builder.Register(p => configuration.GetSection("Api").Get<ApiOptions>())
                .SingleInstance();

            builder.Register(p => configuration.GetSection("AzureTable").Get<AzureTableStorageOptions>())
                .SingleInstance();

            builder.RegisterInstance(this.MockShortenUrlGenerator.Object)
                .As<IShortenUrlGenerator>()
                .SingleInstance();

            builder.RegisterInstance(this.MockRepository.Object)
                .As<IRepository>()
                .SingleInstance();
        }

        private string GetContentRootPath()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"..\..\..\..\WebApplication";
            return Path.Combine(testProjectPath, relativePathToHostProject);
        }
    }
}