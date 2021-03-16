using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NSILearningManagementSystem.Models;

namespace NSILearningManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection col)
        {
            //FormsAuthentication.SetAuthCookie(u.UserName, false);
            if (ModelState.IsValid) // this is check validity
            {
                using (LMSEntities1 db = new LMSEntities1())
                {
                    var UserName = col["EmailID"];
                    var Password = col["Password"];
                    var data = db.M_UserDetails.Where(a => a.UserName.Equals(UserName) && a.Password.Equals(Password)).FirstOrDefault();
                    //var data = (from u in M_UserDetails where u.UserName == col["EmailID"].ToString()).ToList();
                    //return data.Select(u => new ViewTestPackageState)
                    if (data != null)
                    {
                        Session["UserID"] = data.UserID.ToString();
                        Session["UserName"] = col["EmailID"].ToString();
                        if (data.UserType == "1")
                        {
                            return RedirectToAction("Index", "Instructor");
                        }
                        else if(data.UserType == "2")
                        {
                            return RedirectToAction("Index", "TraineeProgress");
                        }
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                        @ViewBag.Message = "Wrong Username and password combination !";
                        return View();
                    }
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View();
            }
        }
    }
}