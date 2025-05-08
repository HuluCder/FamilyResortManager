namespace FamilyResortManager.Data.DataBase.Models;

public class Booking
{
    public int Id { get; set; }

    public int RoomId { get; set; }
    public virtual Room Room { get; set; }

    public int ClientId { get; set; }
    public virtual  Client Client { get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public string Status { get; set; } = string.Empty;
}