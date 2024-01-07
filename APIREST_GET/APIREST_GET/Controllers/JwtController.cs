using APIREST_GET.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Security.Claims;

namespace APIREST_GET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        public readonly ARQUITECTURAContext _dbcontext;
        private readonly IHttpClientFactory _httpClientFactory;
        public JwtController(ARQUITECTURAContext _context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _dbcontext = _context;
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public static dynamic validateToken(ClaimsIdentity identity, ARQUITECTURAContext _dbcontext)
        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "You must a valid token",
                        result = ""
                    };
                }
                var ide = identity.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                var findUser = _dbcontext.Users.Find(Guid.Parse(ide));
                return new
                {
                    success = true,
                    message = "Success",
                    result = findUser
                };
            }
            catch (Exception e)
            {
                return new
                {
                    success = false,
                    message = "Catch: " + e.Message,
                    result = ""
                };
            }
        }
    }
}
