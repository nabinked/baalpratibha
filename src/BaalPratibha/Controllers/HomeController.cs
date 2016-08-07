using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BaalPratibha.DbPortal;
using BaalPratibha.Extensions;
using BaalPratibha.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BaalPratibha.Controllers
{
    public class HomeController : BaseController
    {

        private readonly ContestantDb _contestantDb;
        private readonly IHostingEnvironment _environment;
        // GET: /<controller>/
        public IActionResult Index()
        {

            var homeView = new HomePageView { ContestantViewList = _contestantDb.GetAllContestants() };
            return View(homeView);
        }

        public HomeController(IToastNotification toastNotification, ContestantDb contestantDb, IHostingEnvironment environment) : base(toastNotification)
        {
            _contestantDb = contestantDb;
            _environment = environment;
        }

    }
}
