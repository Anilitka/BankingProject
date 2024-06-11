using System.Security.AccessControl;

namespace FinalProjectWebAPI.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string AccName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal AccBalance { get; set; }
        public AccType AccType { get; set; }
        public string AccNumber { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LlastUpdated { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        Random rnd = new Random();

        public Account()
        {
            AccNumber = GenerateRandomAccNumber();
        }
        private string GenerateRandomAccNumber()
        {
            char[] accountNumber = new char[8];
            for (int i = 0; i < 8; i++)
            {
                accountNumber[i] = (char)('0' + rnd.Next(10));
            }
            return new string(accountNumber);
        }
    }



    public enum AccType
    {
        Savings,
        Ordinary
    }
}
