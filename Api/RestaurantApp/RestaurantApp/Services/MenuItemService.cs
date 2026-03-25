using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.DTOs;
using RestaurantApp.Interfaces;
using RestaurantApp.Models;

namespace RestaurantApp.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly AppDbContext _context;

        public MenuItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItemDto>> GetAllAsync()
        {
            var items = await _context.MenuItems.ToListAsync();
            return items.Select(m => new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Price = m.Price,
                Category = m.Category
            });
        }
        
        public async Task<MenuItemDto?> GetByIdAsync(int id)
        {
            var m = await _context.MenuItems.FindAsync(id);
            if (m == null) return null;

            return new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Price = m.Price,
                Category = m.Category
            };
        }

        public async Task<MenuItemDto> CreateAsync(MenuItemDto dto)
        {
            var entity = new MenuItem
            {
                Name = dto.Name,
                Price = dto.Price,
                Category = dto.Category
            };

            _context.MenuItems.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task<MenuItemDto?> UpdateAsync(int id, MenuItemDto dto)
        {
            var entity = await _context.MenuItems.FindAsync(id);
            if (entity == null) return null;

            entity.Name = dto.Name;
            entity.Price = dto.Price;
            entity.Category = dto.Category;

            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.MenuItems.FindAsync(id);
            if (entity == null) return false;

            _context.MenuItems.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
