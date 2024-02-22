using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UsersApp.Data;
using UsersApp.Models;

namespace UsersApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public HomeController(ILogger<HomeController> logger, UserDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult UsersTable()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> Block(string userIds)
        {
            if (userIds != null)
            {

                List<string> UserIds = userIds.Split(',').ToList();
                if (UserIds == null || UserIds.Count == 0)
                {
                    return BadRequest("No users selected for blocking.");
                }
                var currentUser = await _userManager.GetUserAsync(User);

                // Perform blocking logic using userIds
                foreach (var userId in UserIds)
                {
                    var user = await _context.Users.FindAsync(userId);
                    if (user != null)
                    {
                        user.IsActive = false;
                    }
                }
                // Check if the current user is among those being blocked
                if (currentUser != null && UserIds.Contains(currentUser.Id))
                {
                    // Log out the current user
                    await _signInManager.SignOutAsync();

                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("UsersTable"); // Redirect to appropriate action after blocking
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(string userIds)
        {
            if (userIds != null)
            {
                List<string> UserIds = userIds.Split(',').ToList();
                if (UserIds == null || UserIds.Count == 0)
                {
                    return BadRequest("No users selected for unblocking.");
                }

                // Perform unblocking logic using userIds
                foreach (var userId in UserIds)
                {
                    var user = await _context.Users.FindAsync(userId);
                    if (user != null)
                    {
                        user.IsActive = true;
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("UsersTable"); // Redirect to appropriate action after unblocking
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userIds)
        {
            if (userIds != null)
            {
                List<string> UserIds = userIds.Split(',').ToList();
                if (UserIds == null || UserIds.Count == 0)
                {
                    return BadRequest("No users selected for deletion.");
                }

                // Perform deletion logic using userIds
                foreach (var userId in UserIds)
                {
                    var user = await _context.Users.FindAsync(userId);
                    if (user != null)
                    {
                        _context.Users.Remove(user);
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("UsersTable"); // Redirect to appropriate action after deletion
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
