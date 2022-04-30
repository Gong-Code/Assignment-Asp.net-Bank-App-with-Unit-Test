using System.ComponentModel.DataAnnotations;
using AspNetCoreHero.ToastNotification.Abstractions;
using BankStartWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankStartWeb.Pages.CustomerManager
{
    [Authorize(Roles = "Admin, Cashier")]
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public EditModel(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
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
        [Range(0, 9999)]
        public int TelephoneCountryCode { get; set; }
        public string Telephone { get; set; }
        [MaxLength(50)]
        public string EmailAddress { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        public List<SelectListItem> AllCountries { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AllCountriesCode { get; set; } = new List<SelectListItem>();

        public void OnGet(int customerId)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);

            Givenname = customer.Givenname;
            Surname = customer.Surname;
            Streetaddress = customer.Streetaddress;
            City = customer.City;
            Zipcode = customer.Zipcode;
            Country = customer.Country;
            CountryCode = customer.CountryCode;
            NationalId = customer.NationalId;
            Telephone = customer.Telephone;
            TelephoneCountryCode = customer.TelephoneCountryCode;
            EmailAddress = customer.EmailAddress;
            Birthday = customer.Birthday;

            AllCountries.Add(new SelectListItem("Sweden", "SE"));
            AllCountries.Add(new SelectListItem("Norway", "NO"));
            AllCountries.Add(new SelectListItem("Finland", "FI"));

            AllCountriesCode.Add(new SelectListItem("SE", "Sweden"));
            AllCountriesCode.Add(new SelectListItem("NO", "Norway"));
            AllCountriesCode.Add(new SelectListItem("FI", "Finland"));
        }

        public IActionResult OnPost(int customerId)
        {
            if (ModelState.IsValid)
            {
                var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);

                customer.Givenname = Givenname;
                customer.Surname = Surname;
                customer.Streetaddress = Streetaddress;
                customer.City = City;
                customer.Zipcode = Zipcode;
                customer.Country = Country;
                customer.CountryCode = CountryCode;
                customer.NationalId = NationalId;
                customer.Telephone = Telephone;
                customer.TelephoneCountryCode = TelephoneCountryCode;
                customer.EmailAddress = EmailAddress;
                customer.Birthday = Birthday;

                _context.SaveChanges();

                _notyf.Success("Success!");

                return RedirectToPage("/BankManager/Customers");
            }

            return Page();
        }
    }
}
