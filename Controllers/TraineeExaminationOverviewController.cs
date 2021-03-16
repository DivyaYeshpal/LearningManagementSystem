using NSILearningManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using NSILearningManagementSystem;

namespace NSILearningManagementSystem.Controllers
{
    public class TraineeExaminationOverviewController : Controller
    {
        // GET: TraineeExaminationOverview
        public ActionResult Index()
        {
            int TraineeID = 1;
            List<TraineeExaminationOverview> listExm = new List<TraineeExaminationOverview>();
            //ServiceReference1.CourseServiceClient obj1 = new ServiceReference1.CourseServiceClient();
            //var traineecrsstatus = obj1.SelectTraineeDashboardDetails(TraineeID);
            //foreach (DataRow itms in traineecrsstatus.TraineeCrseStatusTable.Rows)
            //{
            //    TraineeExaminationOverview trexm = new TraineeExaminationOverview();
            //    if (traineecrsstatus.TraineeCrseStatusTable.Rows.Count > 0)
            //    {
            //        trexm.CourseCompleted = Convert.ToInt32(itms["CoursesCompleted"].ToString());
            //        trexm.CourseAssigned = Convert.ToInt32(itms["CoursesAssigned"].ToString());
            //        trexm.CourseInProgress = Convert.ToInt32(itms["CoursesInProgress"].ToString());
            //        trexm.CertificateAcheived = Convert.ToInt32(itms["CertificateAcheived"].ToString());
            //    }
            //    listCrs.Add(trexm);
            //}
            LMSEntities1 db = new LMSEntities1();
            var StatusCount = db.sp_TraineeDashboardDetails(TraineeID);
            foreach (var items in StatusCount)
            {
                TraineeExaminationOverview exm = new TraineeExaminationOverview();
                exm.CourseCompleted = (int)items.CoursesCompleted;
                exm.CourseAssigned = (int)items.CoursesAssigned;
                exm.CourseInProgress = (int)items.CoursesInProgress;
                exm.CertificateAcheived = (int)items.CertificateAcheived;
                listExm.Add(exm);
            }
            var ExamDetails = db.sp_TraineeExamOverview(TraineeID);
            foreach (var items in ExamDetails)
            {
                TraineeExaminationOverview exm = new TraineeExaminationOverview();
                exm.SiNo = (int)items.SiNo;
                exm.CourseID = (int)items.CourseID;
                exm.CourseName = items.Course;
                exm.ExamID = items.ExamID;
                exm.ExamName = items.Exam;
                exm.ExamStatus = items.ExamStatus;
                exm.Instructor = items.Instructor;
                exm.ExamCode = items.ExamCode;
                exm.ExamDate = (DateTime)items.ExamDate;
                exm.AttemptsRemaining = (int)items.ExamAttempts;
                listExm.Add(exm);
            }

            var source = db.sp_TraineeAssignmentSelectAll(1).ToList();
            var CourseStat = db.sp_TraineeCourseStatus_SelectData(1).ToList();
            string checkval = "";
            for (int i = 1; i < 9; i++)
            {
                TraineeExaminationOverview model = new TraineeExaminationOverview();
                var AsgnSubmit = source.Where(s => s.TraineeAttachDate != null && s.TraineeAttachDate.Value.Month == i).Count();
                model.IsAsgnCompleted = 1;
                model.AsgnSubmitted = AsgnSubmit.ToString();
                var AsgnPending = source.Where(s => s.TraineeAttachDate == null && s.AssignmentDate.Value.Month == i).Count();
                model.AsgnPending = AsgnPending.ToString();
                var CourseAssigned = CourseStat.Where(s => s.RegisteredDate != null && s.RegisteredDate.Value.Month == i && s.RegisteredDate.Value.Year == 2021 && (s.CourseStatus == 1 || s.CourseStatus == 2)).Count();
                model.CourseAssignedStatus = CourseAssigned.ToString();
                var CourseCompleted = CourseStat.Where(s => s.RegisteredDate != null && s.CompletedDate != null && s.CompletedDate.Value.Month == i && s.CompletedDate.Value.Year == 2021 && (s.CourseStatus == 3 || s.CourseStatus == 4)).Count();
                model.CourseCompletedStatus = CourseCompleted.ToString();
                checkval = checkval + model.CourseCompletedStatus;
                listExm.Add(model);
            }

            var ExamNotification = db.sp_TraineeExamOverview(1).ToList();
            var notify = ExamNotification.Where(s => s.ExamDate != null && s.ExamDate > DateTime.Now).ToList();
            foreach (var items in notify)
            {
                TraineeExaminationOverview model = new TraineeExaminationOverview();
                model.ExamNameNotify = items.Exam;
                model.ExamDateNotify = (DateTime)items.ExamDate;
                listExm.Add(model);
            }

            var AssignNotification = source.Where(s => s.TraineeAttachDate != null).ToList();
            foreach (var items in AssignNotification)
            {
                TraineeExaminationOverview model = new TraineeExaminationOverview();
                model.AssignmentNotifyPending = 1;
                model.AssignInstructorNameNotify = items.InstructorName;
                model.AssignmentNameNotify = items.AssignmentName;
                model.AssignmentDueDateNotify = (DateTime)items.DueDate;
                model.InstructorImageNotify = items.InstructorImage;
                listExm.Add(model);
            }
            return View(listExm);
        }

    }
}