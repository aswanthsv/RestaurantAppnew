using RestaurantApp.DTOs;

namespace RestaurantApp.Interfaces
{
    public interface ITableService
    {
        Task<IEnumerable<TableDto>> GetAllAsync();
        Task<TableDto> GetByIdAsync(int id);
        Task<TableDto> CreateAsync(TableDto dto);

        Task<TableDto?> UpdateAsync(int id,TableDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<DashboardTableDto>> GetDashboardOverviewAsync();

    }
}
