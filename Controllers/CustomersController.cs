using JwtAuthenticationWebAPI.Models;
using JWTAuthenticationWebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace JJwtAuthenticationWebAPI.Controllers
{
    [ServiceFilter(typeof(LogFilter))]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly UserContext _userContext;
        private ILogger<CustomersController> _logger;
        public CustomersController(UserContext userContext, ILogger<CustomersController> logger)
        {
            _userContext = userContext;
            _logger = logger;

        }
        [HttpGet, Authorize(Roles = "Manager, Support")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Kishan Prajapati", "Michelle Chen" };
        }

        [HttpGet, Authorize(Roles = "Support")]
        [Route("Support")]
        public IEnumerable<string> GetSupport()
        {
            return new string[] { "Kishan Prajapati", "Michelle Chen", "Support" };
        }

        [HttpGet, Authorize(Roles = "HelpDesk")]
        [Route("HelpDesk")]
        public IEnumerable<string> GetHelpDesk()
        {
            return new string[] { "Kishan Prajapati", "Michelle Chen", "HelpDesk" };
        }

        [HttpGet, Authorize(Roles = "Manager")]
        [Route("test")]
        public IEnumerable<string> GetTest()
        {
            _logger.LogCritical("nlog is working from a controller");
            throw new ArgumentException("way wrong");
            //return new string[] { "Test" };
        }

        [HttpGet, Authorize(Roles = "Manager")]
        [Route("loginHistory")]
        public IActionResult GetLoginHistory(string userName)
        {
            var user = _userContext.LoginModels.FirstOrDefault(u =>
               (u.UserName == userName));

            if (user is null)
                return Unauthorized();

            var loginHistory = _userContext.LoginHistory.Where(s=>s.UserName == userName).ToList();
            return Ok(loginHistory);
        }

        [HttpGet, Authorize(Roles = "Manager")]
        [Route("GetDeviceInfo")]
        public IActionResult GetDeviceInfo()
        {
            var re = Request;
            
            var headers = re.Headers;
            ContextualInfo contextualInfo = new ContextualInfo();

            if (headers.Keys.Contains("UserId"))
            {
                headers.TryGetValue("UserId", out StringValues userValue);
                contextualInfo.UserId = userValue;  
            }

            if (headers.Keys.Contains("SendingAppName"))
            {
                headers.TryGetValue("SendingAppName", out StringValues SendingAppNameValue);
                contextualInfo.SendingAppName = SendingAppNameValue;               
            }

            if (headers.Keys.Contains("TrackingId"))
            {
                headers.TryGetValue("Trackingid", out StringValues TrackingidValue);
                contextualInfo.TrackingId = Guid.Parse(TrackingidValue);        
            }

            if (headers.Keys.Contains("Timestamp"))
            {
                headers.TryGetValue("Timestamp", out StringValues TimestampValue);
                contextualInfo.Timestamp = Convert.ToDateTime(TimestampValue);
                 
            }

            return Ok();
        }
    }
}
