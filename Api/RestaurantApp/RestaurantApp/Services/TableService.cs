using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.DTOs;
using RestaurantApp.Interfaces;
using RestaurantApp.Models;

namespace RestaurantApp.Services
{
    public class TableService : ITableService
    {
        private readonly AppDbContext _context;

        public TableService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TableDto>> GetAllAsync()
        {   
            var tables = await _context.Tables.ToListAsync();
            return tables.Select(t => new TableDto
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                Seats = t.Seats,
                IsOccupied = t.IsOccupied
            });
        }

        public async Task<TableDto?> GetByIdAsync(int id)
        {
            var t = await _context.Tables.FindAsync(id);
            if (t == null) return null;

            return new TableDto
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                Seats = t.Seats,
                IsOccupied = t.IsOccupied
            };
        }

        public async Task<TableDto> CreateAsync(TableDto dto)
        {
            var entity = new Models.Table
            {
                TableNumber = dto.TableNumber,
                Seats = dto.Seats,
                IsOccupied = dto.IsOccupied
            };

            _context.Tables.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task<TableDto?> UpdateAsync(int id, TableDto dto)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null) return null;

            table.TableNumber = dto.TableNumber;
            table.Seats = dto.Seats;
            table.IsOccupied = dto.IsOccupied;

            await _context.SaveChangesAsync();
            return dto;
        }  

        public async Task<bool> DeleteAsync(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null) return false;

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DashboardTableDto>> GetDashboardOverviewAsync()
        {
            var tables = await _context.Tables
                .Include(t => t.orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .ToListAsync();

            var dashboard = tables.Select(t =>
            {
                var activeOrder = t.orders?
                    .Where(o => o.Status == "Pending" || o.Status == "Served")
                    .OrderByDescending(o => o.Id)
                    .FirstOrDefault();

                decimal? total = null;

                if (activeOrder != null && activeOrder.OrderItems != null)
                {
                    total = activeOrder.OrderItems.Sum(i => (i.MenuItem?.Price ?? 0) * i.Quantity);
                }

                return new DashboardTableDto
                {
                    TableId = t.Id,
                    Status = activeOrder != null ? activeOrder.Status : "Available",
                    CurrentOrderId = activeOrder?.Id,
                    TotalAmount = total
                };
            }).ToList();

            return dashboard;
        }

    }
}
