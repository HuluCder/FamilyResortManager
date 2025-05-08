namespace FamilyResortManager.Data.DataBase.Models;

public class CleaningTask
{
    public int Id { get; set; }

    public int RoomId { get; set; }
    public virtual Room Room { get; set; }

    public int StaffId { get; set; }
    public virtual Staff Staff { get; set; }

    public DateTime TaskDate { get; set; }
    public string Status { get; set; } = string.Empty;
}