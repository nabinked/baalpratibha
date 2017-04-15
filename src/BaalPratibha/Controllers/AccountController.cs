using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BaalPratibha.DbPortal;
using BaalPratibha.Extensions;
using BaalPratibha.Models;
using BaalPratibha.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BaalPratibha.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserDb _userDb;
        private readonly ContestantDb _contestantDb;

        public AccountController(UserDb userDb, DbPortal.ContestantDb contestantDb, IToastNotification toastNotification) : base(toastNotification)
        {
            _userDb = userDb;
            _contestantDb = contestantDb;
        }

        // GET: /<controller>/
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                var contestant = _userDb.GetUserByUserName(userName);
                bool passWordMatch = contestant.Password.Equals(password); 
                if (contestant != null && passWordMatch)
                {

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, contestant.UserName),
                        new Claim(ClaimTypes.Sid, contestant.Id.ToString()),
                        new Claim(ClaimTypes.GivenName, contestant.FullName),
                        new Claim(ClaimTypes.Role, contestant.Role.ToString())
                    };
                    var identity = new ClaimsIdentity(claims, "Basic", "name", "role");

                    await
                        HttpContext.Authentication.SignInAsync("BaalPratibhaAuthCookie", new ClaimsPrincipal(identity));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ShowTaskFailedToast("Could not authenticate the user. Please try again");
                    return View();
                }
            }

            return View();

        }

        public async Task<RedirectToActionResult> LogOff()
        {
            await HttpContext.Authentication.SignOutAsync("BaalPratibhaAuthCookie");
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult UpdateProfile(int userId)
        {
            var user = _userDb.GetUserById(userId);
            var role = user.Role;

            if (role == Models.Enums.Roles.Admin) //Admin
            {
                return View("UpdateAdmin");
            }
            else
            {
                if (User.GetRole() != Models.Enums.Roles.Admin.ToString() && userId != User.GetId()) return Forbid();

                return RedirectToAction("Update", "Contestant", new { userId });
            }
        }
    }
}
