using EventManagementWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EventManagementWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // Action Method
        public async Task<IActionResult> Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                // Get the current logged in user details
                var user = await userManager.GetUserAsync(User);

                // Check if the current logged in user has not been assigned any role
                if (!await userManager.IsInRoleAsync(user, "User") && !await userManager.IsInRoleAsync(user, "Admin"))
                {
                    // Assign this user the role User
                    await userManager.AddToRoleAsync(user, "User");

                    // Refresh the authentication cookie so the new role takes effect
                    await signInManager.RefreshSignInAsync(user);
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
