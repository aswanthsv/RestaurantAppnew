namespace RestaurantApp.Models
{
    public class Table
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Seats { get; set; }
        public bool IsOccupied {  get; set; }

        public ICollection<Order>? orders { get; set; }
    }
}
