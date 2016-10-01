using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fileUpload_mvc.Models;
using System.Data.Objects.DataClasses;
using System.IO;

namespace fileUpload_mvc.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private dataEntities upload_db=new dataEntities();
        public ActionResult Index()
        {
            return View(upload_db.uploadfile.ToList());
        }

        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(HttpPostedFileBase file)
        {
            if(file.ContentLength>0)
            {
                var fileName=Path.GetFileName(file.FileName);
                var path=Path.Combine(Server.MapPath("~/App_Data/UpLoads"), fileName);
                
                file.SaveAs(path);
                uploadfile Dosya=new uploadfile();
                Dosya.dosyaAdi=Path.GetFileName(file.FileName);
                Dosya.dosyaUzanti=Path.GetExtension(path);
                Dosya.dosyaBoyutu=file.ContentLength.ToString();
                Dosya.dosyaTur=file.ContentType;
                upload_db.uploadfile.AddObject(Dosya);
                upload_db.SaveChanges();
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            return View(upload_db.uploadfile.ToList().Where(x=>x.dosyaId==id).SingleOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(int id,FormCollection collect)
        {
            uploadfile Dosya=new uploadfile();
            Dosya=upload_db.uploadfile.ToList().Where(x => x.dosyaId==id).SingleOrDefault();
            upload_db.uploadfile.DeleteObject(Dosya);
            upload_db.SaveChanges();
            return View("Index", upload_db.uploadfile.ToList());
        }

        public ActionResult Detay(int id)
        {
            return View(upload_db.uploadfile.ToList().Where(x => x.dosyaId==id).SingleOrDefault());
        }
    }
}
