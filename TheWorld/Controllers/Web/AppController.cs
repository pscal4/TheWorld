using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.ViewModels;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailServices _mailService;
        private IConfigurationRoot _config;

        public AppController(IMailServices mailService, IConfigurationRoot config)
        {
            _mailService = mailService;
            _config = config;
        }

        public IActionResult Index()
        {
            // Find a view, render it and return it
            return View();
        }
        public IActionResult About()
        {
              return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:To"], model.Email, "From TheWorld", model.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent";

            }
            return View();
        }
    }
}
