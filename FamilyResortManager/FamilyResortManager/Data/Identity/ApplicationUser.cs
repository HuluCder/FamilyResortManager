using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using FamilyResortManager.Data.DataBase.Models;

namespace FamilyResortManager.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public string? TelegramChatId { get; set; }

        public virtual ICollection<TaskEntry> AssignedTasks { get; set; }
        public virtual ICollection<TaskLogEntry> Logs { get; set; }
    }
}