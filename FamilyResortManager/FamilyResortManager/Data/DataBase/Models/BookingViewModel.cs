using System.ComponentModel.DataAnnotations;

namespace FamilyResortManager.Data.DataBase.Models;

public class BookingViewModel
{
    public class InputModel
    {
        public int RoomId { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }
        
        public string Status { get; set; }
    }
    
    public InputModel Input { get; set; }
    
    [Required(ErrorMessage = "Имя клиента обязательно")]
    public string ClientName { get; set; }
    
    [Required(ErrorMessage = "Email клиента обязателен")]
    [EmailAddress(ErrorMessage = "Неверный формат email")]
    public string ClientEmail { get; set; }
    
    [Required(ErrorMessage = "Телефон клиента обязателен")]
    [Phone(ErrorMessage = "Неверный формат телефона")]
    public string ClientPhone { get; set; }
}
