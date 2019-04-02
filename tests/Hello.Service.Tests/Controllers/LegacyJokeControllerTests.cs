using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Hello.Service.Controllers;
using Hello.Service.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hello.Service.Tests.Controllers
{
    public class LegacyJokeControllerTests
    {
        private readonly IFixture _fixture;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LegacyJokeController> _logger;
        private readonly HttpClient _httpClient;

        public LegacyJokeControllerTests()
        {
            _fixture = new Fixture();
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            _logger = Substitute.For<ILogger<LegacyJokeController>>();
        }

        [Fact]
        public void GetJokeAsync_WhenBadRequest_ThrowException()
        {
            // Arrange
            var jokeContent = JsonConvert.SerializeObject(_fixture.Create<JokeResponse>());
            var statusCode = HttpStatusCode.BadRequest;
            var handler = new HttpMessageHandlerTest(statusCode, jokeContent);
            var httpClient = new HttpClient(handler);

            _httpClientFactory.CreateClient(Arg.Any<string>()).Returns(httpClient);

            var controller = new LegacyJokeController(_httpClientFactory, _logger);

            // Act
            Action action = () => controller.GetJokeAsync().GetAwaiter().GetResult();

            action.Should().ThrowExactly<Exception>();
        }

        [Fact]
        public async Task GetJokeAsync_WhenSucessRequest_ReturnJoke()
        {
            // Arrange
            var jokeContent = JsonConvert.SerializeObject(_fixture.Create<JokeResponse>());
            var statusCode = HttpStatusCode.OK;
            var handler = new HttpMessageHandlerTest(statusCode, jokeContent);
            var httpClient = new HttpClient(handler);

            _httpClientFactory.CreateClient(Arg.Any<string>()).Returns(httpClient);

            var controller = new LegacyJokeController(_httpClientFactory, _logger);

            // Act
            var result = await controller.GetJokeAsync();

            result.Should().NotBeNull();
        }
    }

    internal class HttpMessageHandlerTest : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly IFixture _fixture;
        private readonly string _content;

        public HttpMessageHandlerTest(HttpStatusCode statusCode, string content = default)
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new TypeRelay(typeof(HttpContent), typeof(StringContent)));
            _statusCode = statusCode;
            _content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = _fixture.Create<HttpResponseMessage>();
            result.Content = new StringContent(_content);
            result.StatusCode = _statusCode;
            return Task.FromResult(result);
        }
    }
}