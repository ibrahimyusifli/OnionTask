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
        [HttpPost("[Action]")]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {

            return StatusCode(StatusCodes.Status200OK, await _service.Login(dto));
           
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> LoginByRefresh(string refToken)
        {

            return Ok(await _service.LoginByRefreshToken(refToken));

        }
    }
}
