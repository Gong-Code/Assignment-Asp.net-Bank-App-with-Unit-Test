using BankStartWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IAccountService.ErrorCode MakeDeposit(int accountId, decimal amount)
        {
            if (amount < 0)
            {
                return IAccountService.ErrorCode.AmountIsNegative;
            }

            var account = _context.Accounts
                .Include(a => a.Transactions)
                .First(a => a.Id == accountId);

            account.Balance += amount;

            var transaction = new Transaction();
            {
                transaction.Type = "Debit";
                transaction.Amount = amount;
                transaction.Operation = "Deposit cash";
                transaction.Date = DateTime.Now;
                transaction.NewBalance = account.Balance;
            }

            account.Transactions.Add(transaction);

            _context.SaveChanges();

            return IAccountService.ErrorCode.ok;
        }

        public IAccountService.ErrorCode MakeWithdrawal(int accountId, decimal amount, string type)
        {
            var account = _context.Accounts.First(a => a.Id == accountId);

            if (account.Balance < amount)
            {
                return IAccountService.ErrorCode.BalanceIsToLow;
            }

            if (amount < 0)
            {
                return IAccountService.ErrorCode.AmountIsNegative;
            }

            account.Balance -= amount;

            var transaction = new Transaction();
            {
                transaction.Type = type;
                transaction.Operation = "Withdrawal";
                transaction.Amount = amount;
                transaction.Date = DateTime.UtcNow;
                transaction.NewBalance = account.Balance;
            }

            account.Transactions.Add(transaction);

            _context.SaveChanges();

            return IAccountService.ErrorCode.ok;
        }
    }
}
