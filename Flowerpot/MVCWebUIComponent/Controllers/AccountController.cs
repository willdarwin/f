using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using GeneralUtilities.Utilities.Unity;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using MVCWebUIComponent.Filter;
using MVCWebUIComponent.Models;
using MVCWebUIComponent.Models.Translates;
using Trirand.Web.Mvc;
using UserDomain.DomainLayer;
using UserDomain.DomainLayer.Entities;
using UserDomain.ServiceLayer;
using UtilityComponent.Filters;

namespace MVCWebUIComponent.Controllers
{
    public class AccountController : BaseController
    {

        private IUserService UserService { get; set; }

        public AccountController(IUserService userService)
        {
            UserService = userService;

        }
        public AccountController()
        {
            UserService = PolicyInjection.Wrap<IUserService>(Container.Resolve<UserService>());

        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn(string referURL)
        {
            var model = new LogOnModel();
            if (referURL != null && referURL != "")
            {
                model.ReturnUrl = referURL;
            }
            return View(model);
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string referURL)
        {
            if (ModelState.IsValid)
            {
                var user = new User();
                user.UserName = model.UserName;
                user.Password = model.Password;
                user = UserService.ValidateUser(user);
                if (user.UserId > 0)
                {
                    Session.Add("UserId", user.UserId);
                    FormsAuthentication.SetAuthCookie(user.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(referURL) && referURL.Length > 1 && referURL.StartsWith("/")
                        && !referURL.StartsWith("//") && !referURL.StartsWith("/\\"))
                    {
                        return Redirect(referURL);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User();
                user.UserName = model.UserName;
                user.Password = model.Password;
                user.Email = model.Email;
                user = UserService.RegisterUser(user);

                if (user.Status == UserStatus.Success)
                {
                    Session.Add("UserId", user.UserId);
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(user.Status));
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    var currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(UserStatus userStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (userStatus)
            {
                case UserStatus.UserNameRegistered:
                    return @Resources.Resources.ErrUserNameRegistered;

                case UserStatus.EmailRegistered:
                    return @Resources.Resources.ErrEmailRegistered;

                default:
                    return @Resources.Resources.ErrDefault;
            }
        }
        #endregion


        public ActionResult InvitationCodeGenerate()
        {
            return View();
        }



        [HttpPost]
        public ActionResult InvitationCodeGenerate(GenerateInvitationCodeModel model)
        {
            if (ModelState.IsValid)
            {
                var count = model.Number;
                UserService.InvitationCodeGenerate(count);
            }
            return View();
        }


        //[AutoMapperConfigurationActionFilter(typeof(UserDomainMVCProfile))]
        //public ActionResult ShowAllInvitationCode()
        //{
        //    List<InvitationCode> list = UserService.GetAllInvitationCode();
        //    List<InvitationCodeModel> listModel = new List<InvitationCodeModel>();
        //    for (int i = 0; i < list.Count; i++ )
        //    {
        //        listModel.Add(Mapper.Map<InvitationCode, InvitationCodeModel>(list[i]));
        //    }
        //    return View(listModel);
        //}


        [UserAuthorize]
        public ActionResult ShowAllInvitationCode()
        {
            // Get the model (setup) of the grid defined in the /Models folder.
            var gridModel = new CodesJqGridModel();
            var ordersGrid = gridModel.CodesGrid;

            // customize the default Orders grid model with custom settings
            // NOTE: you need to call this method in the action that fetches the data as well,
            // so that the models match
            SetUpGrid(ordersGrid);

            // Pass the custmomized grid model to the View
            return View(gridModel);
        }
    
        private void SetUpGrid(JQGrid ordersGrid)
        {
            // Customize/change some of the default settings for this model
            // ID is a mandatory field. Must by unique if you have several grids on one page.
            ordersGrid.ID = "OrdersGrid";
            // Setting the DataUrl to an action (method) in the controller is required.
            // This action will return the data needed by the grid
            ordersGrid.DataUrl = Url.Action("SearchGridDataRequested");
            // show the search toolbar
            ordersGrid.ToolBarSettings.ShowSearchToolBar = false;

            ordersGrid.ToolBarSettings.ShowEditButton = true;
            ordersGrid.ToolBarSettings.ShowAddButton = true;
            ordersGrid.ToolBarSettings.ShowDeleteButton = true;
            ordersGrid.EditDialogSettings.CloseAfterEditing = true;
            ordersGrid.AddDialogSettings.CloseAfterAdding = true;

        }

        [AutoMapperConfigurationActionFilter(typeof(UserDomainMVCProfile))]
        public JsonResult SearchGridDataRequested()
        {
            // Get both the grid Model and the data Model
            // The data model in our case is an autogenerated linq2sql database based on Northwind.
            var gridModel = new CodesJqGridModel();
            var list = UserService.GetAllInvitationCode();
            var listModel = new List<InvitationCodeModel>();
            for (var i = 0; i < list.Count; i++)
            {
                listModel.Add(Mapper.Map<InvitationCode, InvitationCodeModel>(list[i]));
            }
            IQueryable gridList = (from codes in listModel select codes).AsQueryable<InvitationCodeModel>();
            SetUpGrid(gridModel.CodesGrid);
            
            // return the result of the DataBind method, passing the datasource as a parameter
            // jqGrid for ASP.NET MVC automatically takes care of paging, sorting, filtering/searching, etc
            return gridModel.CodesGrid.DataBind(gridList);
        }
    }
}
