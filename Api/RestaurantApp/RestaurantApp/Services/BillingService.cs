using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.DTOs;
using RestaurantApp.Interfaces;
using RestaurantApp.Models;
using System.Diagnostics;

namespace RestaurantApp.Services
{
    public class BillingService : IBillingService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BillingService> _logger;

        public BillingService(AppDbContext context, ILogger<BillingService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<BillingDto> CreateBillingAsync(BillingDto dto, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("➜ CreateBilling START: OrderId={OrderId}, Amount={Amount}", dto.OrderId, dto.Amount);

            try
            {
                var billing = new Billing
                {
                    OrderId = dto.OrderId,
                    Amount = dto.Amount,
                    PaymentStatus = dto.PaymentStatus ?? "Pending",
                    PaymentMethod = dto.PaymentMethod ?? "",
                    Notes = dto.Notes
                };

                _context.Billings.Add(billing);
                await _context.SaveChangesAsync(cancellationToken);
                sw.Stop();

                _logger.LogInformation("✓ CreateBilling SUCCESS in {ElapsedMs}ms | BillingId={Id}", sw.ElapsedMilliseconds, billing.Id);

                dto.Id = billing.Id;
                dto.BillingDate = billing.BillingDate;
                return dto;
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "✗ CreateBilling FAILED after {ElapsedMs}ms", sw.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task<IEnumerable<BillingDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("➜ GetAllBillings START");

            try
            {
                var billings = await _context.Billings
                    .Include(b => b.Order)
                    .ToListAsync(cancellationToken);
                sw.Stop();

                _logger.LogInformation("✓ GetAllBillings SUCCESS in {ElapsedMs}ms | Count={Count}", sw.ElapsedMilliseconds, billings.Count);

                return billings.Select(b => new BillingDto
                {
                    Id = b.Id,
                    OrderId = b.OrderId,
                    Amount = b.Amount,
                    BillingDate = b.BillingDate,
                    PaymentStatus = b.PaymentStatus,
                    PaymentMethod = b.PaymentMethod,
                    Notes = b.Notes
                });
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "✗ GetAllBillings FAILED after {ElapsedMs}ms", sw.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task<BillingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("➜ GetBillingById START: Id={Id}", id);

            try
            {
                var billing = await _context.Billings
                    .Include(b => b.Order)
                    .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
                sw.Stop();

                if (billing == null)
                {
                    _logger.LogWarning("⚠ GetBillingById NOT FOUND in {ElapsedMs}ms | Id={Id}", sw.ElapsedMilliseconds, id);
                    return null;
                }

                _logger.LogInformation("✓ GetBillingById SUCCESS in {ElapsedMs}ms | Id={Id}", sw.ElapsedMilliseconds, id);

                return new BillingDto
                {
                    Id = billing.Id,
                    OrderId = billing.OrderId,
                    Amount = billing.Amount,
                    BillingDate = billing.BillingDate,
                    PaymentStatus = billing.PaymentStatus,
                    PaymentMethod = billing.PaymentMethod,
                    Notes = billing.Notes
                };
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "✗ GetBillingById FAILED after {ElapsedMs}ms | Id={Id}", sw.ElapsedMilliseconds, id);
                throw;
            }
        }

        public async Task<IEnumerable<BillingDto>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("➜ GetBillingByOrderId START: OrderId={OrderId}", orderId);

            try
            {
                var billings = await _context.Billings
                    .Where(b => b.OrderId == orderId)
                    .ToListAsync(cancellationToken);
                sw.Stop();

                _logger.LogInformation("✓ GetBillingByOrderId SUCCESS in {ElapsedMs}ms | Count={Count}", sw.ElapsedMilliseconds, billings.Count);

                return billings.Select(b => new BillingDto
                {
                    Id = b.Id,
                    OrderId = b.OrderId,
                    Amount = b.Amount,
                    BillingDate = b.BillingDate,
                    PaymentStatus = b.PaymentStatus,
                    PaymentMethod = b.PaymentMethod,
                    Notes = b.Notes
                });
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "✗ GetBillingByOrderId FAILED after {ElapsedMs}ms", sw.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task<bool> UpdatePaymentStatusAsync(int id, string paymentStatus, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("➜ UpdatePaymentStatus START: Id={Id}, Status={Status}", id, paymentStatus);

            try
            {
                var billing = await _context.Billings.FindAsync(new object[] { id }, cancellationToken);
                if (billing == null)
                {
                    sw.Stop();
                    _logger.LogWarning("⚠ UpdatePaymentStatus NOT FOUND in {ElapsedMs}ms | Id={Id}", sw.ElapsedMilliseconds, id);
                    return false;
                }

                billing.PaymentStatus = paymentStatus;
                await _context.SaveChangesAsync(cancellationToken);
                sw.Stop();

                _logger.LogInformation("✓ UpdatePaymentStatus SUCCESS in {ElapsedMs}ms | Id={Id}", sw.ElapsedMilliseconds, id);
                return true;
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "✗ UpdatePaymentStatus FAILED after {ElapsedMs}ms", sw.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task<bool> DeleteBillingAsync(int id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("➜ DeleteBilling START: Id={Id}", id);

            try
            {
                var billing = await _context.Billings.FindAsync(new object[] { id }, cancellationToken);
                if (billing == null)
                {
                    sw.Stop();
                    _logger.LogWarning("⚠ DeleteBilling NOT FOUND in {ElapsedMs}ms | Id={Id}", sw.ElapsedMilliseconds, id);
                    return false;
                }

                _context.Billings.Remove(billing);
                await _context.SaveChangesAsync(cancellationToken);
                sw.Stop();

                _logger.LogInformation("✓ DeleteBilling SUCCESS in {ElapsedMs}ms | Id={Id}", sw.ElapsedMilliseconds, id);
                return true;
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "✗ DeleteBilling FAILED after {ElapsedMs}ms", sw.ElapsedMilliseconds);
                throw;
            }
        }
    }
}

