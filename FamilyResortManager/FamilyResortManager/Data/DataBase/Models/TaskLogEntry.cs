using System;
using System.ComponentModel.DataAnnotations.Schema;
using FamilyResortManager.Data.Identity;

namespace FamilyResortManager.Data.DataBase.Models
{
    [Table("TaskLogEntry")]
    public class TaskLogEntry
    {
        public int Id { get; set; }

        public int TaskEntryId { get; set; }
        public virtual TaskEntry TaskEntry { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string Action { get; set; } // "Создана", "Комментарий обновлён", "Завершена"
    }
}