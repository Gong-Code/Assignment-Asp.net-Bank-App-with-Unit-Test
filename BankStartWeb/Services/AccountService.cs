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
            if (amount <= 0)
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

        public IAccountService.ErrorCode MakeWithdrawal(int accountId, decimal amount)
        {
            var account = _context.Accounts
                .Include(a => a.Transactions)
                .First(a => a.Id == accountId);
            
            if (account.Balance < amount)
            {
                return IAccountService.ErrorCode.BalanceIsToLow;
            }

            if (amount <= 0)
            {
                return IAccountService.ErrorCode.AmountIsNegative;
            }

            if (amount > account.Balance)
            {
                return IAccountService.ErrorCode.InSufficientFunds;
            }

            account.Balance -= amount;

            var transactions = new Transaction();
            {
                transactions.Type = "Credit";
                transactions.Amount = amount;
                transactions.Operation = "Withdrawal";
                transactions.Date = DateTime.Now;
                transactions.NewBalance = account.Balance;
            }

            account.Transactions.Add(transactions);

            _context.SaveChanges();

            return IAccountService.ErrorCode.ok;
        }

        public IAccountService.ErrorCode Transfer(int fromAccountId, int toAccountId, decimal amount)
        {
            var senderAccount = _context.Accounts
                .Include(s => s.Transactions)
                .First(a => a.Id == fromAccountId);

            if (amount > senderAccount.Balance)
            {
                return IAccountService.ErrorCode.InSufficientFunds;
            }

            if (amount <= 0)
            {
                return IAccountService.ErrorCode.AmountIsNegative;
            }


            var receiverAccount = _context.Accounts
                .Include(s => s.Transactions)
                .First(a => a.Id == toAccountId);

            var sender = new Transaction();
            {
                sender.Amount = amount;
                sender.Operation = "Transfer";
                sender.Date = DateTime.Now;
                sender.Type = "Credit";
                sender.NewBalance = senderAccount.Balance - amount;
            }


            var receiver = new Transaction();
            {
                receiver.Amount = amount;
                receiver.Operation = "Transfer";
                receiver.Date = DateTime.Now;
                receiver.Type = "Debit";
                receiver.NewBalance = receiverAccount.Balance + amount;
            }

            senderAccount.Balance -= amount;
            receiverAccount.Balance += amount;

            senderAccount.Transactions.Add(sender);
            receiverAccount.Transactions.Add(receiver);

            _context.SaveChanges();

            return IAccountService.ErrorCode.ok;

        }

        public IAccountService.ErrorCode AddAccount(int customerId, string accountType)
        {
            var customer = _context.Customers.First(c => c.Id == customerId);

            var account = new Account()
            {
                AccountType = accountType,
                Created = DateTime.Now,
                Balance = 0

            };

            customer.Accounts.Add(account);
            _context.SaveChanges();

            return IAccountService.ErrorCode.ok;
        }
    }
}
