using NToastNotify;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BaalPratibha.Controllers
{
    public class BaseController : Controller
    {

        public IActionResult ShowError(string errorMessage)
        {
            return View("Error", errorMessage);
        }

        public BaseController(IToastNotification toastNotification)
        {
            ToastNotification = toastNotification;
        }

        private IToastNotification ToastNotification { get; set; }
        public void ShowToastNotification(string message, ToastEnums.ToastType type, string title = "")
        {
            ToastNotification.AddToastMessage(title, message, type);
        }
        public void ShowDeletedSuccessfullyToast()
        {
            ToastNotification.AddToastMessage("Task Successfull", "Deleted Succesfully", ToastEnums.ToastType.Success);
        }

        public void ShowSavedSuccessfullyToast()
        {
            ToastNotification.AddToastMessage("Task Successfull", "Record saved Succesfully", ToastEnums.ToastType.Success);
        }

        public void ShowUpdateSuccessfullyToast()
        {
            ToastNotification.AddToastMessage("Task Successfull", "Record updated Succesfully", ToastEnums.ToastType.Success);
        }

        public void ShowTaskFailedToast(string errorMessage = "Unknow error uccurred ! Please contact support service.")
        {
            ToastNotification.AddToastMessage("Task Failed", errorMessage, ToastEnums.ToastType.Error);
        }

        public void ShowTaskSuccessToast(string successMessage = "")
        {
            ToastNotification.AddToastMessage("Task Successfull !", successMessage, ToastEnums.ToastType.Success);
        }

        public override RedirectResult Redirect(string url)
        {
            return base.Redirect(string.IsNullOrEmpty(url) ? Url.Action("Index", "Home") : url);
        }


    }
}
