using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.AccountManager
{
    [Authorize(Roles = "Admin, Cashier")]
    public class TransferModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public TransferModel(ApplicationDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        [BindProperty] public int AccountOne { get; set; }
        [BindProperty] public int AccountTwo { get; set; }
        public int CustomerId { get; set; }
        public string Operation { get; set; }
        [BindProperty]public decimal Amount { get; set; }
        public Customer Customer { get; set; }
        public List<Account> Accounts { get; set; }

        public List<SelectListItem> AllAccounts { get; set; }

        public void OnGet( int customerId)
        {
            Customer = _context.Customers
                .Include(a => a.Accounts)
                .First(c => c.Id == customerId);
            
            Accounts = Customer.Accounts.Select(a => new Account
            {
                Id = a.Id,

            }).ToList();

            CustomerId = customerId;
            SetAllAccounts();

        }

        public void SetAllAccounts()
        {
            var customer = _context.Customers
                .Include(a => a.Accounts)
                .First(c => c.Id == CustomerId);

            AllAccounts = customer.Accounts.Select(a => new SelectListItem
            {
                Text = a.AccountType + " " + a.Balance + " $",
                Value = a.Id.ToString()

            }).ToList();
        }

        


        public IActionResult OnPost(int customerId)
        {
            var accountOne = AccountOne;
            var accountTwo = AccountTwo;
            var amount = Amount;
            if (ModelState.IsValid)
            {
                Customer = _context.Customers.First(c => c.Id == customerId);
                var status = _accountService.Transfer(accountOne, accountTwo, amount);
                
                switch (status)
                {
                    case IAccountService.ErrorCode.InSufficientFunds:
                        ModelState.AddModelError(nameof(Amount),
                            "Insufficient funds");
                    
                        SetAllAccounts();
                        return Page();
                    case IAccountService.ErrorCode.AmountIsNegative:
                        ModelState.AddModelError(nameof(Amount),
                            "You cannot transfer a negative amount.");
                    
                        SetAllAccounts();
                        return Page();
                    default:
                        return RedirectToPage("/AccountManager/TransactionList", new { customerId });
                }
            }

            SetAllAccounts();
            return Page();
        }
    }
}
