using System.ComponentModel.DataAnnotations;
using BankStartWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.CustomerManager
{
    [Authorize(Roles = "Admin, Cashier")]
    [BindProperties]
    public class DetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Id { get; set; }
        [MaxLength(50)] public string Givenname { get; set; }
        [MaxLength(50)] public string Surname { get; set; }
        [MaxLength(50)] public string Streetaddress { get; set; }
        [MaxLength(50)] public string City { get; set; }
        [MaxLength(10)] public string Zipcode { get; set; }
        [MaxLength(30)] public string Country { get; set; }
        [MaxLength(2)] public string CountryCode { get; set; }
        [MaxLength(20)] public string NationalId { get; set; }
        [Range(0, 9999)] public int TelephoneCountryCode { get; set; }
        public string Telephone { get; set; }
        [MaxLength(50)] public string EmailAddress { get; set; }
        public DateTime Birthday { get; set; }

        public List<AccountViewModel> Accounts { get; set; }
        public decimal TotalBalance { get; set; }
        public class AccountViewModel
        {
            public int AccountId { get; set; }
            public string AccountType { get; set; }
            public DateTime Created { get; set; }
            public decimal Balance { get; set; }
            
        }

        public void OnGet(int id)
        {
            var accountInfo = _context.Customers
                .Include(c => c.Accounts)
                .First(customer => customer.Id == id);

            Id = accountInfo.Id;
            Givenname = accountInfo.Givenname;
            Surname = accountInfo.Surname;
            Streetaddress = accountInfo.Streetaddress;
            City = accountInfo.City;
            Zipcode = accountInfo.Zipcode;
            Country = accountInfo.Country;
            CountryCode = accountInfo.CountryCode;
            NationalId = accountInfo.NationalId;
            TelephoneCountryCode = accountInfo.TelephoneCountryCode;
            Telephone = accountInfo.Telephone;
            EmailAddress = accountInfo.EmailAddress;
            Birthday = accountInfo.Birthday;

            Accounts = accountInfo.Accounts.Select(account => new AccountViewModel
            {
                AccountId = account.Id,
                AccountType = account.AccountType,
                Created = account.Created,
                Balance = account.Balance,

            }).ToList();

            TotalBalance = Accounts.Sum(s => s.Balance);

        }
    }
}
