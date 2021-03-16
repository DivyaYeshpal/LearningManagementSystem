using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using NSILearningManagementSystem.Models;
using static NSILearningManagementSystem.Models.GroupDiscussion;

namespace NSILearningManagementSystem.Controllers
{
    public class GroupDiscussionController : Controller
    {
        LMSEntities1 db = new LMSEntities1();
        int InstructorID = 1;
        // GET: GroupDiscussion
        public List<GroupDiscussion> GetAllCourses(int InstructorID)
        {
            var CourseList = db.sp_InstructorCourseModuleSelect(0, 1, 0);
            List<GroupDiscussion> Courses = new List<GroupDiscussion>();
            foreach (var item in CourseList)
            {
                GroupDiscussion gd = new GroupDiscussion();
                gd.CourseID = item.CourseID;
                gd.CourseName = item.CourseName;
                Courses.Add(gd);
            }
            return Courses.ToList();
        }
        public JsonResult GetAllStudentsCourses(string[] CourseID)
        {
            GroupDiscussion gd = new GroupDiscussion();
            string Courseids = "";
            if (CourseID != null)
            {
                foreach (var i in CourseID)
                {
                    Courseids = Courseids + i + ',';
                }
            }
            else
                Courseids = "0";
            //dynamic func_param = JsonConvert.DeserializeObject(CourseID);
            var CourseList = db.sp_TraineeCourseSelect(Courseids);
            List<SelectListItem> Students = new List<SelectListItem>();
            foreach (var item in CourseList)
            {

                Students.Add(new SelectListItem { Text = item.Trainee, Value = item.TraineeID.ToString() });
                gd.TraineeNames = Students;

            }
            return Json(Students, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            GroupDiscussion gd = new GroupDiscussion();
            //List<SelectListItem> items = new List<SelectListItem>();
            //gd.Courses = GetAllCourses(1);
            //items.Add(new SelectListItem
            //{
            //    Text = gd.CourseName,
            //    Value = gd.CourseID.ToString()
            //});
            LMSEntities1 _entity = new LMSEntities1();
            List<GroupDiscussion> grpd = new List<GroupDiscussion>();
            var InstructorDashboard = db.sp_InstructorDashboardSelect(InstructorID);
            foreach (var itemss in InstructorDashboard)
            {
                    GroupDiscussion groupdisc = new GroupDiscussion();
                    ViewBag.TotalExamRemaining = Convert.ToInt32(itemss.TotalExamRemaining);
                    ViewBag.TotalExamCompleted = Convert.ToInt32(itemss.TotalExamCompleted);
                    ViewBag.TotalExamPassed = Convert.ToInt32(itemss.TotalExamPassed);
                    ViewBag.TotalExamFailed = Convert.ToInt32(itemss.TotalExamFailed);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection col, GroupDiscussion gd, HttpPostedFileBase PostedFile)
        {
            //if (ModelState.IsValid)
            //{
            string CourseIDs = "", _extension = "", StudentIDs="";
            gd.InstructorID = 1;
            gd.UserID = Session["UserID"].ToString();
            if (PostedFile.ContentLength > 0)
            {
                _extension = Path.GetExtension(PostedFile.FileName);
            }
            else
            {
                _extension = "";
            }
            gd.Extension = _extension;
            var coursesSelected = new SelectList(col["Courses"]);
            foreach (SelectListItem item in coursesSelected)
            {
                CourseIDs += item.Text + ',';
            }
            var studentsSelected = new SelectList(Request.Form["StudList"]);
            if(studentsSelected.Items.ToString()!="")
            {
                StudentIDs = studentsSelected.Items.ToString();
            }
            LMSEntities1 _entity = new LMSEntities1();
           
            ObjectParameter returnId = new ObjectParameter("Output", typeof(int)); //Create Object parameter to receive a output value.It will behave like output parameter  
            var value = _entity.sp_GroupDiscussion_Insert(col["GroupName"], col["GDMessage"], gd.InstructorID, 2, "Hours",
                    gd.UserID, gd.GroupDiscussionLink, gd.Extension, CourseIDs, StudentIDs, returnId).ToList();
            ViewBag.GroupID = Convert.ToInt32(returnId.Value); //set the out put value to StudentsCount ViewBag  

            if (PostedFile.ContentLength > 0)
            {
                gd.DocumentAttach = "~/GroupDiscussionAttachment/" + ViewBag.GroupID + _extension;
                string _FileName = ViewBag.GroupID + _extension;
                string _path = Path.Combine(Server.MapPath("~/GroupDiscussionAttachment"), _FileName);
                PostedFile.SaveAs(_path);
            }
            ViewBag.Message = "File Uploaded Successfully!!";
            gd.Courses = GetAllCourses(1);
            return View(gd);
            //}
            //List<SelectListItem> items = new List<SelectListItem>();
            //gd.Courses = GetAllCourses(1);
            //items.Add(new SelectListItem
            //{
            //    Text = gd.CourseName,
            //    Value = gd.CourseID.ToString()
            //});
            //return View(gd);
        }
    }
}


