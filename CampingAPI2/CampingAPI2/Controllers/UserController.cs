using Microsoft.AspNetCore.Mvc;
using CampingAPI2.Models;
using CampingAPI2.Data;

namespace CampingAPI2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IDataContext _dataContext;

        public UserController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            try
            {
                var registeredUser = await _dataContext.RegisterUser(user);
                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Email already exists"))
                {
                    return BadRequest(new { message = "Email already used" });
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            var user = await _dataContext.GetUserByEmail(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("Invalid email or password");
            }

            return Ok(user);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            try
            {
                var updatedUser = await _dataContext.UpdateUser(user);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _dataContext.GetUserByEmail(email);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }
    }
}
