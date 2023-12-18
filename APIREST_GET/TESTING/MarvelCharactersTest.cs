using APIREST_GET.Controllers;
using APIREST_GET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace TESTING
{
    public class MarvelCharactersTest
    {
        private readonly MarvelCharactersController _controller;

        public MarvelCharactersTest()
        {
            var options = new DbContextOptionsBuilder<ARQUITECTURAContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryTestDatabase")
                .Options;

            var dbContext = new ARQUITECTURAContext(options);
            var httpClientFactory = new Mock<IHttpClientFactory>();
            _controller = new MarvelCharactersController(dbContext, httpClientFactory.Object);
        }
        //En esta prueba se está verificando si el resultado de la acción getAllCharacters es del tipo List<MarvelCharacter>. Si se recibe False en esta aserción, significa que el tipo real del resultado no es una lista de personajes.
        [Fact]
        public async Task Get_Ok()
        {
            var result = await _controller.getAllCharacters();

            Assert.IsType<List<MarvelCharacter>>(result);
        }

    }
}