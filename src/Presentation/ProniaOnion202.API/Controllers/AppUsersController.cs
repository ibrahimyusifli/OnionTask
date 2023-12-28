using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProniaOnion202.Application.DTOs.Users;

namespace ProniaOnion202.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AppUsersController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterDto dto)
        {
            await _service.Register(dto);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
