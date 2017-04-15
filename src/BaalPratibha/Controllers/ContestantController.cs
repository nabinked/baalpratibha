using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BaalPratibha.DbPortal;
using BaalPratibha.Extensions;
using BaalPratibha.Logic;
using BaalPratibha.Models;
using BaalPratibha.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using NToastNotify;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BaalPratibha.Controllers
{
    public class ContestantController : BaseController
    {

        public ContestantController(IToastNotification toastNotification,
                                    ContestantDb contestantDb,
                                    UserDb userDb,
                                    VoteDb voteDb,
                                    IViewHelper viewHelper,
                                    ShareDb shareDb,
                                    ImageProcessing imageProcessing,
                                    IOptions<AppSettings> appOptions) : base(toastNotification)
        {
            _contestantDb = contestantDb;
            _userDb = userDb;
            _voteDb = voteDb;
            _viewHelper = viewHelper;
            _shareDb = shareDb;
            _imageProcessing = imageProcessing;
            _appSettings = appOptions.Value;
        }

        private readonly ContestantDb _contestantDb;
        private readonly UserDb _userDb;
        private readonly VoteDb _voteDb;
        private readonly IViewHelper _viewHelper;
        private readonly ShareDb _shareDb;
        private readonly ImageProcessing _imageProcessing;
        private AppSettings _appSettings;
        // GET: /<controller>/

        public IActionResult All(string orderBy = "Rank_Asc")
        {
            var isAdmin = User.GetRole() == Models.Enums.Roles.Admin.ToString();

            var showVoteDetails = _viewHelper.ShowVoteDetails(isAdmin, _appSettings.ShowVotes);

            orderBy = _viewHelper.GetOrderBy(showVoteDetails, orderBy);

            var contestants = _contestantDb.GetAllContestants(orderBy.ToLower());
            return View(contestants);
        }


        [Authorize(Policy = "AdminOnly")]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Add(User user, string email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userDb.Insert(user);
                    var emailSender = new Email(email, user.FullName, user.UserName, user.Password);
                    var result = await emailSender.SendMail();
                    return RedirectToAction(nameof(All));
                }
                catch (PostgresException ex)
                {
                    ShowTaskFailedToast(ex.MessageText);
                    return View(user);
                }
            }
            else
            {
                return View(user);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public IActionResult Delete(int userId)
        {
            try
            {
                if (_userDb.Delete(userId))
                {
                    ShowDeletedSuccessfullyToast();
                    return RedirectToAction(nameof(All));
                }
                else
                {
                    ShowTaskFailedToast();
                }
            }
            catch (PostgresException ex)
            {
                ShowTaskFailedToast(ex.MessageText);
            }
            return RedirectToAction(nameof(All));

        }

        [Route("/{userName}")]
        public async Task<IActionResult> Profile(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                if (_appSettings.IsVotingPeriod)
                {
                    await _shareDb.UpdateShare(userName);
                }
                var contestantView = _contestantDb.GetContestantViewByUserName(userName);
                if (contestantView != null)
                    return View(contestantView);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult Update(int userId)
        {
            if (User.GetRole() != Models.Enums.Roles.Admin.ToString() && userId != User.GetId()) return Forbid();
            var user = _userDb.GetUserById(userId);
            if (user == null)
            {
                return ShowError("user could not be found on the database.");
            }
            var contestantDetail = _contestantDb.GetContestantDetailByUserId(userId) ?? new ContestantDetail();
            ViewBag.ProfilePagePhotoId = userId;
            return View(new UpdateContestantViewModel() { FullName = user.FullName, UserName = user.UserName, ContestantDetail = contestantDetail });

        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(UpdateContestantViewModel contestantViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_userDb.UpdateUser(contestantViewModel.FullName, contestantViewModel.UserName) > 0)
                {
                    if (contestantViewModel.ContestantDetail.UserId > 0)
                    {
                        _contestantDb.Update(contestantViewModel.ContestantDetail);
                        ShowUpdateSuccessfullyToast();
                    }
                    else
                    {
                        contestantViewModel.ContestantDetail.UserId =
                            _userDb.GetUserByUserName(contestantViewModel.UserName).Id;
                        _contestantDb.Insert(contestantViewModel.ContestantDetail);
                        ShowSavedSuccessfullyToast();
                    }


                }

            }
            return RedirectToAction("Update", new { userId = contestantViewModel.ContestantDetail.UserId });

        }


        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(ICollection<IFormFile> files, string returnUrl)
        {
            var isSavedSuccessfully = false;
            var message = "";
            if (files.Any())
            {

                try
                {
                    var file = files.FirstOrDefault();
                    if (_imageProcessing.IsImage(file))
                    {
                        await _imageProcessing.AddImage(files.FirstOrDefault(), User.GetId());
                        isSavedSuccessfully = true;
                        message = "File uploaded succesfully";
                    }
                    else
                    {
                        message = "File format is not an image type.";
                    }
                }
                catch (Exception ex)
                {
                    isSavedSuccessfully = false;
                    message = ex.Message;
                }

            }
            else
            {
                message = "No image found submitted";
            }
            if (isSavedSuccessfully)
                ShowTaskSuccessToast(message);
            else
                ShowTaskFailedToast(message);
            return Redirect(returnUrl);
        }


        public IActionResult Vote(int id)
        {
            return View(new Vote() { ContestantId = id });
        }

        [HttpPost]
        public IActionResult Vote(Vote vote)
        {
            var contestant = _contestantDb.GetContestantView(vote.ContestantId);
            if (ModelState.IsValid)
            {
                var oldVote = _voteDb.GetVoteByEmail(vote.VoterEmail);
                if (oldVote != null)
                {
                    //This guy has already voted 
                    if (oldVote.ContestantId == vote.ContestantId)
                    {
                        //voting again for the same contestant again
                        ShowTaskFailedToast($"You have already voted for {contestant.FullName}.");
                        return View(vote);
                    }
                    else
                    {
                        //Voting for a different contestant
                        if (_voteDb.Update(vote))
                        {
                            ShowTaskSuccessToast(
                                $"You have succesfully voted for {contestant.FullName}.");
                            return Redirect(Url.Content($"~/{contestant.UserName}"));
                        }
                        else
                        {
                            return
                                ShowError(
                                    "Database Error. Could not update your vote to database. Please contact support.");
                        }
                    }
                }
                else
                {
                    //This is a new vote
                    if (_voteDb.Insert(vote) > 0)
                    {
                        ShowTaskSuccessToast($"You have succesfully voted for {contestant.FullName}");
                        return Redirect(Url.Content($"~/{contestant.UserName}"));
                    }
                    else
                    {
                        return
                            ShowError("Database Error. Could not insert your vote to database. Please contact support.");
                    }
                }
            }
            else
            {
                //Model state not valid
                return View(vote);
            }

        }

        [HttpGet]
        public IActionResult Search(string searchString)
        {
            var contestants = new List<ContestantView>();

            return Json()
        }
    }
}
