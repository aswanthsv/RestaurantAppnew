namespace RestaurantApp.DTOs
{
    public class DashboardTableDto
    {
        public int TableId { get; set; }
        public string Status { get; set; } = "Available";
        public int? CurrentOrderId { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
