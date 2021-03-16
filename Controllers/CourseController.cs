using NSILearningManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace NSILearningManagementSystem.Controllers
{
    public class CourseController : Controller
    {
        //public ActionResult Index3()
        //{
        //    List<M_Coursevm> listCourse = new List<M_Coursevm>();
        //    ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
        //    var Coursedet = obj.GetCourse();
        //    foreach (var details in Coursedet)
        //    {
        //        M_Coursevm Coursevm = new M_Coursevm();
        //        Coursevm.CourseName = details.CourseName;
        //        //Coursevm.CourseName = details.co;
        //        //Coursevm.CourseCode = item;

        //    }
        //    return View(Coursedet);
        //}
       
        public ActionResult Index()
        {
            List<M_Coursevm> listcrse = new List<M_Coursevm>();
            ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            var crs = obj.GetCourses(1);
            foreach (DataRow itms in crs.CrseTable.Rows)
            {
                M_Coursevm crse = new M_Coursevm();
                crse.CourseID = Convert.ToInt32(itms["CourseID"].ToString());
                crse.CourseName = itms["CourseName"].ToString();
                crse.CourseCode = itms["CourseCode"].ToString();
                crse.CourseCost = Convert.ToDecimal(itms["CourseCost"].ToString());
                crse.CourseDurationUnit = itms["CourseDurationUnit"].ToString();
                crse.CourseDurationValue = Convert.ToDecimal(itms["CourseDurationValue"].ToString());
                string CourseStartDate = (itms["CourseStartDate"].ToString() == " " ? "1900-01-01" : itms["CourseStartDate"].ToString());
                crse.CourseStartDate = Convert.ToDateTime(CourseStartDate);
                crse.CourseStartDateMonth = itms["CourseStartDateMonth"].ToString();
                crse.CourseImage = itms["CourseImage"].ToString();
                var instrcrs = obj.GetInstructorCourses(0, crse.CourseID);
                if (instrcrs.InstrCrseTable.Rows.Count > 0)
                {
                    crse.InstructorName = instrcrs.InstrCrseTable.Rows[0]["Instructor"].ToString();
                }
                var studcrs = obj.GetInstructorCourses(1, crse.CourseID);
                if (studcrs.InstrCrseTable.Rows.Count > 0)
                {
                    crse.StudentCount = Convert.ToInt32(studcrs.InstrCrseTable.Rows[0]["StudentCount"].ToString());
                }

                listcrse.Add(crse);
            }
            return View(listcrse);
        }
        // GET: Course
        //public ActionResult Index1()
        //{
        //    List<M_Coursevm> listCourse = new List<M_Coursevm>();
        //    ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
        //    var Courses = obj.GetCourses();
        //    foreach (var item in Courses)
        //    {
        //        M_Coursevm Coursevm = new M_Coursevm();
        //        Coursevm.CourseCode = item.CourseCode;
        //        //Coursevm.CourseCost = item.CourseCost;
        //        //Coursevm.CourseDurationUnit = item.CourseDurationUnit;
        //        //Coursevm.CourseDurationValue = item.CourseDurationValue;
        //        //Coursevm.CourseEndDate = item.CourseEndDate;
        //        Coursevm.CourseID = item.CourseID;
        //        Coursevm.CourseName = item.CourseName;
        //        //Coursevm.CourseStartDate = item.CourseStartDate;
        //        //Coursevm.Description = item.Description;
        //        //Coursevm.InstitutionID = item.InstitutionID;
        //        //Coursevm.MaxAttempt = item.MaxAttempt;
        //        //Coursevm.NoofSeats = item.NoofSeats;
        //        //Coursevm.TrainingValidityUnit = item.TrainingValidityUnit;
        //        //Coursevm.TraniningValidity = item.TraniningValidity;
        //        listCourse.Add(Coursevm);
        //    }
        //    return View(listCourse);
        //}
        public ActionResult DisplayMessage(string msg)
        {
            ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            ViewBag.ValueMsg = obj.GetValue(msg);
            return View();
        }

    }
}