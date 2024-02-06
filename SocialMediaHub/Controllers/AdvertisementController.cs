using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaHub.Models;
using SocialMediaHub.Repositories;

namespace SocialMediaHub.Controllers
{
    [Route("api/advertisements")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementRepository _advertisementRepository;

        public AdvertisementController(IAdvertisementRepository advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAdvertisements()
        {
            var advertisements = await _advertisementRepository.GetAllAdvertisements();
            return Ok(advertisements);
        }

        [HttpGet("{advertisementId}")]
        public async Task<IActionResult> GetAdvertisement(int advertisementId)
        {
            var advertisement = await _advertisementRepository.GetAdvertisement(advertisementId);
            if (advertisement == null)
                return NotFound();

            return Ok(advertisement);
        }

        [HttpPost]
        public async Task<IActionResult> AddAdvertisement([FromBody] Advertisement advertisement)
        {
            await _advertisementRepository.AddAdvertisement(advertisement);
            return Ok("Advertisement added successfully.");
        }

        [HttpPut("{advertisementId}")]
        public async Task<IActionResult> UpdateAdvertisement(int advertisementId, [FromBody] Advertisement updatedAdvertisement)
        {
            try
            {
                await _advertisementRepository.UpdateAdvertisement(updatedAdvertisement);
                return Ok("Advertisement updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{advertisementId}")]
        public async Task<IActionResult> RemoveAdvertisement(int advertisementId)
        {
            try
            {
                await _advertisementRepository.RemoveAdvertisement(advertisementId);
                return Ok("Advertisement removed successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("csv")]
        public async Task<IActionResult> GetAdvertisementsCsv()
        {
            try
            {
                var advertisementsCsvBytes = await _advertisementRepository.GetAdvertisementsCsvBytes();
                return File(advertisementsCsvBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "advertisements.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
