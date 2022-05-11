using AspNetCoreHero.ToastNotification.Abstractions;
using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankStartWeb.Pages.CustomerManager
{
    [Authorize(Roles = "Admin, Cashier")]
    public class AddAccountModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;
        private readonly INotyfService _notyfService;

        public AddAccountModel(ApplicationDbContext context, IAccountService accountService, INotyfService notyfService)
        {
            _context = context;
            _accountService = accountService;
            _notyfService = notyfService;
        }
        [BindProperty] public string AccountType { get; set; }
        [BindProperty] public int CustomerId { get; set; }
        public List<SelectListItem> SetAccountTypes { get; set; }

        public void OnGet(int customerId)
        {
            CustomerId = customerId;

            SetAllAccounts();
        }

        private void SetAllAccounts()
        {
            SetAccountTypes = _context.Accounts.Select(a => a.AccountType).Distinct().Select(accountType => new SelectListItem
            {
                Text = accountType,
                Value = accountType

            }).ToList();
        }

        public IActionResult OnPost()
        {
            var customerId = CustomerId;
            var accountType = AccountType;

            if (ModelState.IsValid)
            {
                _accountService.AddAccount(customerId, accountType);

                _notyfService.Success("Success!");

                return RedirectToPage("/CustomerManager/Detail", new {id = customerId});
            }

            SetAllAccounts();

            _notyfService.Error("Failed!");

            return Page();
        }


    }
}
