using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIREST_GET.Models;
using Newtonsoft.Json;
using System.Net;

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
            int consult = 0;
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
                    int id = character.Id;
                    string name=character.Name;
                    string description = character.Description;
                    string path = "" + character.Thumbnail.Path + "." + character.Thumbnail.Extension;
                    MarvelCharacter marvelPerson = new MarvelCharacter
                    {
                        Id = id,
                        Name = name,
                        Description = description,
                        Thumbnail = path,
                        ConsultNumber = consult + 1
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
