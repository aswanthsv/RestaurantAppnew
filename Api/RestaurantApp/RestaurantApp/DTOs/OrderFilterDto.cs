namespace RestaurantApp.DTOs
{
    public class OrderFilterDto
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
