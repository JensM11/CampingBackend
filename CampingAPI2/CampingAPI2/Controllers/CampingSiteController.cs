using CampingAPI2.Data;
using Microsoft.AspNetCore.Mvc;
using CampingAPI2.Models;

namespace CampingAPI2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampingSiteController : ControllerBase
    {
        private IDataContext _dataContext;

        public CampingSiteController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampingSite>>> GetCampingSites()
        {
            var campingsites = await _dataContext.GetSites();
            return Ok(campingsites);
        }

        [HttpPost]
        public async Task<ActionResult<CampingSite>> CreateCampingSite(CampingSite campingspot)
        {

            campingspot.IsAvailable = true;

            try
            {
                var createdCampingsite = await _dataContext.AddCampingSite(campingspot);
                if (createdCampingsite == null)
                {
                    // Handle the case where the campingspot creation failed
                    return StatusCode(500, "Failed to create camping spot");
                }

                return CreatedAtAction(nameof(GetCampingSites), new { id = createdCampingsite.Id }, createdCampingsite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{ownerEmail}")]
        public async Task<ActionResult<IEnumerable<CampingSite>>> GetCampingSitesByOwnerEmail(string ownerEmail)
        {
            var campingsites = await _dataContext.GetCampingSitesByOwnerEmail(ownerEmail);
            return Ok(campingsites);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteCampingSite(string name)
        {
            try
            {
                var success = await _dataContext.DeleteCampingSite(name);
                if (!success)
                    return NotFound($"Camping spot with name '{name}' not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet ("Available")]
        public async Task<ActionResult<IEnumerable<CampingSite>>> GetAvailableSites()
        {
            // Get all camping sites where IsAvailable is true
            var campingsites = await _dataContext.GetAvailableSites();
            return Ok(campingsites);
        }
    }
}
