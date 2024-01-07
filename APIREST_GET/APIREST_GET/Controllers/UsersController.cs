using APIREST_GET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIREST_GET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly ARQUITECTURAContext _dbcontext;
        public IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public UsersController(ARQUITECTURAContext _context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _dbcontext = _context;
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configuration = configuration;
        }


        [HttpPost]
        [Route("CreateUpdateUser")]
        public dynamic InsertUser([FromBody] User user)
        {
            
        #pragma warning disable CS8604
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var responseToken = JwtController.validateToken(identity, _dbcontext);
            if (!responseToken.success) return responseToken;
            User tokenUser = responseToken.result;
            if (tokenUser.Rol != "ADM")
            {
                throw new Exception("You do not have the required permissions");
            }
            try
            {
                
                var existingUser = _dbcontext.Users.Find(user.IdeUser);
                if (existingUser == null)
                {
                    HashCreator hash = new HashCreator();
                    user.Password = hash.CalculateSHA256Hash(user.Password);
                    _dbcontext.Users.Add(user);
                }
                else 
                {
                    existingUser.UserName = user.UserName;
                    existingUser.Name = user.Name;
                    existingUser.LastName = user.LastName;
                    existingUser.Rol = user.Rol;
                    
                }
                _dbcontext.SaveChanges();
                return Ok(new { success = true, message = "User added successfully.", data= user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("Login")]
        public dynamic Login([FromBody] Object optData)
        {
            try 
            {
                var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
                if(data==null)
                {
                    throw new Exception("The value is null");
                }
                string user = data.user.ToString();
                string password = data.password.ToString();
                HashCreator hash = new HashCreator();
                string passwordHashed= hash.CalculateSHA256Hash(password);
                var users = _dbcontext.Users.Where(x => x.UserName == user && x.Password == passwordHashed).FirstOrDefault();
                if (users == null)
                {
                    return new
                    {
                        success = false,
                        message = "Incorrect credentials",
                        result = ""
                    };
                }

                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", users.IdeUser.ToString()),
                    new Claim("Name", users.Name),
                    new Claim("Rol", users.Rol),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddMinutes(3),
                        signingCredentials: singIn
                    );
                return new
                {
                    success = true,
                    message = "successfully",
                    result = new JwtSecurityTokenHandler().WriteToken(token)
                };

            } catch (Exception e) 
            {
                return new
                {
                    error = e
                };
            }
            
        }
        #pragma warning restore CS8604
    }

}
