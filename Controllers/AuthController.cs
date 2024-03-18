using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using JwtAuthenticationWebAPI.Models;
using JwtAuthenticationWebAPI.Services;
using JWTAuthenticationWebAPI;
using JWTAuthenticationWebAPI.Dto;
using JWTAuthenticationWebAPI.Filter;
using JWTAuthenticationWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Security.Claims;

namespace JwtAuthenticationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly ITokenService _tokenService;
        private IConfiguration _config;

        public AuthController(UserContext userContext, ITokenService tokenService, IConfiguration config)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _config = config;
        }

        [HttpPost, Route("login")]
        [ValidateModel]
        public IActionResult Login([FromBody] LoginModelDto loginModel)
        {
            //var secretKey = Convert.ToString(_config["jwt:privatekey"]);
            //if (loginModel is null)
            //{
            //    return BadRequest("Invalid client request");
            //}

            //if (Request.Headers.Keys.Contains("SecretKey"))
            //{
            //    Request.Headers.TryGetValue("SecretKey", out StringValues secretValue);                
            //    if(Convert.ToString(secretValue) != null)
            //    {
            //        string decrypytText = Utility.DecryptText(secretValue, secretKey);
            //        if(decrypytText != null && !GetSecretKey(decrypytText).Result) {
            //            return BadRequest("Invalid client request");
            //        }
            //    }
            //}
            //else
            //{
            //    return BadRequest("Invalid client request");
            //}

            //var passwordDecrypt = Utility.DecryptText(loginModel.Password, secretKey);
            var user = _userContext.LoginModels.FirstOrDefault(u =>
                    (u.UserName == loginModel.UserName) && (u.Password == loginModel.Password));
            if (user is null)
                return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.UserName),
                new Claim(ClaimTypes.Role, "Manager"),
                new Claim(ClaimTypes.Role, "Support"),
                new Claim(ClaimTypes.Role, "HelpDesk"),
            };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(Convert.ToInt32(_config["Tokenization:key"]));

            var loginHistory = new LoginHistory()
            {
                UserName = loginModel.UserName,
                IpAddress = GetRemoteIPAddress(),
                Timestamp = DateTime.Now,
                TrackingID = Guid.NewGuid()
            };
            _userContext.LoginHistory.Add(loginHistory);
            _userContext.SaveChanges();

            return Ok(new AuthenticatedResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Claims = claims.Select(x => x.Value).ToList()
            });

            
        }


        //secret value: Azureuser
       private async Task<bool> GetSecretKey(string secretValue) {
            var keyVaultName = _config["jwt:keyvaultName"];
            var secretName = _config["jwt:secretName"]; 
            var kvUri = $"https://{keyVaultName}.vault.azure.net/";

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            KeyVaultSecret secret = await client.GetSecretAsync(secretName);
            if(secret.Value == secretValue)
            {
                return true;
            }
            return false;

        }

        public string GetRemoteIPAddress(bool allowForwarded = true)
        {
            string userIPAddress = string.Empty;
            if (allowForwarded)
            {
                string header = (HttpContext.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault());
                if (IPAddress.TryParse(header, out IPAddress ip))
                {
                   return userIPAddress;
                }
            }
            string hostName = Dns.GetHostName();
            IPHostEntry iPHostEntry = Dns.GetHostEntry(hostName);
            IPAddress[] arrIPAddress = iPHostEntry.AddressList;
            try
            {
                foreach (IPAddress iPAddress in arrIPAddress)
                {
                    if(iPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        userIPAddress = iPAddress.ToString();
                    }
                }
                if (string.IsNullOrEmpty(userIPAddress))
                    userIPAddress = arrIPAddress[arrIPAddress.Length - 1].ToString();
            }
            catch
            {
                try
                {
                    arrIPAddress = Dns.GetHostAddresses(hostName);
                    userIPAddress = arrIPAddress[0].ToString();

                }
                catch
                {
                    userIPAddress = "127.0.0.1";
                }
            }
            return userIPAddress;
        }
    }
}
