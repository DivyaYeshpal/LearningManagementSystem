using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NSILearningManagementSystem.Models;
namespace NSILearningManagementSystem.Controllers
{
    public class TraineeCourseController : Controller
    {
        LMSEntities1 db = new LMSEntities1();
        // GET: TraineeCourse
        public ActionResult Index()
        {
            int TraineeID = 1;
            //List<TraineeCourse> listCrs = new List<TraineeCourse>();
            //ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            //var traineecrs = obj.SelectCourseTrainee(TraineeID);
            //foreach (DataRow itms in traineecrs.TraineeCrseTable.Rows)
            //{
            //    TraineeCourse trcrs = new TraineeCourse();
            //    if (traineecrs.TraineeCrseTable.Rows.Count > 0)
            //    {
            //        trcrs.Instructor = itms["Instructor"].ToString();
            //        trcrs.CourseName = itms["CourseName"].ToString();
            //        trcrs.Trainee = itms["Trainee"].ToString();
            //        trcrs.ModuleCount = Convert.ToInt32(itms["ModuleCount"].ToString());
            //        trcrs.CourseID = Convert.ToInt32(itms["CourseID"].ToString());
            //        trcrs.TraineeID = Convert.ToInt32(itms["TraineeID"].ToString());
            //    }
            //    listCrs.Add(trcrs);
            //}
            //ServiceReference1.CourseServiceClient obj1 = new ServiceReference1.CourseServiceClient();
            //var traineecrsstatus = obj1.SelectTraineeDashboardDetails(TraineeID);
            //foreach (DataRow itms in traineecrsstatus.TraineeCrseStatusTable.Rows)
            //{
            //    TraineeCourse trcrs = new TraineeCourse();
            //    if (traineecrsstatus.TraineeCrseStatusTable.Rows.Count > 0)
            //    {
            //        trcrs.CourseCompleted = Convert.ToInt32(itms["CoursesCompleted"].ToString());
            //        trcrs.CourseAssigned = Convert.ToInt32(itms["CoursesAssigned"].ToString());
            //        trcrs.CourseInProgress = Convert.ToInt32(itms["CoursesInProgress"].ToString());
            //        trcrs.CertificateAcheived = Convert.ToInt32(itms["CertificateAcheived"].ToString());
            //    }
            //    listCrs.Add(trcrs);
            //}

            var StatusCount = db.sp_TraineeDashboardDetails(TraineeID);
            List<TraineeCourse> listTrs = new List<TraineeCourse>();
            foreach (var items in StatusCount)
            {
                TraineeCourse cs = new TraineeCourse();
                cs.CourseCompleted = (int)items.CoursesCompleted;
                cs.CourseAssigned = (int)items.CoursesAssigned;
                cs.CourseInProgress = (int)items.CoursesInProgress;
                cs.CertificateAcheived = (int)items.CertificateAcheived;
                listTrs.Add(cs);
            }
            var TraineeCourse = db.sp_CourseTraineeSelect(TraineeID);
            //List<TraineeCourse> listTrs = new List<TraineeCourse>();
            foreach (var item in TraineeCourse)
            {
                TraineeCourse tc = new TraineeCourse();
                tc.CourseID = (int)item.CourseID;
                tc.CourseName = item.CourseName;
                tc.Instructor = item.Instructor;
                tc.ModuleCount = Convert.ToInt32(item.ModuleCount);
                tc.Trainee = item.Trainee;
                tc.TraineeID = item.TraineeID;
                listTrs.Add(tc);
            }
            return View(listTrs);
        }
        public FileResult DownloadFile(string CourseDocument)
        {
            if (CourseDocument != null)
            {
                //Build the File Path.
                string path = Server.MapPath(CourseDocument);

                string ext = System.IO.Path.GetExtension(path);

                //Read the File data into Byte Array.
                byte[] bytes = System.IO.File.ReadAllBytes(path);

                string strFileExtension = System.IO.Path.GetExtension(path);
                if (strFileExtension == ".pdf")
                {
                    //Send the File to Download.
                    return File(bytes, "application/octet-stream", CourseDocument);
                }
                //else
                //{
                //    return RedirectToAction("NotFound");
                //}
            }
            return File(Url.Content("~/CourseDocument/dureceipt.pdf"), "application/pdf", "dureceipt.pdf");
        }
        //public ActionResult DownloadFile()
        //{
        //    string filename = "/UploadedFiles/jjjj.pdf";
        //    ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
        //    var stream = obj.Download(filename);
        //    StreamReader reader = new StreamReader(stream);
        //    Response.Write(reader.ReadToEnd());
        //    return View();
        //}
        //public ActionResult UploadFile()
        //{
        //    ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
        //    var fileName = obj.Upload('/UploadedFiles/jjjj.pdf');
        //    Session["file"] = fileName;
        //}
        public ActionResult CourseDetails(int CourseID,int TraineeID)
        
        {
            //List<TraineeCourse> listCrs = new List<TraineeCourse>();
            //ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            //var crs = obj.GetCourses(CourseID);
            //foreach (DataRow itms in crs.CrseTable.Rows)
            //{
            //    TraineeCourse trcrs = new TraineeCourse();
            //    if (crs.CrseTable.Rows.Count > 0)
            //    {
            //        trcrs.CourseName = itms["CourseName"].ToString();
            //        trcrs.ModuleCount = Convert.ToInt32(itms["ModuleCount"].ToString());
            //        trcrs.Instructor = itms["Instructor"].ToString();
            //    }
            //    listCrs.Add(trcrs);
            //}
            //ServiceReference1.CourseServiceClient obj2 = new ServiceReference1.CourseServiceClient();
            //var traineecrsstatus = obj2.SelectTraineeDashboardDetails(TraineeID);
            //foreach (DataRow itms in traineecrsstatus.TraineeCrseStatusTable.Rows)
            //{
            //    TraineeCourse trcrs = new TraineeCourse();
            //    if (traineecrsstatus.TraineeCrseStatusTable.Rows.Count > 0)
            //    {
            //        trcrs.CourseCompleted = Convert.ToInt32(itms["CoursesCompleted"].ToString());
            //        trcrs.CourseAssigned = Convert.ToInt32(itms["CoursesAssigned"].ToString());
            //        trcrs.CourseInProgress = Convert.ToInt32(itms["CoursesInProgress"].ToString());
            //        trcrs.CertificateAcheived = Convert.ToInt32(itms["CertificateAcheived"].ToString());
            //    }
            //    listCrs.Add(trcrs);
            //}
            //ServiceReference1.CourseServiceClient obj1 = new ServiceReference1.CourseServiceClient();
            //var mdlcrs = obj1.SelectModuleTrainee(CourseID);
            //foreach (DataRow itms in mdlcrs.TraineeMdlTable.Rows)
            //{
            //    TraineeCourse trcrs = new TraineeCourse();
            //    if (crs.CrseTable.Rows.Count > 0)
            //    {
            //        trcrs.ModuleName = itms["Module"].ToString();
            //        trcrs.Description = itms["Description"].ToString();

            //    }
            //    listCrs.Add(trcrs);
            //}
            //return View(listCrs);

            List<TraineeCourse> listCrs = new List<TraineeCourse>();
            var crs = db.sp_CourseSelect(CourseID);
            foreach (var itms in crs)
            {
                TraineeCourse trcrs = new TraineeCourse();
                trcrs.CourseName = itms.CourseName;
                trcrs.ModuleCount = Convert.ToInt32(itms.ModuleCount);
                trcrs.Instructor = itms.Instructor;
                trcrs.CourseDocument = itms.CourseDocument;
                listCrs.Add(trcrs);
            }
            var StatusCount = db.sp_TraineeDashboardDetails(TraineeID);
            foreach (var itms in StatusCount)
            {
                TraineeCourse trcrs = new TraineeCourse();
                trcrs.CourseCompleted = (int)itms.CoursesCompleted;
                trcrs.CourseAssigned = (int)itms.CoursesAssigned;
                trcrs.CourseInProgress = (int)itms.CoursesInProgress;
                trcrs.CertificateAcheived = (int)itms.CertificateAcheived;
                listCrs.Add(trcrs);
            }

            var mdl = db.sp_CourseModuleSelect(CourseID);
            foreach (var itms in mdl)
            {
                TraineeCourse trcrs = new TraineeCourse();
                trcrs.ModuleName = itms.Module;
                trcrs.Description = itms.Description;
                listCrs.Add(trcrs);
            }
            return View(listCrs);
        }

    }
}