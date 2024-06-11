using FinalProjectWebAPI.Data;
using FinalProjectWebAPI.Interfaces;
using FinalProjectWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinalProjectWebAPI.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly List<Account> _accounts = new List<Account>();
        private readonly List<Transaction> _transactions = new List<Transaction>();
        private readonly IConfiguration _configuration;
        private readonly BankingDbContext _context;
        public AccountRepository(IConfiguration configuration, BankingDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public Account Create(Account account, string pass, string confirmPass)
        {
            if (pass != confirmPass)
                throw new ArgumentException("Passwords do not match");

            CreatePasswordHash(pass, out byte[] passwordHash, out byte[] passwordSalt);
            account.PasswordHash = passwordHash;
            account.PasswordSalt = passwordSalt;
            account.CreateDate = DateTime.Now;
            account.LlastUpdated = DateTime.Now;

            try
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"An error occurred while saving the entity changes: {message}", ex);
            }

            return account;
        }
        public string CreateToken(Account account)
        {
            List<Claim> claims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, account.AccNumber),
               new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        public RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(7)
            };
        }

        public void SetRefreshToken(Account account, RefreshToken refreshToken)
        {
            account.RefreshToken = refreshToken.Token;
            account.TokenCreated = refreshToken.Created;
            account.TokenExpires = refreshToken.Expires;
        }
        private void CreatePasswordHash(string pass, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            }
        }
        public void Delete(int id)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == id);
            if (account != null)
            {
                _accounts.Remove(account);
            }
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _context.Accounts.ToList();
        }

        public Account GetByAccNumber(string accNumber)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccNumber == accNumber);
        }

        public Account GetById(int id)
        {
            return _context.Accounts.FirstOrDefault(a => a.Id == id);
        }

        public Account LogIn(string accNumber, string Pass)
        {
            var account = _accounts.FirstOrDefault(a => a.AccNumber == accNumber);
            if (account == null || !VerifyPasswordHash(Pass, account.PasswordHash, account.PasswordSalt))
                return null;

            return account;
        }

        public void Update(Account account)
        {
            throw new NotImplementedException();
        }
        private bool VerifyPasswordHash(string Pass, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pass));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
        public void Deposit(string accNumber, decimal amount)
        {
            var account = GetByAccNumber(accNumber);
            if (account == null) throw new ArgumentException("Account not found");

            account.AccBalance += amount;
            account.LlastUpdated = DateTime.Now;

            var transaction = new Transaction
            {
                TransactionReference = Guid.NewGuid().ToString(),
                TransacionAmount = amount,
                TransactionFrom = accNumber,
                TransactionTo = accNumber,
                TransactionTyoe = TransType.Deposit,
                TransactionStatus = TransStatus.Success
            };
            _transactions.Add(transaction);
        }

        public void Withdraw(string accNumber, decimal amount)
        {
            var account = GetByAccNumber(accNumber);
            if (account == null) throw new ArgumentException("Account not found");

            if (account.AccBalance < amount) throw new InvalidOperationException("Insufficient funds");

            account.AccBalance -= amount;
            account.LlastUpdated = DateTime.Now;

            var transaction = new Transaction
            {
                TransactionReference = Guid.NewGuid().ToString(),
                TransacionAmount = amount,
                TransactionFrom = accNumber,
                TransactionTo = accNumber,
                TransactionTyoe = TransType.Withdrowal,
                TransactionStatus = TransStatus.Success
            };
            _transactions.Add(transaction);
        }
    }
}
