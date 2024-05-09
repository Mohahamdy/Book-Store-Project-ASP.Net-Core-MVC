using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Repositories;
using Project.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;




namespace Project.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IBookRepository bookRepository;
        private readonly BookStoreContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISenderEmail emailSender;
        public OrderController(ILogger<OrderController> logger, IBookRepository bookRepository, BookStoreContext db, UserManager<ApplicationUser> userManager, ISenderEmail emailSender)
        {
            _logger = logger;
            this.bookRepository = bookRepository;
            this.db = db;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.emailSender = emailSender;
        }




        [HttpGet]
        public IActionResult BookDetails(int id)
        {
            Book book = bookRepository.GetById(id);
            var comments = db.Comments.Where(x => x.book_id == book.ID)
                .Select(b => new CommentVM { comment = b.comment, Date = b.Date, rate = b.rate, userFName = b.user.FirstName, userLName = b.user.LastName }).ToList();


            decimal discountedPrice = book.Price;
            if (book.Discount != null)
            {
                discountedPrice = book.Price - (book.Price * (book.Discount.Percantage / 100));
            }

            BookDetailsVM bookvm = new BookDetailsVM()
            {
                ID = book.ID,
                Name = book.Name,
                Description = book.Description,
                Price = discountedPrice,
                Rate = book.Rate,
                Image = book.Image,
                Quantity = book.Quantity,
                categoryID = book.Category_id,
                commentsNum = comments.Count,
                Author = db.Authors.FirstOrDefault(x => x.ID == book.Author_id),
                Category = db.Categories.FirstOrDefault(x => x.ID == book.Category_id),
                Discount = db.Discounts.FirstOrDefault(x => x.ID == book.Discount_id),
                authorBooks = db.Books.Where(x => x.Author_id == book.Author_id && x.ID != book.ID).Select(x => new BookDetailsVM { ID = x.ID, Name = x.Name, Price = x.Price, Rate = x.Rate, Image = x.Image, Quantity = x.Quantity, Category = x.Category, commentsNum = db.Comments.Where(s => s.book_id == x.ID).Count() }).ToList(),
                categoryBooks = db.Books.Where(x => x.Category_id == book.Category_id && x.ID != book.ID).Select(x => new BookDetailsVM { ID = x.ID, Name = x.Name, Price = x.Price, Rate = x.Rate, Image = x.Image, Quantity = x.Quantity, Author = x.Author, commentsNum = db.Comments.Where(s => s.book_id == x.ID).Count() }).ToList(),
                Comments = comments
            };

            return View("BookDetails", bookvm);
        }



        [HttpPost]
        public IActionResult AddToCart(BookDetailsVM order, int quantity, decimal price)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("ConfirmOrder", "Order") });
            }

            var userId = userManager.GetUserId(User);

            if (!int.TryParse(userId, out int userIdInt))
            {
                return RedirectToAction("Error");
            }


            var existingOrder = db.Orders.Include(o => o.OrderDetails)
                                        .FirstOrDefault(o => o.user_id == userIdInt && o.OrderDetails.Any(od => od.Book_id == order.ID));

            if (existingOrder != null)
            {
                // Update the quantity of the existing order detail
                var existingOrderDetail = existingOrder.OrderDetails.FirstOrDefault(od => od.Book_id == order.ID);
                existingOrderDetail.Quantity = quantity;
                existingOrderDetail.Sub_total = (order.Price * quantity);
                existingOrder.Total_Price = (order.Price * quantity);
            }
            else
            {
                // Create a new order
                var newOrder = new Order
                {
                    Date = DateTime.Now,
                    Time = DateTime.Now.TimeOfDay,

                    Total_Price = order.Price * quantity,
                    user_id = userIdInt
                };


                var orderDetails = new OrderDetails
                {
                    order = newOrder,
                    Book_id = order.ID,
                    Quantity = quantity,
                    Sub_total = order.Price * quantity
                };

                db.OrdersDetails.Add(orderDetails);
            }

            db.SaveChanges();
            Thread.Sleep(2000);
            return RedirectToAction("BookDetails", "Home", new { id = order.ID });
        }



        [HttpGet]
        public IActionResult OrderSummary()
        {
            var orderDetails = db.OrdersDetails
                .Include(od => od.book)
                .ToList();

            var bookDetailsVMs = orderDetails.Select((od, index) =>
            {
                try
                {
                    var bookDetailsVM = new BookDetailsVM
                    {
                        ID = od.Order_id,
                        Name = od.book != null ? od.book.Name : "N/A",
                        // Fetch price from the Book entity
                        Price = od.book.Price,
                        Quantity = od.Quantity,
                        Image = od.book != null ? od.book.Image : null
                    };

                    return bookDetailsVM;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error processing item at index {index}: {ex.Message}", ex);
                }
            }).ToList();

            return View(bookDetailsVMs);
        }






        [HttpPost]
        public IActionResult DeleteOrder(int id)
        {
            var orderDetailToDelete = db.OrdersDetails.FirstOrDefault(od => od.Order_id == id);
            if (orderDetailToDelete != null)
            {
                db.OrdersDetails.Remove(orderDetailToDelete);
                var result = db.SaveChanges();
                if (result > 0)
                {
                    return Json(result);

                }
                else
                {
                    return Json("false");

                }
            }
            return Json(orderDetailToDelete);

            //return RedirectToAction("OrderSummary");
        }





        [HttpPost]
        public IActionResult ConfirmOrder()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("ConfirmOrder", "Order") });
            }

            var userId = userManager.GetUserId(User);

            if (!int.TryParse(userId, out int userIdInt))
            {
                return RedirectToAction("Error");
            }

            var orderDetails = db.OrdersDetails
                                .Include(od => od.book)
                                .Where(od => od.order.user_id == userIdInt)
                                .ToList();

            if (orderDetails.Any())
            {
                decimal totalPrice = orderDetails.Sum(od => od.Sub_total);

                var newOrder = new Order
                {
                    Date = DateTime.Now,
                    Time = DateTime.Now.TimeOfDay,
                    Total_Price = totalPrice,
                    user_id = userIdInt
                };

                db.Orders.Add(newOrder);
                db.SaveChanges();

                foreach (var od in orderDetails)
                {
                    var orderDetailsForNewOrder = new OrderDetails
                    {
                        Order_id = newOrder.ID,
                        Book_id = od.Book_id,
                        Sub_total = od.Sub_total,
                        Quantity = od.Quantity
                    };

                    db.OrdersDetails.Add(orderDetailsForNewOrder);
                }

                db.SaveChanges();

                db.OrdersDetails.RemoveRange(orderDetails);
                db.SaveChanges();

                var userEmail = db.Users.FirstOrDefault(u => u.Id.ToString() == userId)?.Email;

                if (!string.IsNullOrEmpty(userEmail))
                {
                    SendOrderConfirmationEmail(userEmail, newOrder);
                }
            }

            return RedirectToAction("Thanks");
        }

        private void SendOrderConfirmationEmail(string userEmail, Order order)
        {
            string subject = "Order Confirmation";
            string body = "<h1>Your Order Details</h1>" +
                          "<p>Order ID: " + order.ID + "</p>" +
                          "<p>Total Price: $" + order.Total_Price + "</p>" +
                          "<p>Delivery Date: " + order.Date.AddDays(3).ToString("dddd, MMMM dd, yyyy") + "</p>" +
                          "<p>Thank you for your order!</p>";

            emailSender.SendEmailAsync(userEmail, subject, body, true);
        }

        public IActionResult Thanks()
        {
            Order order = new Order
            {
                Date = DateTime.Now.AddDays(3)
            };
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("ConfirmOrder", "Order") });
            }

            var userId = userManager.GetUserId(User);

            if (!int.TryParse(userId, out int userIdInt))
            {
                return RedirectToAction("Error");
            }

            var orderDetails = db.OrdersDetails
                                .Include(od => od.book)
                                .Where(od => od.order.user_id == userIdInt)
                                .ToList();

            db.RemoveRange(orderDetails);
            db.SaveChanges();

            return View("Thanks", order);
        }



    }
}
