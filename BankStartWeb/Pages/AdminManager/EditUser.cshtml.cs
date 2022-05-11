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
    public class EditUserModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotyfService _notyfService;

        public EditUserModel(UserManager<IdentityUser> userManager, INotyfService notyfService)
        {
            _userManager = userManager;
            _notyfService = notyfService;
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)] public string Password { get; set; }
        public List<string> Roles { get; set; }
        public List<SelectListItem> AllRoles { get; set; }

        public void OnGet(string userId)
        {
            UserId = userId;
            var user = _userManager.Users.First(u => u.Id == userId);
            UserName = user.UserName;
            Email = user.Email;
            Password = user.PasswordHash;

            SetRoles();
        }

        public IActionResult OnPost(string userId)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.First(u => u.Id == userId);
                {
                    _userManager.RemoveFromRolesAsync(user, Roles).Wait();
                    user.UserName = UserName;
                    user.Email = Email;
                    user.PasswordHash = Password;
                    _userManager.AddToRolesAsync(user, Roles).Wait();
                    _userManager.UpdateAsync(user).Wait();
                }

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
