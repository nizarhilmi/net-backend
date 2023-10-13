using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using visionet_webapi.Common.Command;
using visionet_webapi.Common.Dto;
using visionet_webapi.Services;

namespace visionet_webapi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public LoginDto Auth(AuthCommand command)
        {
            return authService.Login(command);
        }

    }
}
