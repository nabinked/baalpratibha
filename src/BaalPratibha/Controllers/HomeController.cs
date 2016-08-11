using BaalPratibha.DbPortal;
using BaalPratibha.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BaalPratibha.Controllers
{
    public class HomeController : BaseController
    {

        private readonly ContestantDb _contestantDb;
        private readonly IHostingEnvironment _environment;
        private readonly ShareDb _shareDb;
        // GET: /<controller>/
        public IActionResult Index()
        {

            _shareDb.UpdateAllShares();
            var homeView = new HomePageView { ContestantViewList = _contestantDb.GetAllContestants() };
            return View(homeView);
        }

        public IActionResult RankingMethod()
        {
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

        public HomeController(IToastNotification toastNotification, ContestantDb contestantDb, IHostingEnvironment environment, ShareDb shareDb) : base(toastNotification)
        {
            _contestantDb = contestantDb;
            _environment = environment;
            _shareDb = shareDb;
        }

    }
}
