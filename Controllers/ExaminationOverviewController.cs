using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NSILearningManagementSystem.Models;

namespace NSILearningManagementSystem.Controllers
{
    public class ExaminationOverviewController : Controller
    {
        // GET: ExaminationOverview
        LMSEntities1 db = new LMSEntities1();
        public ActionResult Index()
        {
            int InstructorID = 1;
            List <ExaminationOverview> Overview = new List<ExaminationOverview>();
            var InstructorDashboard = db.sp_InstructorDashboardSelect(InstructorID);
            foreach (var itemss in InstructorDashboard)
            {
                ExaminationOverview exam = new ExaminationOverview();
                exam.TotalExamRemaining =Convert.ToInt32(itemss.TotalExamRemaining);
                exam.TotalExamCompleted = Convert.ToInt32(itemss.TotalExamCompleted);
                exam.TotalExamPassed = Convert.ToInt32(itemss.TotalExamPassed);
                exam.TotalExamFailed = Convert.ToInt32(itemss.TotalExamFailed);
                Overview.Add(exam);
            }
            var ExamList = db.sp_ExamOverview(InstructorID);
            foreach (var item in ExamList)
            {
                ExaminationOverview exam = new ExaminationOverview();
                exam.ExamID = item.ExamID;
                exam.Exam = item.Exam;
                exam.ExamCode = item.ExamCode;
                exam.ExamDate = item.ExamDate;
                exam.ExamStatus = item.ExamStatus;
                exam.Language = item.Language;
                exam.TraineeCount = (int)item.TraineeCount;
                exam.CourseName = item.Course;
                exam.SiNo =(long)item.SiNo;
                Overview.Add(exam);
            }
            var ExamParticipantList = db.sp_ExamParticipantsStatus();
            foreach (var items in ExamParticipantList)
            {
                ExaminationOverview examdet = new ExaminationOverview();
                examdet.Trainee = items.Trainee;
                examdet.TraineeImage = items.TraineeImage;
                examdet.EnrolledDate = items.EnrolledDate;
                examdet.CourseName = items.CourseName;
                examdet.Email = items.EmailID;
                if (items.Percentage != null)
                    examdet.Percentage = (decimal)items.Percentage;
                else
                    examdet.Percentage = 0;
                Overview.Add(examdet);
            }
            return View(Overview.ToList());
        }
    }
}