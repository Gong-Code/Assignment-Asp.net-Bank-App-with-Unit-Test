using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.AccountManager
{
    public class TransactionListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TransactionListModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Id { get; set; }
        public string AccountType { get; set; }
        public DateTime Created { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }

        public void OnGet(int id)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions.OrderByDescending(c => c.Date))
                .First(c => c.Accounts.Any(a => a.Id == id));

            var account = _context.Accounts.First(a => a.Id == id);

            Id = id;
            AccountType = account.AccountType;
            Created = account.Created;
            Balance = account.Balance;
            Transactions = account.Transactions;
        }
    }
}
