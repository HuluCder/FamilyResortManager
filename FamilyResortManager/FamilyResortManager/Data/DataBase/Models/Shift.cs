namespace FamilyResortManager.Data.DataBase.Models;

public class Shift
{
    public int Id { get; set; }

    public int StaffId { get; set; }
    public virtual Staff Staff { get; set; }

    public DateTime ShiftDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}