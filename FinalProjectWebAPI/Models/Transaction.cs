using Microsoft.AspNetCore.Http.Connections;

namespace FinalProjectWebAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string TransactionReference { get; set; }

        public decimal TransacionAmount { get; set; }
        public string TransactionFrom { get; set; }
        public string TransactionTo { get; set; }
        public TransType TransactionTyoe { get; set; }
        public TransStatus TransactionStatus { get; set; }
    }

    public enum TransType
    {
        Deposit,
        Withdrowal,
        Transfer
    }
    public enum TransStatus
    {
        Success,
        Failed,
        Error
    }
}
