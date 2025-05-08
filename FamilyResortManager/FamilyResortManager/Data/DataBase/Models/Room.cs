namespace FamilyResortManager.Data.DataBase.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Status { get; set; } = string.Empty;

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<CleaningTask> CleaningTasks { get; set; } = new List<CleaningTask>();
    }
}
