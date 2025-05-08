namespace FamilyResortManager.Data.DTOs
{
    public class CreateEmployeeDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class EmployeeDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
    
    public class UpdateEmployeeDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}