using System;
using System.Collections.Generic;

namespace RestaurantApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending"; // Pending, Served, Completed

        // Navigation properties
        public Table? Table { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }

    }
}
