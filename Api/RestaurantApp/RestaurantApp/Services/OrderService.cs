using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.DTOs;
using RestaurantApp.Interfaces;
using RestaurantApp.Models;
using System.Diagnostics;

namespace RestaurantApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<DailySalesSummaryDto>> GetDailySalesSummary(DateTime date)
        {
            return await _context.DailySalesSummaries
                .FromSqlRaw("EXEC GetDailySalesSummary @Date = {0}", date)
                .ToListAsync();
        }

        public async Task<List<DailySalesSummaryDto>> GetDailySalesSummaryLinq(DateTime date)
        {
            var result = await (
                from o in _context.Orders
                join b in _context.Billings
                on o.Id equals b.OrderId into billingGroup
                from b in billingGroup.DefaultIfEmpty()
                where o.OrderTime.Date == date.Date
                group new { o, b } by o.OrderTime.Date into g
                select new DailySalesSummaryDto
                {
                    Date = g.Key,
                    TotalOrders = g.Select(x => x.o.Id).Distinct().Count(),
                    TotalRevenue = g.Sum(x => x.b != null ? x.b.Amount : 0)
                }
                ).ToListAsync();
            
            return result;
                
        }

        public async Task<(List<DailySalesSummaryDto> Data, long TimeMs)> GetDailySalesSummaryWithTime(DateTime date)
        {
            var stopwatch = Stopwatch.StartNew();

            var data = await _context.DailySalesSummaries
                .FromSqlRaw("EXEC GetDailySalesSummary @Date = {0}", date)
                .ToListAsync();

            stopwatch.Stop();

            return (data, stopwatch.ElapsedMilliseconds);
        }

        public async Task<(List<DailySalesSummaryDto> Data, long TimeMs)> GetDailySalesSummaryLinqWithTime(DateTime date)
        {
            var stopwatch = Stopwatch.StartNew();

            var data = await GetDailySalesSummaryLinq(date);

            stopwatch.Stop();

            return (data, stopwatch.ElapsedMilliseconds);
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto dto)
        {
            var order = new Order
            {
                TableId = dto.TableId,
                Status = "Pending",
                OrderItems = dto.Items?.Select(i => new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            dto.Id = order.Id;
            return dto;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                TableId = o.TableId,
                Status = o.Status,
                Items = o.OrderItems?.Select(oi => new OrderItemDto
                {
                    MenuItemId = oi.MenuItemId,
                    Quantity = oi.Quantity
                }).ToList()
            });
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                TableId = order.TableId,
                Status = order.Status,
                Items = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    MenuItemId = oi.MenuItemId,
                    Quantity = oi.Quantity
                }).ToList()
            };
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TopDishDto>> GetTopDishes()
        {
            return await _context.OrderItems
                .Include(oi => oi.MenuItem)
                .GroupBy(oi => oi.MenuItemId)
                .Select(g => new TopDishDto
                {
                    Name = g.FirstOrDefault().MenuItem.Name,
                    TotalSold = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(d => d.TotalSold)
                .Take(4)
                .ToListAsync();
        }

        public async Task<List<TopDDto>> GetTopD()
        {
            return await _context.OrderItems
                .Include(oi => oi.MenuItem)
                .GroupBy(oi => oi.MenuItemId)
                .Select(g => new TopDDto
                {
                    Name = g.FirstOrDefault().MenuItem.Name,
                    totals = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(d => d.totals)
                .Take(5)
                .ToListAsync();
        }

        public async Task<List<Order>> GetFilteredOrders(OrderFilterDto filter)
        {
            var query = _context.Orders.AsQueryable();

            // 🔍 Search
            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(o =>
                    o.Customer.Name.Contains(filter.Search) ||
                    o.Id.ToString().Contains(filter.Search));
            }

            // 📌 Status Filter
            if (!string.IsNullOrEmpty(filter.Status))
            {
                query = query.Where(o => o.Status == filter.Status);
            }

            // 📅 Date Range Filter
            if (filter.StartDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(o => o.orde <= filter.EndDate.Value);
            }

            return await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }


        public async Task<OrderSummaryDto?> GetSummaryByOrderIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Table)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            var summary = new OrderSummaryDto
            {
                OrderId = order.Id,
                TableId = order.TableId,
                Status = order.Status,
                Items = order.OrderItems?.Select(oi => new OrderItemSummaryDto
                {
                    ItemName = oi.MenuItem?.Name ?? "Unknown",
                    Quantity = oi.Quantity,
                    Price = oi.MenuItem?.Price ?? 0
                }).ToList() ?? new List<OrderItemSummaryDto>()
            };

            return summary;
        }
    }
}
