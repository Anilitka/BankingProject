using FinalProjectWebAPI.Models;
using System.Collections.Generic;

namespace FinalProjectWebAPI.Interfaces
{
    public interface IAccountRepository
    {
        Account LogIn(string accNumber, string Pass);
        IEnumerable<Account> GetAccounts();
        Account Create(Account account, string Pass, string confirmPass);
        void Update(Account account);
        void Delete(int id);
        Account GetById(int id);
        Account GetByAccNumber(string accNumber);
        void SetRefreshToken(Account account, RefreshToken refreshToken);
        void Deposit(string accNumber, decimal amount);
        void Withdraw(string accNumber, decimal amount);

    }
}
