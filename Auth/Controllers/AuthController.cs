using Microsoft.AspNetCore.Mvc;
using PumpFit_Stock.Auth;
using PumpFit_Stock.Auth.Models;
using System.Threading.Tasks;
using PumpFit_Stock.Auth.Services;
using Microsoft.AspNetCore.Identity.Data;
using System.Configuration;

namespace PumpFit_Stock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "Manager" && request.Password == "54321")
            {
                var token = _jwtService.GenerateToken(request.Username, "manager");
                return Ok(new { Token = token });
            }

            return Unauthorized(new { message = "Dados inválidos" });

        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}