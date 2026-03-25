using Microsoft.AspNetCore.Mvc;
using RestaurantApp.DTOs;
using RestaurantApp.Interfaces;

namespace RestaurantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _service;

        public TableController(ITableService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
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

        [HttpPost]
        public async Task<IActionResult> Create(TableDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TableDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound();

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

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _service.GetDashboardOverviewAsync();
            return Ok(result);
        }
    }
}
