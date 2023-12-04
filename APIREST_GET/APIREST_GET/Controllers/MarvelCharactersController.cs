using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIREST_GET.Models;
using Newtonsoft.Json;
using System.Net;
using Microsoft.Data.SqlClient;

namespace APIREST_GET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarvelCharactersController : ControllerBase
    {
        public readonly ARQUITECTURAContext _dbcontext;
        public MarvelCharactersController(ARQUITECTURAContext _context, IHttpClientFactory httpClientFactory)
        {
            _dbcontext = _context;
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        private readonly IHttpClientFactory _httpClientFactory;

        [HttpGet]
        [Route("getMarvelCharacters")]
        public async Task<IActionResult> getMarvelCharacters()
        {
            string publicKey = "474492f3425dddb8557bd7b9ed068eb2";
            string privateKey = "d1155d2ccda761566628047b49dedb853cf583a8";
            string ts = "1";
            string finalKey = ts + privateKey + publicKey;
            HashCreator newHash= new HashCreator();
            string hash = newHash.CalculateMD5Hash(finalKey);
            try
            {
                string apiUrl = "https://gateway.marvel.com/v1/public/characters?limit=4&ts=" + ts + "&apikey=" + publicKey + "&hash=" + hash;
                var httpClient = _httpClientFactory.CreateClient();
                var externalApiResponse = await httpClient.GetStringAsync(apiUrl);
                var marvelApiResponse = JsonConvert.DeserializeObject<MarvelCharacterViewModel.MarvelApiResponse>(externalApiResponse);
                var marvelCharacters = marvelApiResponse?.Data?.Results;
                if (marvelCharacters == null) 
                {
                    throw new Exception("No existen datos que procesar");
                }
                foreach (var character in marvelCharacters)
                {
                    MarvelCharacter marvelPerson = new MarvelCharacter
                    {
                        Id = character.Id,
                        Name = character.Name,
                        Description = character.Description,
                        Thumbnail = "" + character.Thumbnail.Path + "." + character.Thumbnail.Extension
                    };
                    var addInventry = _dbcontext.MarvelCharacters.Add(marvelPerson);
                }
                await _dbcontext.SaveChangesAsync();
                var storedMarvelCharacters = await _dbcontext.MarvelCharacters.ToListAsync();
                return Ok(storedMarvelCharacters);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }

        }
    }
}
