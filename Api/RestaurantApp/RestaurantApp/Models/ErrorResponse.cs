namespace RestaurantApp.Models
{
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; }
        public string? TraceId { get; set; }
    }
}
