using System.ComponentModel.DataAnnotations;
using AspNetCoreHero.ToastNotification.Abstractions;
using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankStartWeb.Pages.BankManager
{
    [BindProperties]
    public class NewModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public NewModel(ApplicationDbContext context, INotyfService notyf)
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

        public string CountryCode { get; set; }
        [MaxLength(20)] public string NationalId { get; set; }
        
        [Range(0, 9999)] public int TelephoneCountryCode { get; set; }
        public string Telephone { get; set; }
        
        [MaxLength(50)] public string EmailAddress { get; set; }
        
        [DataType(DataType.Date)] public DateTime Birthday { get; set; }

        public List<SelectListItem> AllCountries { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AllCountriesCode { get; set; } = new List<SelectListItem>();


        public void SetAllCountries()
        {
            AllCountries.Add(new SelectListItem("Sweden", "SE"));
            AllCountries.Add(new SelectListItem("Norway", "NO"));
            AllCountries.Add(new SelectListItem("Finland", "FI"));

            AllCountriesCode.Add(new SelectListItem("SE", "Sweden"));
            AllCountriesCode.Add(new SelectListItem("NO", "Norway"));
            AllCountriesCode.Add(new SelectListItem("FI", "Finland")); 
        }
        public void OnGet()
        {

            SetAllCountries();
        }


        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Givenname = Givenname,
                    Surname = Surname,
                    Streetaddress = Streetaddress,
                    City = City,
                    Zipcode = Zipcode,
                    Country = Country,
                    CountryCode = CountryCode,
                    NationalId = NationalId,
                    TelephoneCountryCode = TelephoneCountryCode,
                    Telephone = Telephone,
                    EmailAddress = EmailAddress,
                    Birthday = Birthday
                };

                _context.Customers.Add(customer);
                _context.SaveChanges();

                _notyf.Success("Successfully added customer!");

                return RedirectToPage("/Index");
            }

            
            _notyf.Error("Fail to add customer, Please try again.");

            return Page();


        }
    }
}
