using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.ViewModels;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;
using Microsoft.AspNetCore.Authorization;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailServices _mailService;
        private IConfigurationRoot _config;
        private IWorldRepository _repository;

        public AppController(IMailServices mailService, IConfigurationRoot config, IWorldRepository repository)
        {
            _mailService = mailService;
            _config = config;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Trips()
        {
            var data = _repository.GetAllTrips();
            // Find a view, render it and return it
            return View(data);

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
