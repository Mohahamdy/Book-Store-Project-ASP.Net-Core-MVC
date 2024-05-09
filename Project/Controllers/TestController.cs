using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Models;
using Project.Repositories;
using Project.ViewModels;
using System.Security.Claims;

namespace Project.Controllers
{
    public class TestController : Controller
    {
        BookStoreContext db;
        UserManager<ApplicationUser> userManager;
        IBookRepository bookRepository;
        public TestController(BookStoreContext db, IBookRepository bookRepository, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.bookRepository = bookRepository;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var comments = db.Comments.Where(i => i.user_id == 5).Select(i => i.user.FirstName).ToList();
            string name = "";
            foreach (var comment in comments)
            {
                name += comment;
            }
            return Content(name);
        }

        public IActionResult BookDetail()
        {
            //BookDetailsVM bookDetail = bookRepository.GetBookDetails(6);
            return View("BookDetail");
        }

        public async Task<IActionResult> Test(string email)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(email);
            return Json(user);
        }

        public async Task<IActionResult> test1(int id)
        {
            return Json(userManager.FindByIdAsync(id.ToString()));
        }

        public ActionResult Cart()
        {
            // Extract cart details from JSON data
            //var cartDetails = data["cart"];

            // Process cart details (e.g., save to database, generate cart claims, etc.)
            // Note: Replace this with your actual processing logic

            return Json("1");
        }

        public ActionResult AddClaims()
        {

            // Create a claim with a custom claim type and value
            var claim = new Claim("User", "1");

            // Retrieve the current user's identity
            var identity = (ClaimsIdentity)User.Identity;

            // Add the claim to the user's identity
            identity.AddClaim(claim);

            var claimsJson = JsonConvert.SerializeObject(identity.Claims.Select(c => new { Type = c.Type, Value = c.Value }));
            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                // Get the value of the user ID claim
                var userId = userIdClaim.Value;

                return Json(userId);

            }
            else
            {
                return Content("User ID not found.");
            }
            // Set the expiration time as a custom claim

        }

    }
}
