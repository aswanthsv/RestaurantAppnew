using System.Collections.Generic;

namespace RestaurantApp.DTOs
{
    public class OrderItemSummaryDto
    {
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal => Quantity * Price;

    }

    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public int TableId { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<OrderItemSummaryDto> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
    }
}
