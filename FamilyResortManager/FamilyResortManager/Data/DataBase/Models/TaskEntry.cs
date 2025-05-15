using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FamilyResortManager.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyResortManager.Data.DataBase.Models
{
    [Table("TaskEntry")]
    public class TaskEntry
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = "Ожидает";

        public string? Comment { get; set; }

        public string? AssignedToUserId { get; set; }
        
        public virtual ApplicationUser? AssignedToUser { get; set; }

        public string? Category { get; set; }

        public string Priority { get; set; } = "Обычный";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual List<TaskLogEntry> Logs { get; set; } = new();
    }
}