using Microsoft.AspNetCore.Mvc;
using RestaurantApp.DTOs;
using RestaurantApp.Interfaces;
using System.Diagnostics;

namespace RestaurantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _service;
        private readonly ILogger<BillingController> _logger;

        public BillingController(IBillingService service, ILogger<BillingController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(BillingDto dto, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("?? POST /api/billing - OrderId={OrderId}, Amount={Amount}", dto.OrderId, dto.Amount);

            try
            {
                var result = await _service.CreateBillingAsync(dto, cancellationToken);
                sw.Stop();
                _logger.LogInformation("? POST /api/billing SUCCESS in {ElapsedMs}ms", sw.ElapsedMilliseconds);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "? POST /api/billing FAILED in {ElapsedMs}ms", sw.ElapsedMilliseconds);
                throw;
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("?? GET /api/billing - GetAll");

            try
            {
                var billings = await _service.GetAllAsync(cancellationToken);
                sw.Stop();
                var count = billings.Count();
                _logger.LogInformation("? GET /api/billing SUCCESS in {ElapsedMs}ms | Count={Count}", sw.ElapsedMilliseconds, count);
                return Ok(billings);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "? GET /api/billing FAILED in {ElapsedMs}ms", sw.ElapsedMilliseconds);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("?? GET /api/billing/{Id}", id);

            try
            {
                var billing = await _service.GetByIdAsync(id, cancellationToken);
                sw.Stop();

                if (billing == null)
                {
                    _logger.LogWarning("? GET /api/billing/{Id} - NOT FOUND in {ElapsedMs}ms", id, sw.ElapsedMilliseconds);
                    return NotFound();
                }

                _logger.LogInformation("? GET /api/billing/{Id} SUCCESS in {ElapsedMs}ms", id, sw.ElapsedMilliseconds);
                return Ok(billing);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "? GET /api/billing/{Id} FAILED in {ElapsedMs}ms", id, sw.ElapsedMilliseconds);
                throw;
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("?? GET /api/billing/order/{OrderId}", orderId);

            try
            {
                var billings = await _service.GetByOrderIdAsync(orderId, cancellationToken);
                sw.Stop();
                var count = billings.Count();
                _logger.LogInformation("? GET /api/billing/order/{OrderId} SUCCESS in {ElapsedMs}ms | Count={Count}", orderId, sw.ElapsedMilliseconds, count);
                return Ok(billings);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "? GET /api/billing/order/{OrderId} FAILED in {ElapsedMs}ms", orderId, sw.ElapsedMilliseconds);
                throw;
            }
        }

        [HttpPut("{id}/payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromQuery] string paymentStatus, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("?? PUT /api/billing/{Id}/payment-status - Status={Status}", id, paymentStatus);

            try
            {
                var updated = await _service.UpdatePaymentStatusAsync(id, paymentStatus, cancellationToken);
                sw.Stop();

                if (!updated)
                {
                    _logger.LogWarning("? PUT /api/billing/{Id}/payment-status - NOT FOUND in {ElapsedMs}ms", id, sw.ElapsedMilliseconds);
                    return NotFound();
                }

                _logger.LogInformation("? PUT /api/billing/{Id}/payment-status SUCCESS in {ElapsedMs}ms", id, sw.ElapsedMilliseconds);
                return Ok(new { message = "Payment status updated successfully" });
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "? PUT /api/billing/{Id}/payment-status FAILED in {ElapsedMs}ms", id, sw.ElapsedMilliseconds);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBilling(int id, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("?? DELETE /api/billing/{Id}", id);

            try
            {
                var deleted = await _service.DeleteBillingAsync(id, cancellationToken);
                sw.Stop();

                if (!deleted)
                {
                    _logger.LogWarning("? DELETE /api/billing/{Id} - NOT FOUND in {ElapsedMs}ms", id, sw.ElapsedMilliseconds);
                    return NotFound();
                }

                _logger.LogInformation("? DELETE /api/billing/{Id} SUCCESS in {ElapsedMs}ms", id, sw.ElapsedMilliseconds);
                return Ok(new { message = "Billing deleted successfully" });
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "? DELETE /api/billing/{Id} FAILED in {ElapsedMs}ms", id, sw.ElapsedMilliseconds);
                throw;
            }
        }
    }
}


