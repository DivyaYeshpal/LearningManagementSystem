using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NSILearningManagementSystem.Models;

namespace NSILearningManagementSystem.Controllers
{
    public class AssignmentController : Controller
    {
        // GET: Assignment
        LMSEntities1 db = new LMSEntities1();
        public ActionResult Index(int id=0)
        {
            List<Assignment> crse = new List<Assignment>();
            ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            var crsmdl = obj.GetInstructorCourseModuleDetails(0, id, 0);
            foreach (DataRow itms in crsmdl.InstrCrseMdleTable.Rows)
            {
                Assignment asgmt = new Assignment();
                if (crsmdl.InstrCrseMdleTable.Rows.Count > 0)
                {
                    asgmt.CourseID = itms["CourseID"].ToString();
                    asgmt.CourseName = itms["CourseName"].ToString();
                    crse.Add(asgmt);
                }
                ViewBag.Courses = new SelectList(crse, "CourseID", "CourseName");
            }
            
            List<Assignment> Module = new List<Assignment>();
            var mdl = obj.GetInstructorCourseModuleDetails(2, id, 0);
            foreach (DataRow itms in mdl.InstrCrseMdleTable.Rows)
            {
                Assignment asgmt = new Assignment();
                if (mdl.InstrCrseMdleTable.Rows.Count > 0)
                {
                    asgmt.ModuleID = itms["ModuleID"].ToString();
                    asgmt.ModuleName = itms["Module"].ToString();
                    Module.Add(asgmt);
                }
                ViewBag.Modules = new SelectList(Module, "ModuleID", "ModuleName");
            }
            List<Assignment> Assignmt = new List<Assignment>();
            var asg = obj.SelectAssignment(0,0);
            foreach (DataRow itms in asg.AssignmentTable.Rows)
            {
                Assignment asgmt = new Assignment();
                if (asg.AssignmentTable.Rows.Count > 0)
                {
                    asgmt.AssignmentID =Convert.ToInt32(itms["AssignmentID"].ToString());
                    asgmt.AssignmentName = itms["AssignmentName"].ToString();
                    Assignmt.Add(asgmt);
                }
                ViewBag.Assignments = new SelectList(Assignmt, "AssignmentID", "AssignmentName");
            }
            List<Assignment> Trainee = new List<Assignment>();
            var trn = obj.SelectAssignment(1, 0);
            foreach (DataRow itms in trn.AssignmentTable.Rows)
            {
                Assignment asgmt = new Assignment();
                if (trn.AssignmentTable.Rows.Count > 0)
                {
                    asgmt.TraineeID = Convert.ToInt32(itms["TraineeID"].ToString());
                    asgmt.Trainee = itms["Trainee"].ToString();
                    Trainee.Add(asgmt);
                }
                ViewBag.Trainee = new SelectList(Trainee, "TraineeID", "Trainee");
            }
            
            TempData["InstructorID"] = id;
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection col,HttpPostedFileBase PostedFile)
        {
            int InstructorID=Convert.ToInt32(TempData["InstructorID"]);
            ServiceReference1.Assignment asgn=new ServiceReference1.Assignment();
            ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            asgn.AssignDate=col["startDate"].ToString();
            asgn.DueDate=col["dueDate"].ToString();
            asgn.AssignmentName = col["asgmtName"];
            asgn.CourseID = col["ddlCourse"];
            if(asgn.CourseID != "")
            {
                List<Assignment> module = new List<Assignment>();
                ServiceReference1.CourseServiceClient obj1 = new ServiceReference1.CourseServiceClient();
                var mdl = obj.GetInstructorCourseModuleDetails(2, InstructorID, 0);
                foreach (DataRow itms in mdl.InstrCrseMdleTable.Rows)
                {
                    Assignment asgmt = new Assignment();
                    if (mdl.InstrCrseMdleTable.Rows.Count > 0)
                    {
                        asgmt.ModuleID = itms["ModuleID"].ToString();
                        asgmt.ModuleName = itms["Module"].ToString();
                        module.Add(asgmt);
                    }
                    ViewBag.ModuleID = new SelectList(module, "ModuleID", "ModuleName");
                }
            }
            asgn.TraineeInstr = col["traineeInstruction"];
            asgn.InstructorID = "1";
            asgn.ModuleID= col["ddlModule"];
            asgn.Status = 1;
            asgn.UserID = "1";
            string fileupload = col["fileupload"];
            if (PostedFile.ContentLength > 0)
            {
                string _extension = Path.GetExtension(PostedFile.FileName);
                asgn.Instratt = "~/UploadedFiles/" + asgn.AssignmentName.ToString() + _extension;
                string _FileName = asgn.AssignmentName.ToString() + _extension;
                string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                PostedFile.SaveAs(_path);
            }
            obj.InsertAssignment(asgn);
            return RedirectToAction("Index", "Assignment");
        }
        public JsonResult GetTraineeList(int AssignmentID)
        {
            List<T_AssignmentDetails> TraineeList = db.T_AssignmentDetails.Where(x => x.AssignmentID == AssignmentID).ToList();
            db.Configuration.ProxyCreationEnabled = true;
            ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            List<Assignment> Trainee = new List<Assignment>();
            var trn = obj.SelectAssignment(1, AssignmentID);
            foreach (DataRow itms in trn.AssignmentTable.Rows)
            {
                Assignment asgmt1 = new Assignment();
                if (trn.AssignmentTable.Rows.Count > 0)
                {
                    asgmt1.TraineeID = Convert.ToInt32(itms["TraineeID"].ToString());
                    asgmt1.Trainee = itms["Trainee"].ToString();
                    Trainee.Add(asgmt1);
                }
                ViewBag.Trainee = new SelectList(Trainee, "TraineeID", "Trainee");
            }
            return Json(Trainee, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EvaluationAssignment(int? AssignmentID, int? TraineeID, DateTime? DueDate)
        {
            List<Assignment> Trainee = new List<Assignment>();
            if (AssignmentID == null)
                AssignmentID = 0;
            if (TraineeID == null)
                TraineeID = 0;
            if (DueDate == null)
                DueDate = Convert.ToDateTime("1900-01-01");
            var spModel = db.sp_TraineeAssignments_Select(0, AssignmentID, TraineeID, DueDate);
            foreach (var item in spModel)
            {
                Assignment asgn = new Assignment();
                asgn.AssignmentName = item.AssignmentName;
                asgn.AssignmentID = Convert.ToInt64(item.AssignmentID);
                asgn.Trainee = item.Trainee;
                asgn.TraineeID = Convert.ToInt32(item.TraineeID);
                asgn.CourseID = item.CourseID.ToString();
                asgn.CourseName = item.CourseName;
                asgn.SubmittedDate = item.SubmittedDate.ToString();
                asgn.Gender = item.Gender;
                asgn.SubmittedStatus = item.SubmissionStatus;
                asgn.TraineeStatus = Convert.ToInt16(item.TraineeStatus);
                asgn.TraineeImage = item.TraineeImage;
                asgn.TraineeAttach = item.TraineeAttach;
                Trainee.Add(asgn);
            }
            //var pagedData = Pagination.PagedResult(Trainee, pageNum, pageSize);
            //return Json(pagedData, JsonRequestBehavior.AllowGet);
            //return Json(Trainee, JsonRequestBehavior.AllowGet);
            return View(Trainee);
        }
        public JsonResult BindingModalPopup(int? AssignmentID, int? TraineeID, DateTime? DueDate,int pageNum = 1, int pageSize = 2)
        {
            List<Assignment> Trainee = new List<Assignment>();
            if (AssignmentID == null)
                AssignmentID = 0;
            if (TraineeID == null)
                TraineeID = 0;
            if (DueDate == null)
                DueDate =Convert.ToDateTime( "1900-01-01");
            var spModel = db.sp_TraineeAssignments_Select(0, AssignmentID, TraineeID,DueDate);
            foreach (var item in spModel)
            {
                Assignment asgn = new Assignment();
                asgn.AssignmentID =Convert.ToInt64(item.AssignmentID);
                asgn.Trainee = item.Trainee;
                asgn.TraineeID =Convert.ToInt32(item.TraineeID);
                asgn.CourseID =item.CourseID.ToString();
                asgn.CourseName = item.CourseName;
                asgn.SubmittedDate = item.SubmittedDate.ToString();
                asgn.Gender = item.Gender;
                asgn.SubmittedStatus =item.SubmissionStatus;
                asgn.TraineeStatus = Convert.ToInt16(item.TraineeStatus);
                asgn.TraineeImage = item.TraineeImage;
                Trainee.Add(asgn);               
            }
            var pagedData = Pagination.PagedResult(Trainee, pageNum, pageSize);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
            //return Json(Trainee, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownloadAttachment(int TraineeID,int AssignmentID)
        {
            // Find user by passed id
            var file = db.T_AssignmentDetails.FirstOrDefault(x => x.TraineeID == TraineeID && x.AssignmentID == AssignmentID);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(file.TraineeAttach));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file.TraineeAttach);
        }
        [HttpGet]
        public ActionResult SubmitAssignmentGrade(int AssignmentID, int TraineeID, string Grade)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (AssignmentID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_AssignmentDetails asgn = db.T_AssignmentDetails.FirstOrDefault(x => x.TraineeID == TraineeID && x.AssignmentID == AssignmentID);

            if (asgn == null)
            {
                return HttpNotFound();
            }
            Assignment model = new Assignment();
            asgn.AssignmentID=AssignmentID;
            asgn.Grade = Grade;
            db.Entry(asgn).State = EntityState.Modified;
            db.SaveChanges();
            //return RedirectToAction("Index", "Assignment", new { InstructorID = 1 });
            return new EmptyResult();
        }
        public JsonResult SearchTrainee(string Search, int AssignmentID)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            //var trainees = from p in db.M_Trainee select p;
            //if (!string.IsNullOrWhiteSpace(Search))
            //{
            //    trainees = trainees.Where(p => p.Trainee.Contains(Search));
            //}
            //return Json(trainees, JsonRequestBehavior.AllowGet);
            List<Assignment> Trainee = new List<Assignment>();
            //if (!string.IsNullOrWhiteSpace(Search))
            //{
                var spModel = db.sp_TraineeSearchResult(Search, AssignmentID);
            //}
            foreach (var item in spModel)
            {
                Assignment asgn = new Assignment();
                asgn.Trainee = item.Trainee;
                asgn.TraineeID = Convert.ToInt32(item.TraineeID);
                Trainee.Add(asgn);
            }
            return Json(Trainee, JsonRequestBehavior.AllowGet);
        }
    }
}
