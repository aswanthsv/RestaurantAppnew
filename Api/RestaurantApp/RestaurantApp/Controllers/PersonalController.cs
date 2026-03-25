using Microsoft.AspNetCore.Mvc;
using RestaurantApp.DTOs;
using RestaurantApp.Interfaces;

namespace RestaurantApp.Controllers
{
    public class PersonalController : ControllerBase
    {
        private readonly IPersonalService _service;

        public PersonalController(IPersonalService service)
        {
            _service = service;
        }

        [HttpGet("personal-get")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllPersonalAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("add-data")]
        public async Task<IActionResult> Create(PersonalDto dto)
        {
            var result = await _service.CreatePersonalAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("personal-get-filter")]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null)
        {
            var result = await _service.GetAllPersonalAsync(search);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
