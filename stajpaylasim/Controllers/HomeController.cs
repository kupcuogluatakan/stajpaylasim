using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using stajpaylasim.Models;

namespace stajpaylasim.Controllers
{
    public class HomeController : Controller
    {
        StudentInternEntities StudentIntern = new StudentInternEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            if (!String.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                FormsAuthentication.SignOut();
                return View();
            }
            else
            {
                
                //var userMail = HttpContext.User.Identity.Name;
                var userStudent = StudentIntern.Student.Where(s => s.STc == login.Tc && s.SPass == login.Pass).SingleOrDefault();
                var userTeacher = StudentIntern.Teacher.Where(t => t.TTc == login.Tc && t.TPass == login.Pass).SingleOrDefault();

                if (userStudent != null)
                {
                    FormsAuthentication.SetAuthCookie(login.Tc, true);
                    Session["SId"] = userStudent.SId;
                    Session["Sad"] = userStudent.SAdSoyad;
                    return RedirectToAction("Student", "Home");
                }
                else if (userTeacher != null)
                {
                    FormsAuthentication.SetAuthCookie(login.Tc, true);
                    Session["TId"] = userTeacher.TId;
                    Session["Tad"] = userTeacher.TAdSoyad;
                    return RedirectToAction("Teacher", "Home");
                }
            }
            return View();
        }
        [Authorize]
        public ActionResult Teacher()
        {
            int DocTId = Convert.ToInt32(Session["TId"].ToString());

            ViewBag.DocState = new SelectList(StudentIntern.DocStates, "DsId", "DSAck");
            var q1 = from d in StudentIntern.Documents
                     join t in StudentIntern.Teacher on d.TId equals t.TId
                     join s in StudentIntern.Student on d.SId equals s.SId
                     join dt in StudentIntern.DocTitle on d.DTId equals dt.DTId
                     join ds in StudentIntern.DocStates on d.DSId equals ds.DSId
                     where t.TId == DocTId 
                     select new DocTeacher
                     {
                         DocId = d.DId,
                         SNameSurname = s.SAdSoyad,
                         Doctitle = dt.DTAck,
                         DocDesc = d.DDesc,
                         DocState = ds.DSAck,
                         DocName = d.DName,
                         DSId = (int)d.DSId
                     };

            return View(q1.ToList());
        }
        [Authorize]
        [HttpPost]
        public ActionResult Teacher(FormCollection Nesneler)
        {
            int DSid = Convert.ToInt32(Nesneler["DsId"]);
            int DocId = Convert.ToInt32(Nesneler["DocId"]);
            int DocTId = Convert.ToInt32(Session["TId"].ToString());

            Documents doc = StudentIntern.Documents.Where(d => d.DId == DocId).SingleOrDefault();
            doc.DSId = DSid;
            StudentIntern.SaveChanges();
            ViewBag.productName = "";
            ViewBag.Success = "Staj Durumu Başarıyla Güncellenmiştir.";

            ViewBag.DocState = new SelectList(StudentIntern.DocStates, "DsId", "DSAck");

            var q1 = from d in StudentIntern.Documents
                     join t in StudentIntern.Teacher on d.TId equals t.TId
                     join s in StudentIntern.Student on d.SId equals s.SId
                     join dt in StudentIntern.DocTitle on d.DTId equals dt.DTId
                     join ds in StudentIntern.DocStates on d.DSId equals ds.DSId
                     where t.TId == DocTId
                     select new DocTeacher
                     {
                         DocId = d.DId,
                         SNameSurname = s.SAdSoyad,
                         Doctitle = dt.DTAck,
                         DocDesc = d.DDesc,
                         DocState = ds.DSAck,
                         DocName = d.DName
                     };

            return View(q1.ToList());
        }
        [Authorize]
        public ActionResult Student()
        {
            ViewBag.Title = new SelectList(StudentIntern.DocTitle, "DTId", "DTAck");
            ViewBag.Teacher = new SelectList(StudentIntern.Teacher, "TId", "TAdSoyad");

            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Student(StudentDoc studentDoc)
        {

            if (studentDoc.DTId > 0 && !(String.IsNullOrEmpty(studentDoc.DocDesc))  && studentDoc.SDoc != null && studentDoc.SDoc.ContentLength > 0)
            {
                studentDoc.DocSId = Convert.ToInt32(Session["SId"].ToString());
                try
                {
                    string StudentFilename = ((DateTime.Now.ToString("dd_MM_yyyy_HH:mm")).Replace(":", "") + studentDoc.SDoc.FileName);
                    Documents documents = new Documents();
                    documents.DTId = studentDoc.DTId;
                    documents.DDesc = studentDoc.DocDesc;
                    documents.DName = StudentFilename;
                    documents.DURL = StudentFilename;
                    documents.TId = studentDoc.TId;
                    documents.SId = studentDoc.DocSId;
                    documents.DSId = 1;
                    StudentIntern.Documents.Add(documents);
                    StudentIntern.SaveChanges();
                    string Upath = Path.Combine(HttpContext.Server.MapPath("~/Content/Documents"), Path.GetFileName(StudentFilename));
                    studentDoc.SDoc.SaveAs(Upath);

                    ViewBag.company = "";
                    ViewBag.Success += "Staj Dosyaniz Başarıyla Eklenmiştir.";

                    ViewBag.Title = new SelectList(StudentIntern.DocTitle, "DTId", "DTAck");
                    ViewBag.Teacher = new SelectList(StudentIntern.Teacher, "TId", "TAdSoyad");
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
                ViewBag.Success = "";

            ViewBag.Title = new SelectList(StudentIntern.DocTitle, "DTId", "DTAck");
            ViewBag.Teacher = new SelectList(StudentIntern.Teacher, "TId", "TAdSoyad");


            return View();
        }

        
        public ActionResult LogOut()
        {
            //Session.Abandon();
            //Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        //public ActionResult LogOff()
        //{
        //    FormsAuthentication.SignOut();
        //    return RedirectToAction("Login");
        //}

        //[HttpPost]
        //public ActionResult LogOut(Login login)
        //{
        //    login.Tc = null;
        //    login.Pass = null;
        //    return RedirectToAction("Login", "Home");
        //}

    }
}