using FluentAssertions;
using Hello.Service.Models.Data;
using Hello.Service.Models.Response;
using NSubstitute;
using System;
using Xunit;

namespace Hello.Service.Tests.Models.Response
{
    public class JokeResponseTests
    {
        [Fact]
        public void TestMethod1()
        {
            var data = new JokeResponse
            {
                Value = new JokeData()
            };
            data.Value.Should().NotBeNull();
        }
    }
}
