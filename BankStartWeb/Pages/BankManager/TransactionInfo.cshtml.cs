using System.ComponentModel.DataAnnotations;
using System.Linq;
using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.BankManager
{
    public class TransactionInfoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TransactionInfoModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<TransactionInfoViewModel> TransactionInfo { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public class TransactionInfoViewModel
        {
            [MaxLength(10)]
            public string? AccountType { get; set; }

            public DateTime Created { get; set; }
            public decimal Balance { get; set; }
        }

        public void OnGet(int id)
        {
            var customerInfo = _context.Customers
                .Include(customerInfo => customerInfo.Accounts)
                .First(cust => cust.Id == id);


            Id = customerInfo.Id;
            FirstName = customerInfo.Givenname;
            LastName = customerInfo.Surname;

            TransactionInfo = customerInfo.Accounts
                .Select(account => new TransactionInfoViewModel
                {
                    AccountType = account.AccountType,
                    Balance = account.Balance,
                })
                .ToList();

        }
    }

}
