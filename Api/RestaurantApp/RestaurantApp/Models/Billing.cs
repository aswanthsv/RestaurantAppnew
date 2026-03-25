using System;

namespace RestaurantApp.Models
{
    public class Billing
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BillingDate { get; set; } = DateTime.Now;
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Cancelled
        public string PaymentMethod { get; set; } = ""; // Cash, Card, Online, etc.
        public string? Notes { get; set; }

        // Navigation properties
        public Order? Order { get; set; }
    }
}
