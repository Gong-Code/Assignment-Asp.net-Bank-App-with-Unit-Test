using System.ComponentModel.DataAnnotations;
using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.BankManager
{
    public class TransactionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TransactionsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public string SortOrder { get; set; }
        public string SortCol { get; set; }
        public int PageNum { get; set; }
        public int TotalPageCount { get; set; }
        public string SearchWord { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }
        public class TransactionViewModel
        {
            public int Id { get; set; }

            [MaxLength(10)]
            public string Type { get; set; }
            [MaxLength(50)]
            public string Operation { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public decimal NewBalance { get; set; }
        }

        public void OnGet(string searchWord, string col = "Id", string order = "asc", int pageNum = 1)
        {
            PageNum = pageNum;
            SearchWord = searchWord;
            SortCol = col;
            SortOrder = order;
            SearchWord = searchWord;

            var search = _context.Transactions.AsQueryable();

            int.TryParse(SearchWord, out var searchId);
            if (!string.IsNullOrEmpty(SearchWord))
                search = search.Where(s =>
                    s.Type.Contains(SearchWord) || s.Operation.Contains(SearchWord) || s.Id.Equals(searchId));

            search = search.OrderBy(col,
                order == "asc" ? ExtensionMethods.QuerySortOrder.Asc : ExtensionMethods.QuerySortOrder.Desc);

            var pageResult = search.GetPaged(PageNum, 20);
            TotalPageCount = pageResult.PageCount;

            Transactions = pageResult.Results.Select(c => new TransactionViewModel
            {
                Id = c.Id,
                Type = c.Type,
                Operation = c.Operation,
                Date = c.Date,
                Amount = c.Amount,
                NewBalance = c.NewBalance
                
            })
            .ToList();
        }
    }

}
