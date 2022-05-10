using System.ComponentModel.DataAnnotations;
using AspNetCoreHero.ToastNotification.Abstractions;
using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.AccountManager
{
    [Authorize(Roles = "Admin, Cashier")]
    public class DepositModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;
        private readonly INotyfService _notyfService;

        public DepositModel(ApplicationDbContext context, IAccountService accountService, INotyfService notyfService)
        {
            _context = context;
            _accountService = accountService;
            _notyfService = notyfService;
        }

        [BindProperty] public int AccountId { get; set; }
        [BindProperty]public int CustomerId { get; set; }
        [BindProperty] public decimal Amount { get; set; }
        public Account Account { get; set; }
        public Customer Customer { get; set; }

        public void OnGet(int accountId, int customerId)
        {
            AccountId = accountId;
            CustomerId = customerId;
        }

        public IActionResult OnPost(int accountId, int customerId)
        {
            if (!ModelState.IsValid) return Page();
            Customer = _context.Customers
                .First(c => c.Id == customerId);

            Account = _context.Accounts
                .Include(t => t.Transactions)
                .First(a => a.Id == accountId);

            var status = _accountService.MakeDeposit(accountId, Amount);

            _notyfService.Success("Deposit Complete!");

            switch (status)
            {
                case IAccountService.ErrorCode.AmountIsNegative:
                    ModelState.AddModelError(nameof(Amount),
                        "Insufficient funds");

                    return Page();
                case IAccountService.ErrorCode.BalanceIsToLow:
                    ModelState.AddModelError(nameof(Amount),
                        "Your current balance is to low.");

                    return Page();
                case IAccountService.ErrorCode.ok:
                    return RedirectToPage("/AccountManager/TransactionList", new {customerId});
                default:
                    return Page();
            }
        }
    }
}
