using System.ComponentModel.DataAnnotations;
using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankStartWeb.Pages.AccountManager
{
    public class DepositModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public DepositModel(ApplicationDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        [Range(1, 5000)] 
        public decimal Amount { get; set; }
        public string Type { get; set; }

        public void OnGet(int id)
        {
            var account = _context.Accounts
                .First(a => a.Id == id);

            id = account.Id;
        }

        public IActionResult OnPost(int id, decimal amount)
        {
            if (ModelState.IsValid)
            {
                var status = _accountService.MakeDeposit(id, amount);

                if (status == IAccountService.ErrorCode.ok)
                {
                    return RedirectToPage("TransactionList", new {Id = id}); // Set property Id to the current id.
                }

                ModelState.AddModelError("amount", "invalid amount");
            }

            return Page();

        }
    }
}
