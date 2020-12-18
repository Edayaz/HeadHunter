using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HeadHunter.Models.Entity;

namespace HeadHunter.Controllers
{
    public class AccountController : Controller
    {
        HeadHunterEntities db = new HeadHunterEntities();
        // GET: Account
        public ActionResult Index()
        {
            
            return View();
        }
       

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(HeadHunter.Models.UnifiedModels.LoginModel model, string returnUrl)
        {
            if ((model.Account.AccountMail == null)||(model.AccountPassword.AccountPassword1==null))
            {
                ViewBag.Msg = "Please Enter Your Mail Or Password";
                return View();
            }
            else
            {
                Account newAccount = new Account();
                AccountPassword newAccountPassword = new AccountPassword();
                newAccount = db.Account.FirstOrDefault(m => m.AccountMail == model.Account.AccountMail);
                newAccountPassword = db.AccountPassword.FirstOrDefault(m => m.AccountId == newAccount.AccountId);


                //var count = db.Account.Where(x => x.AccountMail == model.Account.AccountMail && x.AccountFullName == model.AccountPassword.AccountPassword1).Count();
                if ((model.Account.AccountMail == newAccount.AccountMail) && (model.AccountPassword.AccountPassword1 == newAccountPassword.AccountPassword1))
                {
                    FormsAuthentication.SetAuthCookie(model.Account.AccountMail, false);
                    if (newAccount.AccountType == false)
                    {
                        Session.Add("MemberAuth", RoleNames.Employer);
                        Session.Add("UserMail", model.Account.AccountMail);
                    }
                    else
                    {
                        Session.Add("MemberAuth", RoleNames.Employee);
                        Session.Add("UserMail", model.Account.AccountMail);
                    }
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ViewBag.Msg = "Invalid User";
                    return View();
                }
            }
           

        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //[Authorize]
        [AuthorizeRoleAttribute(RoleNames.Employee,RoleNames.Employer)]
        public ActionResult Logout()
        {
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string returnUrl, HeadHunter.Models.UnifiedModels.SignUpModel model)
        {
            DateTime localDate = DateTime.Now;
            if ((db.Account.FirstOrDefault(m => m.AccountMail == model.Account.AccountMail))==null)
            {
                Account newAccount = new Account();
                newAccount.AccountFlag = 1;
                newAccount.AccountFullName = model.Account.AccountFullName;
                newAccount.AccountMail = model.Account.AccountMail;
                newAccount.AccountType = model.Account.AccountType;
                newAccount.AccountStatus = localDate;
                db.Account.Add(newAccount);
                db.SaveChanges();

                AccountPassword newAccountPassword = new AccountPassword();
                Account savedAccount = new Account();
                savedAccount = db.Account.FirstOrDefault(m => m.AccountMail == model.Account.AccountMail);

                newAccountPassword.AccountId = savedAccount.AccountId;
                newAccountPassword.AccountPassword1 = model.AccountPassword.AccountPassword1;
                newAccountPassword.AccountPasswordFlag = true;

                Profil newGithubProfile = new Profil();
                Profil newStackOvProfile = new Profil();

                if (model.Profile.ProfileUsername != null)
                {
                    newGithubProfile.AccountId = savedAccount.AccountId;
                    newGithubProfile.ProfileFlag = true;
                    newGithubProfile.ProfileUsername = model.Profile.ProfileUsername;
                    newGithubProfile.SourceId = 1;
                    db.Profil.Add(newGithubProfile);
                }
                if (model.StackOverFlowUsername != null)
                {
                    newStackOvProfile.AccountId = savedAccount.AccountId;
                    newStackOvProfile.ProfileFlag = true;
                    newStackOvProfile.ProfileUsername = model.StackOverFlowUsername;
                    newStackOvProfile.SourceId = 2;
                    db.Profil.Add(newStackOvProfile);
                }
                
               
                db.AccountPassword.Add(newAccountPassword);
                db.SaveChanges();
            }
            else
            {
                ViewBag.Msg = "Invalid Mail";

                return View();
            }



            return RedirectToLocal(returnUrl);

        }
    }
    public enum RoleNames
    {
        Employer = 1,
        Employee = 2,

    }

    class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        private RoleNames[] roleNames { get; set; }
        public AuthorizeRoleAttribute(params RoleNames[] roles)
        {
            roleNames = roles;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            object session = HttpContext.Current.Session["MemberAuth"];

            foreach (var role in roleNames)
            {
                //if (int.Parse(HttpContext.Current.Session["MemberAuth"].ToString()) == int.Parse(role.ToString()))
                if (session == null)
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
                else if (session.ToString() == role.ToString())
                {
                    return;
                }
            }
            filterContext.Result = new RedirectResult("~/Account/Login");
        }
    }
}