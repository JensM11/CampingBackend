using Microsoft.AspNetCore.Mvc;
using CampingAPI2.Models;
using CampingAPI2.Data;

namespace CampingAPI2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OwnerController : ControllerBase
    {
        private readonly IDataContext _dataContext;

        public OwnerController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterOwnerC([FromBody] Owner owner)
        {
            try
            {
                var registeredOwner = await _dataContext.RegisterOwner(owner);
                return Ok(registeredOwner);
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
            var owner = await _dataContext.GetOwnerByEmail(request.Email);

            if (owner == null || !BCrypt.Net.BCrypt.Verify(request.Password, owner.Password))
            {
                return Unauthorized("Invalid email or password");
            }

            return Ok(owner);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateOwner([FromBody] Owner owner)
        {
            try
            {
                var updatedOwner = await _dataContext.UpdateOwner(owner);
                return Ok(updatedOwner);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("email/{email}")]
        public IActionResult GetOwnerByEmail(string email)
        {
            var owner = _dataContext.GetOwnerByEmail(email);

            if (owner == null)
            {
                return NotFound(new { message = "Owner not found" });
            }

            return Ok(owner);
        }

        [HttpGet("id/{id}")]
        public IActionResult GetOwnerById(int id)
        {
            var owner = _dataContext.GetOwnerById(id);

            if (owner == null)
            {
                return NotFound(new { message = "Owner not found" });
            }

            return Ok(owner);
        }
    }
}
