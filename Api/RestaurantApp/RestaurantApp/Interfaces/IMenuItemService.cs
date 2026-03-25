using RestaurantApp.DTOs;

namespace RestaurantApp.Interfaces
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemDto>> GetAllAsync();
        Task<MenuItemDto?> GetByIdAsync(int id);
        Task<MenuItemDto> CreateAsync(MenuItemDto dto);
        Task<MenuItemDto?> UpdateAsync(int id, MenuItemDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
