using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mini_AMS.Pages.Admin
{
    [Authorize]
    public class AssignRolesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AssignRolesModel(UserManager<IdentityUser> userManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string RoleName { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            // পেজ খোলার সময় এখানে আর কিছুর দরকার নেই
        }

        // ✅ ডিফল্ট রোল তৈরি করার অ্যাকশন
        public async Task<IActionResult> OnPostCreateRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("Accountant"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Accountant"));
            } 
            if (!await _roleManager.RoleExistsAsync("Viewer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Viewer"));
            }

            Message = "Roles Created Successfully (Admin, Accountant, Viewer)";
            return Page();
        }

        // ✅ ইউজারের সাথে Role অ্যাসাইন করার অ্যাকশন
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(RoleName))
            {
                Message = "Email and Role Name must be provided.";
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                Message = $"User with email {Email} not found.";
                return Page();
            }

            if (!await _roleManager.RoleExistsAsync(RoleName))
            {
                Message = $"Role {RoleName} does not exist.";
                return Page();
            }

            var result = await _userManager.AddToRoleAsync(user, RoleName);
            if (result.Succeeded)
            {
                Message = $"User {Email} added to role {RoleName}.";
            }
            else
            {
                Message = "Error adding role.";
            }

            return Page();
        }
    }
}
