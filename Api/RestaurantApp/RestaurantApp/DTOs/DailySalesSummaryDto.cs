namespace RestaurantApp.DTOs
{
    public class DailySalesSummaryDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public DateTime Date { get; set; }
    }
}
