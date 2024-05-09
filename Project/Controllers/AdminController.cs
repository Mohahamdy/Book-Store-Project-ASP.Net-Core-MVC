using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AdminController(BookStoreContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var adminRoleId = _context.AspNetRoles
                                      .Where(r => r.Name == "ADMIN")
                                      .Select(r => r.Id)
                                      .FirstOrDefault();

            if (adminRoleId != null)
            {
                var adminUserIds = _context.AspNetUserRoles
                                            .Where(ur => ur.RoleId == adminRoleId)
                                            .Select(ur => ur.UserId)
                                            .ToList();

                var adminUsers = _context.AspNetUsers
                                         .Where(u => adminUserIds.Contains(u.Id))
                                         .ToList();

                return View("GetAll", adminUsers);
            }

            return View("Error");
        }

        public IActionResult EditAdmin(int id)
        {
            var admin = _context.AspNetUsers.FirstOrDefault(u => u.Id == id);

            if (admin == null)
            {
                return NotFound();
            }

            return View("EditAdmin", admin);
        }

        [HttpPost]
        public IActionResult SaveAdmin(ApplicationUser admin)
        {
            var existingAdmin = _context.AspNetUsers.FirstOrDefault(u => u.Id == admin.Id);

            if (existingAdmin == null)
            {
                return NotFound();
            }

            existingAdmin.FirstName = admin.FirstName;
            existingAdmin.LastName = admin.LastName;
            existingAdmin.Email = admin.Email;

            _context.SaveChanges();

            return RedirectToAction("GetAll");
        }

        public IActionResult DeleteAdmin(int id)
        {
            var admin = _context.AspNetUsers.FirstOrDefault(u => u.Id == id);

            if (admin == null)
            {
                return NotFound();
            }

            return View("DeleteAdmin", admin);
        }

        [HttpPost]
        public IActionResult ConfirmDeleteAdmin(int id)
        {
            var admin = _context.AspNetUsers.FirstOrDefault(u => u.Id == id);

            if (admin == null)
            {
                return NotFound();
            }

            var orderIds = _context.Orders.Where(o => o.user_id == id).Select(o => o.ID).ToList();
            var orderDetails = _context.OrdersDetails.Where(od => orderIds.Contains(od.Order_id));
            _context.OrdersDetails.RemoveRange(orderDetails);

            var orders = _context.Orders.Where(o => o.user_id == id);
            _context.Orders.RemoveRange(orders);

            var comments = _context.Comments.Where(c => c.user_id == id);
            _context.Comments.RemoveRange(comments);

            var userRoles = _context.AspNetUserRoles.Where(ur => ur.UserId == id);
            _context.AspNetUserRoles.RemoveRange(userRoles);

            var books = _context.Books.Where(b => b.Admin_id == id);
            _context.Books.RemoveRange(books);

            _context.AspNetUsers.Remove(admin);
            _context.SaveChanges();

            return RedirectToAction("GetAll");
        }

        public IActionResult AddAdmin()
        {
            return View("AddAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin(ApplicationUserViewModel adminViewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if the role "ADMIN" exists, if not, create it
                if (!await _roleManager.RoleExistsAsync("ADMIN"))
                {
                    var adminRole = new IdentityRole<int>("ADMIN");
                    await _roleManager.CreateAsync(adminRole);
                }

                var admin = new ApplicationUser
                {
                    FirstName = adminViewModel.FirstName,
                    LastName = adminViewModel.LastName,
                    PhoneNumber = adminViewModel.PhoneNumber,
                    Address = adminViewModel.Address,
                    Email = adminViewModel.Email,
                    UserName = adminViewModel.Email
                };

                var result = await _userManager.CreateAsync(admin, adminViewModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "ADMIN");

                    return RedirectToAction("GetAll");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("AddAdmin", adminViewModel);
        }
    }
}
