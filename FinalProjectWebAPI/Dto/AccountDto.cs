using FinalProjectWebAPI.Models;

namespace FinalProjectWebAPI.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string AccName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal AccBalance { get; set; }
        public string AccNumber { get; set; }
        public AccType AccType { get; set; }
        public string Password { get; set; } 
        public string ConfirmPassword { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LlastUpdated { get; set; }
    }
}
