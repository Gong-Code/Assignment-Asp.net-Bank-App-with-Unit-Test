using System.ComponentModel.DataAnnotations;
using BankStartWeb.Data;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Services
{
    public class DepositService : IDepositService
    {
        private readonly ApplicationDbContext _context;

        public DepositService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<DepositViewModel> Deposit { get; set; }
        public class DepositViewModel
        {
            public int Id { get; set; }

            [MaxLength(10)]
            public string? AccountType { get; set; }

            public DateTime Created { get; set; }
            public decimal Balance { get; set; }
        }

        public void MakeDeposit(decimal amount, int accountId)
        {
            if (amount <= 0)
            {
                
            }

            var account = _context.Accounts
                .Include(a => a.Transactions)
                .First(a => a.Id == accountId);

            account.Balance += amount;

            var transactions = new Transaction
            {
                Type = "Debit",
                Amount = amount,
                Operation = "Deposit cash",
                Date = DateTime.Now,
                NewBalance = account.Balance,
            };

            account.Transactions.Add(transactions);
            _context.SaveChanges();
        }
    }
}
