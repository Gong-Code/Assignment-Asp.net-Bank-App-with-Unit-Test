using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankStartWeb.Pages.AdminManager
{
    [Authorize(Roles = "Admin")]
    public class UserListModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserListModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public List<UserViewModel> Users { get; set; }

        public class UserViewModel
        {
            public string Id { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public IList<string> UserRoles { get; set; }
        }

        public void OnGet(string userId)
        {
            Users = _userManager.Users.Select(user => new UserViewModel()
            {
                UserId = userId,
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                UserRoles = _userManager.GetRolesAsync(user).Result

            }).ToList();
        }
    }

}
