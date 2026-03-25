namespace RestaurantApp.DTOs
{
    public class BillingDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BillingDate { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string PaymentMethod { get; set; } = "";
        public string? Notes { get; set; }
    }
}
