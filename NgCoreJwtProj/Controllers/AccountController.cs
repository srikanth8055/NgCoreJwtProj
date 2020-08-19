using DataLayer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.JsonPatch.Helpers;
using Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Xml;

namespace NgCoreJwtProj.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration configuration;
        private IUserObj _iuserobj;
        public AccountController(IUserObj userobj, IConfiguration configuration)
        {
            _iuserobj = userobj;
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("getUser")]
        public IActionResult getUser()
        {
            return Ok("User1");
        }

        [HttpPost("ValidateUser")]
        public async Task<IActionResult> ValidateUser([FromBody]User user)
        {
            try
            {
                var constr = configuration["ConnectionStrings:ConnectionString"];

                UserData userdata = new UserData();
                var data = userdata.GetUser(user);
                if (data != null)
                {
                    var claims = new[] {
                            new Claim("Name", data.name),
                            new Claim(ClaimTypes.Role, data.role),

                    };

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var tokeOptions = new JwtSecurityToken(
                     issuer: configuration["AppSettings:Issuer"],
                     audience: configuration["AppSettings:Audience"],
                     claims: claims,
                     expires: DateTime.Now.AddHours(5),
                     signingCredentials: signinCredentials
                     );

                    var token = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    return  Ok(new { TempPwdUserId = data.id, Token = token });
                }
                else
                {
                    return BadRequest("Invalid client request");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost("ValidateUserDapper")]
        public async Task<IActionResult> ValidateUserDapper([FromBody]User user)
        {
            try
            {
                var data = _iuserobj.ValidateUserDapper(user);
                if (data != null)
                {
                    var claims = new[] {
                            new Claim(ClaimTypes.Name, data.name),
                            new Claim(ClaimTypes.Role, data.role)
                    };
                    var token = GenerateToken(claims);
                    string refreshToken = GenerateRefreshToken();
                    SaveRefreshToken(user.name, refreshToken);
                    return Ok(new { TempPwdUserId = data.id, Token = token.token, RefreshToken = refreshToken, expirationTokenTime = token.tokeOption });
                }
                else
                {
                    return BadRequest("Invalid client request");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
       
        private dynamic GenerateToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
             issuer: configuration["AppSettings:Issuer"],
             audience: configuration["AppSettings:Audience"],
             claims: claims,
             expires: DateTime.Now.AddHours(5),
             signingCredentials: signinCredentials
             );


            var token = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return new {token=token, tokeOption=tokeOptions.ValidTo };
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
     
        private void SaveRefreshToken(string username, string newRefreshToken)
        {
            //XmlTextReader textReader = new XmlTextReader("./wwwroot/secure/token.xml");
            //textReader.Read();
            // If the node has value  
            using (XmlWriter writer = XmlWriter.Create("./wwwroot/secure/token.xml"))
            {
                writer.WriteStartElement("tokens");
                writer.WriteElementString(username, newRefreshToken);
                writer.WriteEndElement();
                writer.Flush();
            }
            configuration.Bind($"RefreshToken:{username}", newRefreshToken);
        }

        private string GetRefreshToken(string username)
        {
            string token="";
            using (XmlReader reader = XmlReader.Create("./wwwroot/secure/token.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        //return only when you have START tag  
                        if (reader.Name.ToString()==username)
                        {
                              token= reader.ReadString();
                        }
                    }
                }
            }
            return token;
        }

        [HttpPost("RefreshToken")]
        public IActionResult Refresh([FromBody] dynamic objData)
        {
            var principal = GetPrincipalFromExpiredToken(Convert.ToString(objData.token));
            var username = principal.Identity.Name;
            var savedRefreshToken = GetRefreshToken(username); //retrieve the refresh token from a data store
            if (savedRefreshToken != Convert.ToString(objData.refreshToken))
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken = GenerateToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();
            SaveRefreshToken(username, newRefreshToken);

            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidIssuer = configuration["AppSettings:Issuer"],
                ValidAudience = configuration["AppSettings:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}