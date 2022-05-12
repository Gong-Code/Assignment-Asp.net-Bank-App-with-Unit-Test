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

        public int AccountCount { get; set; }
        public int CustomersCount { get; set; }
        public decimal TotalBalance { get; set; }

        public void OnGet()
        {
            CustomersCount = _context.Customers.Count();
            AccountCount = _context.Accounts.Count();
            TotalBalance = _context.Accounts.Sum(s => s.Balance);
        }

        
    }
}