using EducationAPI.DTOs;
using EducationAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login-teacher")]
        public async Task<IActionResult> LoginAsTeacher(LoginDto dto)
        {
            var token = await _authService.LoginAsTeacher(dto);
            return Ok(new { token });
        }

        [HttpPost("login-student")]
        public async Task<IActionResult> LoginAsStudent(LoginDto dto)
        {
            var token = await _authService.LoginAsStudent(dto);
            return Ok(new { token });
        }
    }
}
