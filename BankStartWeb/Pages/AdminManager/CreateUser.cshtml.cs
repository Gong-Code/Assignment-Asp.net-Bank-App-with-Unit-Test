using System.ComponentModel.DataAnnotations;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankStartWeb.Pages.AdminManager
{
    [BindProperties]
    [Authorize(Roles = "Admin")]
    public class CreateUserModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotyfService _notyfService;

        public CreateUserModel(UserManager<IdentityUser> userManager, INotyfService notyfService)
        {
            _userManager = userManager;
            _notyfService = notyfService;
        }
        public string UserName { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)] public string Password { get; set; }
        public List<string> Roles { get; set; }
        public List<SelectListItem> AllRoles { get; set; } 


        public void OnGet()
        {
            SetRoles();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                var user = new IdentityUser();
                {
                    user.UserName = UserName;
                    user.Email = Email;
                    user.PasswordHash = Password;
                    user.EmailConfirmed = true;
                };

                _userManager.CreateAsync(user, Password).Wait();
                _userManager.AddToRolesAsync(user, Roles).Wait();

                _notyfService.Success("Success!");
                
                return RedirectToPage("/AdminManager/UserList");
            }

            SetRoles();
            return Page();
        }

        public void SetRoles()
        {
            AllRoles = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "Cashier",
                    Text = "Cashier"
                },
                new SelectListItem()
                {
                    Value ="Admin",
                    Text = "Admin"
                }
            };

        }
    }
}
