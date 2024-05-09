using Microsoft.AspNetCore.Mvc;
using Project.ViewModels;
using System.Net.Mail;
using System.Net;
using Project.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Project.Controllers
{
    public class ContactController : Controller
    {


        public ContactController()
        {

        }
        public IActionResult Index()
        {
            return View("ContactUs");
        }

        [HttpPost]


        [HttpPost]
        public IActionResult SendMessage(ContactVM Cont_vm)
        {


            try
            {
                if (ModelState.IsValid)
                {


                    MailMessage message = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient();
                    message.From = new MailAddress(Cont_vm.Email);  // the email which the user enterd in form 
                    message.To.Add("abdallah.shafiq49@gmail.com"); // the reciver 
                    message.Subject = Cont_vm.Subject;
                    message.IsBodyHtml = true;
                    message.Body = $"Name: {Cont_vm.Name}\nEmail: {Cont_vm.Email}\nPhone: {Cont_vm.Phone}\nMessage: {Cont_vm.Message}";



                    smtpClient.Port = 587;
                    smtpClient.Host = "sandbox.smtp.mailtrap.io";
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("c3e22046e96d1e", "bf4e2a33f13a4e");
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtpClient.Send(message);
                    ViewBag.isSent = true;
                    Cont_vm.IsSent = true;


                    return RedirectToRoute("ContactUs");
                }


            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
                ViewBag.isSent = false;
                Cont_vm.IsSent = false;


            }
            return View("ContactUs", Cont_vm);
        }
    }
}
