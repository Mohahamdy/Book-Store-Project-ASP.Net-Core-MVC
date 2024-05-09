using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Mapper;
using Project.Models;
using Project.Repositories;
using Project.ViewModels;
using System.Security.Claims;

namespace Project.Controllers
{
    public class ProfileController : Controller
    {
        private readonly BookStoreContext bookStore;
        private readonly IMapper _mapper;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ProfileController(BookStoreContext bookStore, IMapper _mapper,
            IUserProfileRepository userProfileRepository, UserManager<ApplicationUser> userManager
            , IWebHostEnvironment environment,
            SignInManager<ApplicationUser> signInManager
            )
        {
            this.bookStore = bookStore;
            this._mapper = _mapper;
            this.userProfileRepository = userProfileRepository;
            this.userManager = userManager;
            this.environment = environment;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {



            UserDetails userInfo = userProfileRepository.UserDetails(getUserID());
            //return Json(result);
            return View("Profile", userInfo);
            //return Json(userInfo);
        }

        [HttpGet]
        public IActionResult Edit()
        {


            UserDetails userInfo = userProfileRepository.UserDetails(getUserID());

            return View("EditTabs", userInfo);
            //return Json(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDetails newUserDetails)
        {



            var user = await userManager.FindByIdAsync(getUserID());

            if (user == null)
            {
                return NotFound();
            }





            // Update user properties
            user.FirstName = newUserDetails.FirstName;
            user.LastName = newUserDetails.LastName;

            user.Address = newUserDetails.Address;
            user.PhoneNumber = newUserDetails.PhoneNumber;



            // Save changes to the database
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Update successful
                return RedirectToAction("Index");
            }
            else
            {
                // Update failed
                // Handle errors here
                return BadRequest(result.Errors);
            }
            //return Json(newUserDetails);
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(IFormFile image, string oldImage)
        {
            var user = await userManager.FindByIdAsync(getUserID());

            if (user == null)
            {
                return NotFound();
            }

            string fileName = "";
            if (image != null && image.Length > 0)
            {
                // Define a unique filename for the image
                fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(image.FileName)}";
                var oldImagePath = Path.Combine(environment.WebRootPath, "images/users/", oldImage);
                var filePath = Path.Combine(environment.WebRootPath, "images/users/", fileName);
                DeleteImageFile(oldImagePath);
                // Save the image to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                // Update the user's profile image reference
                user.image = fileName;



                //Updating Image Claim 
                // Update or add the profile image claim
                await userManager.RemoveClaimAsync(user, new Claim("image", oldImage));
                await userManager.AddClaimAsync(user, new Claim("image", user.image));


                // Save changes to the database
                var resultOfUpdating = await userManager.UpdateAsync(user);
                if (!resultOfUpdating.Succeeded)
                {
                    // Handle update failure
                    return BadRequest(resultOfUpdating.Errors);
                }
                // Sign out the user to refresh the authentication cookie with updated claims
                await signInManager.SignOutAsync();

                // Sign in the user again to re-issue the authentication cookie with updated claims
                await signInManager.SignInAsync(user, isPersistent: false);

            }
            return RedirectToAction("Index");
            //return Json(fileName);
        }
        private void DeleteImageFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        public IActionResult Billing()
        {
            return View("EditTabs");
        }
        public IActionResult Security()
        {
            return View("EditTabs");
        }

        public string getUserID()
        {
            Claim idclaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string id = idclaim.Value;

            return id;
        }
    }
}
