using _09_auth.Data;
using _09_auth.Data.Models;
using _09_auth.DTO;
using _09_auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _09_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDtos dto)
        {
            var user = new User()
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            return Created("User successfuly created.", _repository.Create(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDtos dto)
        {
            var user = _repository.GetUserByEmail(dto.Email);
            if(user is null)
            {
                return BadRequest("Invalid login or password");
            }
            if(!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest("Invalid login or password");
            }

            var jwt = _jwtService.Generate(user.Id);
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new { 
               message = "Success",
            });
        }

        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Virify(jwt);
                var userId = int.Parse(token.Issuer);
                var user = _repository.GetUserById(userId);
                return Ok(user);
            }
            catch (Exception)
            {

                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new
            {
                message = "Success"
            });
        }
    }
}
