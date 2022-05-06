using System.ComponentModel.DataAnnotations;
using BankStartWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.AccountManager
{
    [Authorize(Roles = "Admin, Cashier")]
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
        public class TransactionViewModel
        {
            public int Id { get; set; }

            [MaxLength(10)]
            public string Type { get; set; }
            [MaxLength(50)]
            public string Operation { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public decimal NewBalance { get; set; }

        }

        public IActionResult OnGetFetchMore(int customerId, int pageNum)
        {
            var query = _context.Accounts.Where(e => e.Id == customerId)
                .SelectMany(e => e.Transactions)
                .OrderByDescending(e => e.Date);

            var r = query.GetPaged(pageNum, 5);

            var list = r.Results.Select(e => new TransactionViewModel
            {
                Id = e.Id,
                Amount = e.Amount,
                Type = e.Type,
                Operation = e.Operation,
                Date = e.Date,
                NewBalance = e.NewBalance
            }).ToList();

            bool lastPage = pageNum == r.PageCount;

            return new JsonResult(new { items = list, lastPage = lastPage });
        }

        public void OnGet(int customerId)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions.OrderByDescending(c => c.Date))
                .First(c => c.Accounts.Any(a => a.Id == customerId));

            var account = _context.Accounts.First(a => a.Id == customerId);

            Id = customerId;
            AccountType = account.AccountType;
            Created = account.Created;
            Balance = account.Balance;
            Transactions = account.Transactions;
        }
    }

}
