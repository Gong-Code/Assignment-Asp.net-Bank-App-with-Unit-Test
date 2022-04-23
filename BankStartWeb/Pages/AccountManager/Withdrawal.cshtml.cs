using System.ComponentModel.DataAnnotations;
using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        [Range(1, 5000)]
        public decimal Amount { get; set; }
        public string Type { get; set; }

        public void OnGet(int id)
        {
            var account = _context.Accounts
                .First(a => a.Id == id);

            id = account.Id;
        }

        public IActionResult OnPost(int id, decimal amount, string type)
        {
            if (ModelState.IsValid)
            {
                var status = _accountService.MakeWithdrawal(id, amount, type);

                if (status == IAccountService.ErrorCode.ok)
                {
                    return RedirectToPage("TransactionList", new {Id = id});
                }
            }

            return Page();
        }
    }
}
