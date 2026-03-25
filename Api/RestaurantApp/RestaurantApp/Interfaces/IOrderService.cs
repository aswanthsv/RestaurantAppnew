using RestaurantApp.DTOs;

namespace RestaurantApp.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(OrderDto dto);
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task<bool> UpdateStatusAsync(int id, string status);
        Task<OrderSummaryDto?> GetSummaryByOrderIdAsync(int id);

        Task<List<TopDishDto>> GetTopDishes();

        Task<List<TopDDto>> GetTopD();
        Task<List<OrderFilterDto>> GetFilteredOrders();
        Task<List<DailySalesSummaryDto>> GetDailySalesSummary(DateTime date);

        Task<List<DailySalesSummaryDto>> GetDailySalesSummaryLinq(DateTime date);

        Task<(List<DailySalesSummaryDto> Data, long TimeMs)> GetDailySalesSummaryWithTime(DateTime date);

        Task<(List<DailySalesSummaryDto> Data, long TimeMs)> GetDailySalesSummaryLinqWithTime(DateTime date);



    }
}
