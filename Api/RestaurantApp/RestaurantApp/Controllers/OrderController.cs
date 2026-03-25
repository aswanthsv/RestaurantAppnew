using Microsoft.AspNetCore.Mvc;
using RestaurantApp.DTOs;
using RestaurantApp.Interfaces;
using RestaurantApp.Services;

namespace RestaurantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet("top-dishes")]
        public async Task<IActionResult> GetTopDishes(int topN)
        {
            var result = await _service.GetTopDishes();
            return Ok(result);
        }

        [HttpGet("get-top-D")]
        public async Task<IActionResult> GetTopD()
        {
            var result = await _service.GetTopD();
            return Ok(result);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders([FromQuery] OrderFilterDto filter)
        {
            var result = await _service.GetFilteredOrders(filter);
            return Ok(result);
        }

        [HttpGet("daily-sales-summary")]
        public async Task<IActionResult> GetDailySalesSummary(DateTime date)
        {
            var result = await _service.GetDailySalesSummary(date);
            return Ok(result);
        }



        [HttpGet("daily-sales-summary-linq")]
        public async Task<IActionResult> GetDailySalesSummaryLinq(DateTime date)
        {
            var result = await _service.GetDailySalesSummaryLinq(date);
            return Ok(result);
        }

        [HttpGet("compare-performance")]
        public async Task<IActionResult> ComparePerformance(DateTime date)
        {
            var spResult = await _service.GetDailySalesSummaryWithTime(date);
            var linqResult = await _service.GetDailySalesSummaryLinqWithTime(date);

            return Ok(new
            {
                StoredProcedureTimeMs = spResult.TimeMs,
                LinqTimeMs = linqResult.TimeMs,
                StoredProcedureData = spResult.Data,
                LinqData = linqResult.Data
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDto dto)
        {
            var result = await _service.CreateOrderAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var updated = await _service.UpdateStatusAsync(id, status);
            if (!updated) return NotFound();
            return Ok();
        }

        [HttpGet("{id}/summary")]
        public async Task<IActionResult> GetSummary(int id)
        {
            var summary= await _service.GetSummaryByOrderIdAsync(id);
            if (summary == null) return NotFound();
            return Ok(summary);
        }
    }                               
}
