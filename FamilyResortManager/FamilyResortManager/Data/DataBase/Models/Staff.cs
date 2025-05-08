namespace FamilyResortManager.Data.DataBase.Models;

public class Staff
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public double HourlyRate { get; set; }

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
    public virtual ICollection<CleaningTask> CleaningTasks { get; set; } = new List<CleaningTask>();
}