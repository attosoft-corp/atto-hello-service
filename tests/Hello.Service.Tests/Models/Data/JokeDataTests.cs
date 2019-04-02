using FluentAssertions;
using Hello.Service.Models.Data;
using System.Collections.Generic;
using Xunit;

namespace Hello.Service.Tests.Models.Data
{
    public class JokeDataTests
    {

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var data = new JokeData
            {
                categories = new List<string> { "teste1", "teste2" },
                Id = 1,
                Joke = "My Joke"
            };

            // Assert
            data.categories.Should().NotBeEmpty();
            data.Id.Should().Be(1);
            data.Joke.Should().Be("My Joke");
            

        }
    }
}