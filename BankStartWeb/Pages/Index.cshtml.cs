using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankStartWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Customer> Customers { get; set; }
        public List<Account> Accounts { get; set; }

        public decimal TotalSum()
        {
            decimal total = Accounts.Sum(account => account.Balance);

            return Math.Round(total);
        }

        public void OnGet()
        {
            Customers = _context.Customers.Select(c => new Customer()).ToList();

            Accounts = _context.Accounts.Select(a => new Account
            {
                Balance = a.Balance

            }).ToList();
        }
    }
}