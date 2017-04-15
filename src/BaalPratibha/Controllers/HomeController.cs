using BaalPratibha.DbPortal;
using BaalPratibha.Extensions;
using BaalPratibha.Logic;
using BaalPratibha.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NToastNotify;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BaalPratibha.Controllers
{
    public class HomeController : BaseController
    {

        private readonly ContestantDb _contestantDb;
        private readonly IHostingEnvironment _environment;
        private readonly ShareDb _shareDb;
        private AppSettings _appSettings;
        private IViewHelper _viewHelper;

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {

            if (_appSettings.IsVotingPeriod)
            {
                await _shareDb.UpdateAllShares();
            }
            var orderBy = "";
            var isAdmin = User.GetRole() == Models.Enums.Roles.Admin.ToString();
            var showVoteDetails = _viewHelper.ShowVoteDetails(isAdmin, _appSettings.ShowVotes);
            orderBy = _viewHelper.GetOrderBy(showVoteDetails, orderBy);
            var homeView = new HomePageView { ContestantViewList = _contestantDb.GetAllContestants(orderBy) };
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

        public HomeController(IToastNotification toastNotification,
                                ContestantDb contestantDb,
                                IHostingEnvironment environment,
                                ShareDb shareDb,
                                IOptions<AppSettings> appOptions,
                                IViewHelper viewHelper) : base(toastNotification)
        {
            _viewHelper = viewHelper;
            _appSettings = appOptions.Value;
            _contestantDb = contestantDb;
            _environment = environment;
            _shareDb = shareDb;
        }

        public IActionResult Finals()
        {
            return View();
        }

    }
}
