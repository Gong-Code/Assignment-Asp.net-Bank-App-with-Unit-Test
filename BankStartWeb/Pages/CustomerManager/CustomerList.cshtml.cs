using System.ComponentModel.DataAnnotations;
using BankStartWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.CustomerManager
{
    [Authorize(Roles = "Admin, Cashier")]
    public class CustomerListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CustomerListModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<CustomerViewModel> Customers { get; set; }

        public string SortOrder { get; set; }
        public string SortCol { get; set; }
        public int PageNum { get; set; }
        public int TotalPageCount { get; set; }
        public string SearchWord { get; set; }

        public class CustomerViewModel
        {
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
            
        }

        public void OnGet(string searchWord, string col = "Id", string order = "asc", int pageNum = 1)
        {
            PageNum = pageNum;
            SearchWord = searchWord;
            SortCol = col;
            SortOrder = order;
            SearchWord = searchWord;

            var search = _context.Customers.Include(s => s.Accounts).AsQueryable();

            int.TryParse(SearchWord, out var searchId);
            if (!string.IsNullOrEmpty(SearchWord))
                search = search.Where(s =>
                    s.Givenname.Contains(SearchWord) || s.City.Contains(SearchWord) || s.Id.Equals(searchId));

            search = search.OrderBy(col,
                order == "asc" ? ExtensionMethods.QuerySortOrder.Asc : ExtensionMethods.QuerySortOrder.Desc);

            var pageResult = search.GetPaged(PageNum, 50);
            TotalPageCount = pageResult.PageCount;

            Customers = pageResult.Results.Select(c => new CustomerViewModel
            {
                Id = c.Id,
                Givenname = c.Givenname,
                Surname = c.Surname,
                Streetaddress = c.Streetaddress,
                City = c.City,
                Zipcode = c.Zipcode,
                Country = c.Country,
                CountryCode = c.CountryCode,
                NationalId = c.NationalId,
                TelephoneCountryCode = c.TelephoneCountryCode,
                Telephone = c.Telephone,
                EmailAddress = c.EmailAddress,
                Birthday = c.Birthday,

            }).ToList();
        }
    }

    
}
