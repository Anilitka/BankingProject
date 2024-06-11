namespace FinalProjectWebAPI.Dto
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string TransactionReference { get; set; }
        public decimal TransacionAmount { get; set; }
        public string TransactionFrom { get; set; }
        public string TransactionTo { get; set; }
        public string TransactionType { get; set; }
        public string TransactionStatus { get; set; }
    }
}
