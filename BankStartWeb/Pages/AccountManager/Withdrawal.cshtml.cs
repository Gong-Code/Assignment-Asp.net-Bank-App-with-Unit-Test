using System.ComponentModel.DataAnnotations;
using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.AccountManager
{
    
    public class WithdrawalModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public WithdrawalModel(ApplicationDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        [BindProperty] public decimal Amount { get; set; }
        public string Operation { get; set; }
        public Account Account { get; set; }
        public Customer Customer { get; set; }
        

        public void OnGet(int accountId, int customerId)
        {
            AccountId = accountId;
            CustomerId = customerId;
        }

        public IActionResult OnPost(int accountId, int customerId)
        {
            if (ModelState.IsValid)
            {
                Customer = _context.Customers
                    .First(c => c.Id == customerId);
                
                Account = _context.Accounts
                    .Include(t => t.Transactions)
                    .First(a => a.Id == accountId);
                
                var status = _accountService.MakeWithdrawal(accountId, Amount);

                if (status == IAccountService.ErrorCode.InSufficientFunds)
                {
                    ModelState.AddModelError(nameof(Amount),
                        "Insufficient funds");

                    return Page();
                }

                if (status == IAccountService.ErrorCode.AmountIsNegative)
                {
                    ModelState.AddModelError(nameof(Amount),
                        "Cannot withdraw negative amounts.");

                    return Page();
                }

                if (status == IAccountService.ErrorCode.BalanceIsToLow)
                {
                    ModelState.AddModelError(nameof(Amount),
                        "Your current balance is to low.");

                    return Page();
                }

                if (status == IAccountService.ErrorCode.ok)
                {
                    return RedirectToPage("/AccountManager/TransactionList", new { customerId });
                }
            }

            return Page();
        }
    }
}
