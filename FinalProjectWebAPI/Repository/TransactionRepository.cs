using FinalProjectWebAPI.Interfaces;
using FinalProjectWebAPI.Models;

namespace FinalProjectWebAPI.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly List<Transaction> _transactions = new List<Transaction>();
        public void Create(Transaction transaction)
        {
            _transactions.Add(transaction);
        }

        public void Delete(int id)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                _transactions.Remove(transaction);
            }
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _transactions;
        }

        public IEnumerable<Transaction> GetByAccNumber(string accountNumber)
        {
            return _transactions.Where(t => t.TransactionFrom == accountNumber || t.TransactionTo == accountNumber);
        }

        public Transaction GetById(int id)
        {
            return _transactions.FirstOrDefault(t => t.Id == id);
        }

    }
}
