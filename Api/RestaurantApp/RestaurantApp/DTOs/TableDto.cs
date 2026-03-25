namespace RestaurantApp.DTOs
{
    public class TableDto
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Seats { get; set; }
        public bool IsOccupied { get; set; }
    }
}
