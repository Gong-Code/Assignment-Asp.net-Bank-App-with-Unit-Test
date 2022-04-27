using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.AccountManager
{
    [BindProperties]
    public class TransferModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public TransferModel(ApplicationDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public int AccountId { get; set; }
        public int TransferId { get; set; }
        public int CustomerId { get; set; }
        public string Operation { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public Customer Customer { get; set; }
        public List<Account> Accounts { get; set; }
        

        public void OnGet(int accountId, int customerId)
        {
            Customer = _context.Customers
                .Include(a => a.Accounts)
                .First(c => c.Id == customerId);
            
            Accounts = Customer.Accounts.Select(a => new Account
            {
                Id = a.Id,

            }).ToList();

            AccountId = accountId;
            CustomerId = customerId;

        }

        public IActionResult OnPost(int accountId, int customerId)
        {
            if (ModelState.IsValid)
            {
                Customer = _context.Customers.First(c => c.Id == customerId);
                var status = _accountService.Transfer(accountId, TransferId, Amount);
                
                if (status == IAccountService.ErrorCode.InSufficientFunds)
                {
                    ModelState.AddModelError(nameof(Amount),
                        "Insufficient funds");

                    return Page();
                }

                if (status == IAccountService.ErrorCode.AmountIsNegative)
                {
                    ModelState.AddModelError(nameof(Amount),
                        "You cannot transfer a negative amount.");

                    return Page();
                }

                return RedirectToPage("/AccountManager/TransactionList", new { customerId });

            }

            return Page();
        }
    }
}
