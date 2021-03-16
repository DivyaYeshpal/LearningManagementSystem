using NSILearningManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NSILearningManagementSystem.Controllers
{
    public class SurveyController : Controller
    {
        // GET: Survey
        public ActionResult CourseInstructorEvaluation()
        {
            List<Survey> Listcrse = new List<Survey>();
            ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            var crs = obj.SelectCourseTrainee(1);
            foreach (DataRow itms in crs.TraineeCrseTable.Rows)
            {
                Survey _surv = new Survey();
                if (crs.TraineeCrseTable.Rows.Count > 0)
                {
                    _surv.CourseID = Convert.ToInt32(itms["CourseID"].ToString());
                    _surv.CourseName = itms["CourseName"].ToString();
                    _surv.InstructorID = Convert.ToInt32(itms["InstructorID"].ToString());
                    _surv.Instructor = itms["Instructor"].ToString();
                    Listcrse.Add(_surv);
                }
                ViewBag.Programs = new SelectList(Listcrse, "CourseID", "CourseName");
                ViewBag.Instructors = new SelectList(Listcrse, "InstructorID", "Instructor");
            }
            List<Survey> Listsrvy = new List<Survey>();
            List<Survey> Listmodel = new List<Survey>();
            var survey = obj.SelectSurveyQuestions();
            foreach (DataRow itms in survey.TraineeSurveyLinkTable.Rows)
            {
                Survey _surv = new Survey();
                if (survey.TraineeSurveyLinkTable.Rows.Count > 0)
                {
                    _surv.QuestionID = Convert.ToInt32(itms["QuestionID"].ToString());
                    _surv.Question = itms["Question"].ToString();
                    _surv.QuestionCategoryID = Convert.ToInt32(itms["CategoryID"].ToString());
                    _surv.QuestionCategory = itms["Category"].ToString();
                    _surv.SubCategory = itms["SubCategory"].ToString();
                    _surv.SiNo = Convert.ToInt32(itms["SiNo"].ToString());
                    _surv.Option1 = itms["Option1"].ToString();
                    _surv.Option2 = itms["Option2"].ToString();
                    _surv.Option3 =itms["Option3"].ToString();
                    _surv.Option4 = itms["Option4"].ToString();
                    _surv.Option5 = itms["Option5"].ToString();
                    Listsrvy.Add(_surv);
                }
                var surv = Listsrvy.Where(s => s.QuestionCategoryID == 1).ToList();
                //Listmodel.Add(surv);
            }
            return View(Listsrvy);
        }
    }
}