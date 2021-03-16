using NSILearningManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NSILearningManagementSystem.Controllers
{
    public class TraineeProgressController : Controller
    {
        // GET: TraineeProgress
        public ActionResult Index()
        {
            int TraineeID = 1;
            //List<TraineeProgress> listCrs = new List<TraineeProgress>();
            //ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            //var traineecrs = obj.SelectCourseTrainee(TraineeID);
            //foreach (DataRow itms in traineecrs.TraineeCrseTable.Rows)
            //{
            //    TraineeProgress trpgrs = new TraineeProgress();
            //    if (traineecrs.TraineeCrseTable.Rows.Count > 0)
            //    {
            //        trpgrs.CourseName = itms["CourseName"].ToString();
            //        trpgrs.CourseID = Convert.ToInt32(itms["CourseID"].ToString());
            //        trpgrs.CourseDocument= itms["CourseDocument"].ToString();
            //    }
            //    listCrs.Add(trpgrs);
            //}
            //ServiceReference1.CourseServiceClient obj1 = new ServiceReference1.CourseServiceClient();
            //var traineecrsstatus = obj1.SelectTraineeDashboardDetails(TraineeID);
            //foreach (DataRow itms in traineecrsstatus.TraineeCrseStatusTable.Rows)
            //{
            //    TraineeProgress trpgrs = new TraineeProgress();
            //    if (traineecrsstatus.TraineeCrseStatusTable.Rows.Count > 0)
            //    {
            //        trpgrs.CourseCompleted = Convert.ToInt32(itms["CoursesCompleted"].ToString());
            //        trpgrs.CourseAssigned = Convert.ToInt32(itms["CoursesAssigned"].ToString());
            //        trpgrs.CourseInProgress = Convert.ToInt32(itms["CoursesInProgress"].ToString());
            //        trpgrs.CertificateAcheived = Convert.ToInt32(itms["CertificateAcheived"].ToString());
            //    }
            //    listCrs.Add(trpgrs);
            //}
            //ServiceReference1.CourseServiceClient obj2 = new ServiceReference1.CourseServiceClient();
            //var traineecrslink = obj2.SelectTraineeCourseLink(TraineeID);
            //foreach (DataRow itms in traineecrslink.TraineeCrseLinkTable.Rows)
            //{
            //    TraineeProgress trpgrs = new TraineeProgress();
            //    if (traineecrslink.TraineeCrseLinkTable.Rows.Count > 0)
            //    {
            //        trpgrs.CourseName = itms["Course"].ToString();
            //        trpgrs.LinkDetails = itms["LinkDetails"].ToString(); 
            //        trpgrs.Instructor = itms["Instructor"].ToString();
            //        trpgrs.Duration = itms["Duration"].ToString();
            //        trpgrs.DocumentAttach= itms["DocumentAttachment"].ToString();
            //        trpgrs.InstructorImage = itms["InstructorImage"].ToString();
            //    }
            //    listCrs.Add(trpgrs);
            //}


            LMSEntities1 db = new LMSEntities1();
            List<TraineeProgress> listTrs = new List<TraineeProgress>();
            var traineecrs = db.sp_CourseTraineeSelect(TraineeID);
            foreach (var itms in traineecrs)
            {
                TraineeProgress trpgrs = new TraineeProgress();
                trpgrs.CourseName = itms.CourseName.ToString();
                trpgrs.CourseID = Convert.ToInt32(itms.CourseID.ToString());
                trpgrs.CourseDocument = itms.CourseDocument.ToString();
                listTrs.Add(trpgrs);
            }
            var StatusCount = db.sp_TraineeDashboardDetails(TraineeID);
            foreach (var items in StatusCount)
            {
                TraineeProgress cs = new TraineeProgress();
                cs.CourseCompleted = (int)items.CoursesCompleted;
                cs.CourseAssigned = (int)items.CoursesAssigned;
                cs.CourseInProgress = (int)items.CoursesInProgress;
                cs.CertificateAcheived = (int)items.CertificateAcheived;
                listTrs.Add(cs);
            }
            var traineecrslink = db.sp_TraineeCourseDetailsLink(TraineeID);
            foreach (var items in traineecrslink)
            {
                TraineeProgress trpgrs = new TraineeProgress();
                trpgrs.LinkDetails = items.LinkDetails.ToString();
                trpgrs.Instructor = items.Instructor.ToString();
                trpgrs.Duration = items.Duration.ToString();
                trpgrs.DocumentAttach = items.DocumentAttachment.ToString();
                trpgrs.InstructorImage = items.InstructorImage.ToString();
                listTrs.Add(trpgrs);
            }
            var source = db.sp_TraineeAssignmentSelectAll(1).ToList();
            var AssignNotification = source.Where(s => s.TraineeAttachDate != null).ToList();
            foreach (var items in AssignNotification)
            {
                TraineeProgress model = new TraineeProgress();
                model.AssignmentNotifyPending = 1;
                model.AssignInstructorNameNotify = items.InstructorName;
                model.AssignmentNameNotify = items.AssignmentName;
                model.AssignmentDueDateNotify = (DateTime)items.DueDate;
                model.InstructorImageNotify = items.InstructorImage;
                listTrs.Add(model);
            }
            return View(listTrs);
        }
        public FileResult DownloadFile(string CourseScheduleDocument)
        {
            if (CourseScheduleDocument != null)
            {
                //Build the File Path.
                string path = Server.MapPath(CourseScheduleDocument);

                string ext = System.IO.Path.GetExtension(path);

                //Read the File data into Byte Array.
                byte[] bytes = System.IO.File.ReadAllBytes(path);

                string strFileExtension = System.IO.Path.GetExtension(path);
                if (strFileExtension == ".pdf")
                {
                    //Send the File to Download.
                    return File(bytes, "application/octet-stream", CourseScheduleDocument);
                }
                //else
                //{
                //    return RedirectToAction("NotFound");
                //}
            }
            return File(Url.Content("~/CourseDocument/dureceipt.pdf"), "application/pdf", "dureceipt.pdf");
        }
    }
}