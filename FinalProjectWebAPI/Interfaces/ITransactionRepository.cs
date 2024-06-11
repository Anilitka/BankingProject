using FinalProjectWebAPI.Models;

namespace FinalProjectWebAPI.Interfaces
{
    public interface ITransactionRepository
    {

        Transaction GetById(int id);
        IEnumerable<Transaction> GetAll();
        IEnumerable<Transaction> GetByAccNumber(string accountNumber);
        void Create(Transaction transaction);
        void Delete(int id);
    }
}
