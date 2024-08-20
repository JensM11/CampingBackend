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

            var owner = await _dataContext.GetOwnerByEmail(campingspot.OwnerEmail);
            if (owner == null)
            {
                return BadRequest("Owner with the given email does not exist.");
            }

            campingspot.IsAvailable = true;
            campingspot.ClientEmail = "None";

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
            var allCampingsites = await _dataContext.GetCampingSitesByOwnerEmail(ownerEmail);

            // Separate available and booked sites
            var availableSites = allCampingsites.Where(c => c.IsAvailable).ToList();
            var bookedSites = allCampingsites.Where(c => !c.IsAvailable).ToList();

            return Ok(new { AvailableSites = availableSites, BookedSites = bookedSites });
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

        [HttpGet("Available")]
        public async Task<ActionResult<IEnumerable<CampingSite>>> GetAvailableSites()
        {
            var campingsites = await _dataContext.GetAvailableSites();
            return Ok(campingsites);
        }

        [HttpPost("book/{id}")]
        public async Task<IActionResult> BookCampingSite(int id, [FromBody] BookingRequest request)
        {
            try
            {
                var campsite = await _dataContext.GetCampingSiteById(id);
                if (campsite == null)
                {
                    return NotFound($"Camping spot with ID '{id}' not found");
                }

                if (!campsite.IsAvailable)
                {
                    return BadRequest("Camping spot is not available");
                }

                campsite.IsAvailable = false;
                campsite.ClientEmail = request.ClientEmail; // Use the model property

                var success = await _dataContext.UpdateCampingSite(campsite);
                if (!success)
                {
                    return StatusCode(500, "Failed to update camping spot");
                }

                return Ok(campsite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("booked/{clientEmail}")]
        public async Task<ActionResult<IEnumerable<CampingSite>>> GetBookedSitesByClientEmail(string clientEmail)
        {
            var campingsites = await _dataContext.GetBookedSitesByClientEmail(clientEmail);
            return Ok(campingsites);
        }
    }


}
