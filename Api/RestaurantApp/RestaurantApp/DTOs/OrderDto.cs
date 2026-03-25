namespace RestaurantApp.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int TableId {  get; set; }
        public string Status { get; set; } = "Pending";
        public List<OrderItemDto>? Items { get; set; }
    }
}
