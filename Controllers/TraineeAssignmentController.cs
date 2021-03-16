using NSILearningManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NSILearningManagementSystem.Controllers
{
    public class TraineeAssignmentController : Controller
    {
        // GET: TraineeAssignment
        LMSEntities1 db = new LMSEntities1();
        public ActionResult Index()
        {
            //Session["UserID"].ToString()
            string TraineeID = "1";
            var StatusCount = db.sp_TraineeDashboardDetails(Convert.ToInt32(TraineeID));
            List<TraineeAssignment> listTrs = new List<TraineeAssignment>();
            foreach (var items in StatusCount)
            {
                TraineeAssignment cs = new TraineeAssignment();
                cs.CourseCompleted = (int)items.CoursesCompleted;
                cs.CourseAssigned = (int)items.CoursesAssigned;
                cs.CourseInProgress = (int)items.CoursesInProgress;
                cs.CertificateAcheived = (int)items.CertificateAcheived;
                listTrs.Add(cs);
            }
            ViewData["Assignments"] = new SelectList(db.T_Assignment, "AssignmentID", "AssignmentName");

            List<TraineeAssignment> Assignmt = new List<TraineeAssignment>();
            var asg = db.sp_AssignmentSelect(0, 0);
            foreach (var itms in asg)
            {
                TraineeAssignment asgmt = new TraineeAssignment();
                asgmt.AssignmentID = itms.AssignmentID;
                asgmt.AssignmentName = itms.AssignmentName;
                //asgmt.AssignmentCode = itms.AssignmentCode;
                //asgmt.CourseID = itms.CourseID;
                listTrs.Add(asgmt);
            }
            //ViewBag.Assignments = new SelectList(Assignmt, "AssignmentID", "AssignmentName");
            return View(listTrs);
        }
        [HttpPost]
        public ActionResult Index(FormCollection col,HttpPostedFileBase PostedFile)
        {
            TraineeAssignment asgn = new TraineeAssignment();
            asgn.TraineeID = 1;
            var StatusCount = db.sp_TraineeDashboardDetails(Convert.ToInt32(asgn.TraineeID));
            asgn.AssignmentID = Convert.ToInt32(col["Assignments"].ToString());

            List<TraineeAssignment> listTrs = new List<TraineeAssignment>();
            foreach (var items in StatusCount)
            {
                TraineeAssignment cs = new TraineeAssignment();
                cs.CourseCompleted = (int)items.CoursesCompleted;
                cs.CourseAssigned = (int)items.CoursesAssigned;
                cs.CourseInProgress = (int)items.CoursesInProgress;
                cs.CertificateAcheived = (int)items.CertificateAcheived;
                listTrs.Add(cs);
            }
            ViewData["Assignments"] = new SelectList(db.T_Assignment, "AssignmentID", "AssignmentName");

            asgn.TraineeComments = col["TraineeComments"].ToString();
            if (PostedFile.ContentLength > 0)
            {
                string _extension = Path.GetExtension(PostedFile.FileName);
                asgn.TraineeAttache = "~/Assignment/" + asgn.AssignmentID + "_" + asgn.TraineeID + _extension;
            }
            else
                asgn.TraineeAttache = "";
            var val = db.sp_AssignmentUpdate(Convert.ToInt32(asgn.AssignmentID), 1, asgn.TraineeAttache, asgn.TraineeComments);
            if (PostedFile.ContentLength > 0)
            {
                string _extension = Path.GetExtension(PostedFile.FileName);
                string _FileName = asgn.AssignmentID.ToString() + "_" + asgn.TraineeID + _extension;
                string _path = Path.Combine(Server.MapPath("~/Assignment"), _FileName);
                PostedFile.SaveAs(_path);
            }
             return View(listTrs);
        }
        public JsonResult GetAssignmentCode(int AssignmentID)
        {
            List<T_Assignment> Asgmt = db.T_Assignment.Where(x => x.AssignmentID == AssignmentID).ToList();
            List<TraineeAssignment> TraineeAsgmt = new List<TraineeAssignment>();
            foreach (var itms in Asgmt)
            {
                TraineeAssignment Asignment = new TraineeAssignment();
                Asignment.AssignmentID = itms.AssignmentID;
                Asignment.AssignmentName = itms.AssignmentName;
                Asignment.AssignmentCode = itms.AssignmentCode;
                TraineeAsgmt.Add(Asignment);
            }
            return Json(TraineeAsgmt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCourseDetails()
        {
            int TraineeID = 1;
            var StatusCount = db.sp_TraineeDashboardDetails(Convert.ToInt32(TraineeID));
            List<TraineeAssignment> listTrs = new List<TraineeAssignment>();
            foreach (var items in StatusCount)
            {
                TraineeAssignment cs = new TraineeAssignment();
                cs.CourseCompleted = (int)items.CoursesCompleted;
                cs.CourseAssigned = (int)items.CoursesAssigned;
                cs.CourseInProgress = (int)items.CoursesInProgress;
                cs.CertificateAcheived = (int)items.CertificateAcheived;
                listTrs.Add(cs);
            }

            var AssignmentDetails = db.sp_TraineeAssignmentSelectAll(Convert.ToInt32(TraineeID));
            foreach (var items in AssignmentDetails)
            {
                TraineeAssignment cs = new TraineeAssignment();
                cs.SiNo = (int)items.SiNo;
                cs.CourseName = items.CourseName;
                cs.AssignmentName = items.AssignmentName;
                cs.DueDate = (DateTime)items.DueDate;
                if (items.TraineeAttachDate != null)
                {
                    cs.SubmittedDate = (DateTime)items.TraineeAttachDate;
                }
                cs.SubmittedStatus = items.SubmissionStatus;
                if (cs.SubmittedStatus == "Completed")
                    cs.ApprovedStatus = "Approved";
                else
                    cs.ApprovedStatus = "Not Approved";
                listTrs.Add(cs);
            }
            return View(listTrs);
        }
    }
}
