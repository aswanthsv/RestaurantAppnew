using RestaurantApp.DTOs;
using System.Threading;

namespace RestaurantApp.Interfaces
{
    public interface IBillingService
    {
        Task<BillingDto> CreateBillingAsync(BillingDto dto, CancellationToken cancellationToken = default);
        Task<IEnumerable<BillingDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<BillingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<BillingDto>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
        Task<bool> UpdatePaymentStatusAsync(int id, string paymentStatus, CancellationToken cancellationToken = default);
        Task<bool> DeleteBillingAsync(int id, CancellationToken cancellationToken = default);
    }
}
