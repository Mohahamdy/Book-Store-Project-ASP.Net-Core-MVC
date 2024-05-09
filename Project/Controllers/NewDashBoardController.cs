using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Repositories;
using Project.ViewModels;

namespace Project.Controllers
{
    public class NewDashBoardController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IAuthorRepository authorRepository;
        private readonly BookStoreContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private static VisitorCount visitorCount = new VisitorCount { Count = 0 };

        public NewDashBoardController(BookStoreContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment, IAuthorRepository authorRepository)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            this.authorRepository = authorRepository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            visitorCount.Count++;
            ViewBag.count = visitorCount.Count;
            ViewBag.BooksCount = _context.Books.ToList().Count;
            ViewBag.UsersCount = _context.Users.ToList().Count;
            ViewBag.OrdersCount = _context.Orders.ToList().Count;
            return View("index");
        }



        #region Get Admins and Users 
        public IActionResult GetAllUsers()
        {
            int? roleID = _context.AspNetRoles.FirstOrDefault(r => r.Name == "user")?.Id;
            if (roleID != null)
            {
                // Get the list of user IDs who have the specific role
                List<int> usersWithSpecificRoleID = _context.AspNetUserRoles
                                                    .Where(ur => ur.RoleId == roleID)
                                                    .Select(ur => ur.UserId)
                                                    .ToList();
                // Get the list of all user IDs
                List<int> allUserIDs = _context.AspNetUsers
                                            .Select(u => u.Id)
                                            .ToList();
                // Get the users who do not have the specific role
                List<ApplicationUser> usersWithoutSpecificRole = _context.AspNetUsers.Include(n => n.Orders)
                                                    .Where(u => usersWithSpecificRoleID.Contains(u.Id))
                                                    .ToList();
                return View("Users", usersWithoutSpecificRole);
            }
            return View("Error");

        }


        public IActionResult GetAllAdmins()
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

                return View("Admins", adminUsers);
            }

            return View("Error");
        }
        #endregion


        #region Add Admin
        [HttpGet]
        public IActionResult ModifyAdmin()

        {

            List<KeyValuePair<int, string>> users_id_name = new List<KeyValuePair<int, string>>();
            var users = _context.AspNetUsers.ToList();

            foreach (var user in users)
            {
                users_id_name.Add(new KeyValuePair<int, string>(user.Id, user.FirstName + " " + user.LastName));
            }

            return View("ModifyAdmin", users_id_name);
        }

        [HttpPost]
        public IActionResult ModifyAdmin(bool isAdmin, int UserID)
        {
            int adminRoleId = _context.Roles.Where(n => n.Name == "Admin").Select(n => n.Id).FirstOrDefault();
            int userRoleId = _context.Roles.Where(n => n.Name == "User").Select(n => n.Id).FirstOrDefault();

            var userRoles = _context.AspNetUserRoles.Where(ur => ur.UserId == UserID).ToList();

            // Remove the user from all current roles
            _context.AspNetUserRoles.RemoveRange(userRoles);

            if (isAdmin)
            {
                if (userRoleId != null)
                {
                    _context.AspNetUserRoles.Add(new IdentityUserRole<int> { UserId = UserID, RoleId = userRoleId });
                }
            }
            else
            {
                if (adminRoleId != null)
                {
                    _context.AspNetUserRoles.Add(new IdentityUserRole<int> { UserId = UserID, RoleId = adminRoleId });
                }
            }

            // Save changes to the database
            _context.SaveChanges();
            return RedirectToAction("ModifyAdmin");


        }

        public List<int> GetAdminsIDs()
        {
            var adminRoleId = _context.AspNetRoles
                                      .Where(r => r.Name == "Admin")
                                      .Select(r => r.Id)
                                      .FirstOrDefault();
            return _context.AspNetUserRoles.Where(ur => ur.RoleId == adminRoleId)
                                            .Select(ur => ur.UserId)
                                            .ToList();
        }




        public async Task<IActionResult> CheckAdminRole(int UserID)
        {

            //var user = await _context.Users.FindAsync(UserID);
            ApplicationUser user = await _context.Users.SingleOrDefaultAsync(s => s.Id == UserID);
            var isAdmin = await _context.UserRoles.AnyAsync(u => u.UserId == UserID && u.RoleId == 2);
            // Check if the user has the "Admin" role
            //var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            return Json(new { isAdmin });
        }


        #endregion


        #region Books
        public IActionResult Books()
        {
            var books = _context.Books.
                Select(x => new BookDetailsVM
                {
                    ID = x.ID,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Rate = x.Rate,
                    Image = x.Image,
                    Quantity = x.Quantity,
                    Author = x.Author,
                    Category = x.Category,
                    Discount = x.Discount,
                    Admin = x.Admin,
                    IsAvailable = x.IsAvailable
                }).ToList();

            return View("Books", books);
        }

        public IActionResult GetBookDeatils(int id)
        {
            var book = _context.Books.Where(x => x.ID == id).
                Select(x => new BookDetailsVM
                {
                    ID = x.ID,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Rate = x.Rate,
                    Image = x.Image,
                    Quantity = x.Quantity,
                    Author = x.Author,
                    Category = x.Category,
                    Discount = x.Discount,
                    Admin = x.Admin,
                    IsAvailable = x.IsAvailable
                }).FirstOrDefault();



            return Json(new
            {
                bookobj = book,
                author = book.Author.Name,
                category = book.Category.Name,
                admin = book.Admin.FirstName + " " + book.Admin.LastName,
                discount = book.Discount?.Percantage ?? null   //book.Discount?.Percentage checks if book.Discount is not null. If it's not null, it accesses the Percentage property.
            });
        }

        public IActionResult BookComments(int id)
        {
            List<CommentVM> comments = _context.Comments.Where(x => x.book_id == id).
                Select(x => new CommentVM
                {
                    comment = x.comment,
                    userFName = x.user.FirstName,
                    userLName = x.user.LastName,
                    rate = x.rate,
                    Date = x.Date,
                    IsAvailable = x.IsAvailable,
                    user_id = x.user_id,
                    book_id = x.book_id
                }).ToList();

            ViewBag.BookName = _context.Books.Where(x => x.ID == id).Select(x => x.Name).FirstOrDefault();


            return View("BookComments", comments);
        }



        public IActionResult DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(x => x.ID == id);
            book.IsAvailable = false;
            _context.Update(book);
            _context.SaveChanges();

            return Json("Done");
        }
        public IActionResult AddBook(int id)
        {
            var book = _context.Books.FirstOrDefault(x => x.ID == id);
            book.IsAvailable = true;
            _context.Update(book);
            _context.SaveChanges();

            return Json("Done");
        }

        public IActionResult EditBook(int id)
        {
            var adminsIds = GetAdminsIDs();
            BookVM bookVM = new BookVM()
            {
                book = _context.Books.FirstOrDefault(x => x.ID == id),
                authors = _context.Authors.ToList(),
                admins = _context.AspNetUsers.Where(u => adminsIds.Contains(u.Id)).ToList(),
                categories = _context.Categories.ToList(),
                discounts = _context.Discounts.ToList(),
            };

            return View("EditBook", bookVM);
        }

        public IActionResult AddNewBook()
        {
            var adminsIds = GetAdminsIDs();

            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Discounts = _context.Discounts.ToList();
            ViewBag.Admins = _context.AspNetUsers.Where(u => adminsIds.Contains(u.Id)).ToList();

            return View("AddNewBook");
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewBook(Book book, IFormFile? imageFile)
        {
            if (imageFile == null)
            {
                ModelState.AddModelError("Image", "Please select Image");
            }
            if (ModelState.IsValid == true)
            {
                try
                {
                    var fileName = "";
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Save the uploaded image to the wwwroot folder
                        var uploadsDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "assets", "Images", "books");
                        if (!Directory.Exists(uploadsDirectory))
                        {
                            Directory.CreateDirectory(uploadsDirectory);
                        }

                        // Generate a unique file name to avoid overwriting existing files
                        fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(uploadsDirectory, fileName);

                        // Save the file to the server
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                    }

                    book.Image = fileName;

                    _context.Books.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Books");
                }
                catch (Exception ex)
                {
                    //send ex message to view as error inside modelstate
                    //ModelState.AddModelError("DepartmentId", "Please Select Department");
                    //ModelState.AddModelError("", ex.Message);
                    // ModelState.AddModelError("", ex.InnerException.Message);
                }
            }

            var adminsIds = GetAdminsIDs();

            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Discounts = _context.Discounts.ToList();
            ViewBag.Admins = _context.AspNetUsers.Where(u => adminsIds.Contains(u.Id)).ToList();

            return View("AddNewBook", book);
        }
        [HttpPost]
        public async Task<IActionResult> EditBook(BookVM _book, IFormFile? imageFile)
        {
            Book book = _context.Books.FirstOrDefault(x => x.ID == _book.book.ID);
            if (ModelState.IsValid == true)
            {
                if (book != null)
                {
                    book.Name = _book.book.Name;
                    book.Description = _book.book.Description;
                    book.Price = _book.book.Price;
                    book.Quantity = _book.book.Quantity;
                    book.IsAvailable = _book.book.IsAvailable;
                    book.Discount_id = _book.book.Discount_id;
                    book.Admin_id = _book.book.Admin_id;
                    book.Author_id = _book.book.Author_id;
                    book.Category_id = _book.book.Category_id;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Save the uploaded image to the wwwroot folder
                        var uploadsDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "assets", "Images", "books");
                        if (!Directory.Exists(uploadsDirectory))
                        {
                            Directory.CreateDirectory(uploadsDirectory);
                        }

                        // Generate a unique file name to avoid overwriting existing files
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(uploadsDirectory, fileName);

                        // Save the file to the server
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Update the book's image file name in the database
                        book.Image = fileName; // Store only the file name in the database
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Books");
                }
            }
            else
            {
                var adminsIds = GetAdminsIDs();

                _book.authors = _context.Authors.ToList();
                _book.admins = _context.AspNetUsers.Where(u => adminsIds.Contains(u.Id)).ToList();
                _book.categories = _context.Categories.ToList();
                _book.discounts = _context.Discounts.ToList();
                return View("EditBook", _book);
            }
            return NotFound();
        }
        #endregion


        #region Categories


        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();

            return View("Categories", categories);
        }

        public IActionResult AddCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.ID == id);
            category.IsAvailable = true;
            _context.Update(category);
            _context.SaveChanges();

            return Json("Done");
        }

        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.ID == id);
            category.IsAvailable = false;
            _context.Update(category);
            _context.SaveChanges();

            return Json("Done");
        }

        public IActionResult EditCategory(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.ID == id);

            return View("EditCategory", category);
        }

        [HttpPost]
        public IActionResult EditCategory(Category _category)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.ID == _category.ID);

            if (ModelState.IsValid == true)
            {
                try
                {
                    if (category != null)
                    {
                        category.Name = _category.Name;
                        category.Description = _category.Description;
                        category.IsAvailable = _category.IsAvailable;

                        _context.Update(category);
                        _context.SaveChanges();

                        return RedirectToAction("Categories");
                    }
                }
                catch (Exception e)
                {
                    NotFound();
                }

            }
            return View("EditCategory", category);

        }

        public IActionResult AddNewCategory()
        {
            return View("AddNewCategory");
        }
        [HttpPost]
        public IActionResult SaveNewCategory(Category _category)
        {
            if (ModelState.IsValid == true)//C#
            {
                try
                {
                    _context.Categories.Add(_category);
                    _context.SaveChanges();
                    return RedirectToAction("Categories");
                }
                catch (Exception ex)
                {
                    //send ex message to view as error inside modelstate
                    //ModelState.AddModelError("DepartmentId", "Please Select Department");
                }
            }

            return View("AddNewCategory", _category);
        }
        #endregion

        #region Author

        [HttpGet]
        public IActionResult GetAllAuthors()
        {
            List<Author> authors = authorRepository.GetAll();
            return View(authors);
        }

        [HttpGet]
        public IActionResult AddAuthor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAuthor(Author author)
        {
            if (ModelState.IsValid == true)
            {
                authorRepository.Add(author);
                authorRepository.Save();

                return RedirectToAction("GetAllAuthors");
            }

            return View(author);
        }

        [HttpGet]
        public IActionResult EditAuthor(int id)
        {
            Author author = authorRepository.GetById(id);
            return View(author);
        }

        [HttpPost]
        public IActionResult EditAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                authorRepository.Update(author);
                authorRepository.Save();

                return RedirectToAction("GetAllAuthors");
            }

            return View(author);
        }

        [HttpGet]
        public IActionResult DeleteAuthor(int id)
        {
            try
            {
                authorRepository.DeleteByID(id);
                authorRepository.Save();

                return RedirectToAction("GetAllAuthors");
            }
            catch (Exception ex)
            {
                return Content("Can not Delte this author because he has books");
            }

        }

        #endregion






    }

}

