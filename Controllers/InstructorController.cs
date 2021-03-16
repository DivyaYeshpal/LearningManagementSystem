using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NSILearningManagementSystem.Models;

namespace NSILearningManagementSystem.Controllers
{
    public class InstructorController : Controller
    {
        // GET: Instructor
        
        public ActionResult Index()
        {
            string InstructorID = Session["UserID"].ToString();
            LMSEntities1 db = new LMSEntities1();
            //List<Instructor> listInstrmdl = new List<Instructor>();
            //ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            //var instrcrsmdl = obj.GetInstructorCourseModuleDetails(0, InstructorID, 0);
            ////var instrcrsmdl = db.sp_InstructorCourseModuleSelect(0, InstructorID, 0);
            //foreach (DataRow itms in instrcrsmdl.InstrCrseMdleTable.Rows)
            //{
            //    Instructor instr = new Instructor();
            //    if (instrcrsmdl.InstrCrseMdleTable.Rows.Count > 0)
            //    {
            //        instr.CourseID = Convert.ToInt32(itms["CourseID"].ToString());
            //        instr.CourseName = itms["CourseName"].ToString();
            //    }
            //    var instrlink = obj.GetInstructorCourseModuleDetails(1, InstructorID, instr.CourseID);
            //    foreach (DataRow det in instrlink.InstrCrseMdleTable.Rows)
            //    {
            //        if (instrlink.InstrCrseMdleTable.Rows.Count > 0)
            //        {
            //            instr.LinkCourseID = Convert.ToInt32(det["CourseID"].ToString());
            //            instr.LinkCourseName = det["CourseName"].ToString();
            //            instr.LinkDescription = det["CourseDescription"].ToString();
            //            instr.TeamsLink = det["LinkDetails"].ToString();
            //            instr.DocAttachment = det["DocumentAttachment"].ToString();
            //        }
            //    }
            //    listInstrmdl.Add(instr);
            //}

            List<Instructor> listInstrmdl = new List<Instructor>();
            ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            var instrcrsmdl = db.sp_InstructorCourseModuleSelect(0,Convert.ToInt32(InstructorID), 0);
            foreach (var itms in instrcrsmdl)
            {
                Instructor instr = new Instructor();
                instr.CourseID = Convert.ToInt32(itms.CourseID.ToString());
                instr.CourseName = itms.CourseName.ToString();
                var instrlink = db.sp_InstructorCourseModuleLinkDetails(0, Convert.ToInt32(InstructorID), instr.CourseID);
                foreach (var det in instrlink)
                {
                    instr.LinkCourseID = Convert.ToInt32(det.CourseID.ToString());
                    instr.LinkCourseName = det.CourseName.ToString();
                    instr.LinkDescription = det.CourseDescription.ToString();
                    instr.TeamsLink = det.LinkDetails.ToString();
                    instr.DocAttachment = det.DocumentAttachment.ToString();
                }
                listInstrmdl.Add(instr);
            }
            return View(listInstrmdl);
        }
        public ActionResult CourseDetails(int CourseID)
        {
            //List<InstructorCourse> listCrs = new List<InstructorCourse>();
            //ServiceReference1.CourseServiceClient obj = new ServiceReference1.CourseServiceClient();
            //var crs = obj.GetCourses(CourseID);
            //foreach (DataRow itms in crs.CrseTable.Rows)
            //{
            //    InstructorCourse incrs = new InstructorCourse();
            //    if (crs.CrseTable.Rows.Count > 0)
            //    {
            //        incrs.CourseName = itms["CourseName"].ToString();
            //        incrs.ModuleCount = Convert.ToInt32(itms["ModuleCount"].ToString());
            //        incrs.Instructor = itms["Instructor"].ToString();
            //        incrs.CourseDocument = itms["CourseDocument"].ToString();
            //    }
            //    listCrs.Add(incrs);
            //}
            //ServiceReference1.CourseServiceClient obj1 = new ServiceReference1.CourseServiceClient();
            //var mdlcrs = obj1.SelectModuleTrainee(CourseID);
            //foreach (DataRow itms in mdlcrs.TraineeMdlTable.Rows)
            //{
            //    InstructorCourse incrs = new InstructorCourse();
            //    if (mdlcrs.TraineeMdlTable.Rows.Count > 0)
            //    {
            //        incrs.ModuleName = itms["Module"].ToString();
            //        incrs.Description = itms["Description"].ToString();
            //    }
            //    listCrs.Add(incrs);
            //}
            LMSEntities1 db = new LMSEntities1();
            List<InstructorCourse> listCrs = new List<InstructorCourse>();
            var crs = db.sp_CourseSelect(CourseID);
            foreach (var itms in crs)
            {
                InstructorCourse incrs = new InstructorCourse();
                incrs.CourseName = itms.CourseName.ToString();
                incrs.ModuleCount = Convert.ToInt32(itms.ModuleCount.ToString());
                incrs.Instructor = itms.Instructor.ToString();
                incrs.CourseDocument = itms.CourseDocument.ToString();
                listCrs.Add(incrs);
            }
            var mdlcrs = db.sp_CourseModuleSelect(CourseID);
            foreach (var itms in mdlcrs)
            {
                InstructorCourse incrs = new InstructorCourse();
                incrs.ModuleName = itms.Module.ToString();
                incrs.Description = itms.Description.ToString();
                listCrs.Add(incrs);
            }
            return View(listCrs);
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
    }
}