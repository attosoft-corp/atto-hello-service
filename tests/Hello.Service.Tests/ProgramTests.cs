using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Xunit;

namespace Hello.Service.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void Main_WhenNotBasicConfigurationSet_ThowException()
        {
            Action action = () =>
            {
                var webHost = WebHost.CreateDefaultBuilder().UseEnvironment("Development").UseStartup<Startup>();

                using (var server = new TestServer(webHost))
                using (var client = server.CreateClient())
                {
                }
            };

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Main_BuildWebHostWhenBasicConfigurationIsSet_Pass()
        {
            Action action = () =>
            {
                var param = new Dictionary<string, string>
                {
                    { "spring:application:name", "ApplicationName" },
                    { "spring:cloud:config:enable", "false" },
                    { "eureka:client:shouldRegisterWithEureka", "false" },
                    { "eureka:client:shouldFetchRegistry", "false" }
                };
                var configuration = new ConfigurationBuilder().AddInMemoryCollection(param).Build();
                var webHost = WebHost.CreateDefaultBuilder()
                .UseConfiguration(configuration)
                .UseEnvironment("Development")
                .UseStartup<Startup>();

                using (var server = new TestServer(webHost))
                using (var client = server.CreateClient())
                {
                }
            };

            action.Should().NotThrow<Exception>();
        }
    }
}