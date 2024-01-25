using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class AuthenticationRequestBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }

        }

        public class CityInfoUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }

            public CityInfoUser(int userId, string userName, string firstName, string lastName, string city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            //1. Validamos el username/password
            CityInfoUser user =  ValidateUserCredentials(authenticationRequestBody.UserName, authenticationRequestBody.Password);
            if(user == null) return Unauthorized();

            //2. Creamos el  token:

            string secretKey = _configuration["Authentication:SecretForKey"] ?? "thisisthesecretforgeneratingakey(mustbeatleast32bitlong)";
            string issuer = _configuration["Authentication:Issuer"] ?? "https://localhost:7102";
            string audience = _configuration["Authentication:Audience"] ?? "cityInfoApi"; 

            var securitykey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("city", user.City));

            var jwtSecurityToken = new JwtSecurityToken(
                issuer,
                audience, 
                claimsForToken, 
                DateTime.UtcNow, 
                DateTime.UtcNow.AddHours(1), 
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private CityInfoUser ValidateUserCredentials(string? userName, string? password)
        {
            //En este metodo se debe validar si las credenciales que llegan coinciden con las guardadas en la base de datos de los usuarios.
            //Por defecto retornaremos un standar
            return new CityInfoUser(1, userName ?? "", "Fer", "Gutierrez", "Rafaela");
        }
    }
}
