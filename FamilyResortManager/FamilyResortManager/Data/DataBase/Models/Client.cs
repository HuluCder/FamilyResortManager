namespace FamilyResortManager.Data.DataBase.Models;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Info { get; set; } = string.Empty;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}