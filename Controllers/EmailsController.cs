using Microsoft.AspNetCore.Mvc;
using HR.Services;
using HR.DataModels;
using HR.Utils;

namespace HR.Controllers
{
    public class EmailsController : Controller
    {
        private readonly EmailService _emailService;

        public EmailsController(EmailService emailService)
        {
            _emailService = emailService;
        }

        // GET: Email/Index
        /// <summary>
        /// Displays the email form.
        /// </summary>
        /// <returns>Returns the view for the email form.</returns>
        public ActionResult Index()
        {
            return View();
        }

        // POST: Email/Index
        /// <summary>
        /// Handles the submission of the email form, sends an email with optional attachment.
        /// </summary>
        /// <param name="model">The email model containing recipient, subject, body, and attachment.</param>
        /// <returns>Redirects to the Candidates index action after sending the email.</returns>
        [HttpPost]
        public IActionResult Index(IEmail model)
        {
            string errorMessage;
            if (_emailService.SendEmail(model, out errorMessage))
            {
                ViewBag.Message = Constants.EMAIL.EmailSentSuccess;
            }
            else
            {
                ViewBag.Message = string.Format(Constants.InternalServerError, errorMessage);
            }

            return RedirectToAction("Index", "Candidates");
        }
    }
}
