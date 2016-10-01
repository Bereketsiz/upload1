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
        private ResimDataEntities RsmYukle = new ResimDataEntities();//Entity mize verdiğimiz isim burdan bir nesne oluşturuyoruz.

        public ActionResult Index()
        {
            return View(RsmYukle.ResimYukle.ToList());//ResimYukle tablomuzun adı.Listeleme yapıyoruz.
        }

        public ActionResult Ekle()
        {
            return View();
        }
        //Burada resim ekleme işlemi yapıyoruz.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ekle(HttpPostedFileBase resimDosyasi)
        {
            try
            {
                if (resimDosyasi.ContentLength > 0)
                {
                    var resimAdi = Path.GetFileName(resimDosyasi.FileName);
                    var resimYolu = Path.Combine(Server.MapPath("~/App_Data/Resimler"), resimAdi);
                    resimDosyasi.SaveAs(resimYolu);

                    ResimYukle resimDosya = new ResimYukle();
                    resimDosya.resimAdi = Path.GetFileName(resimDosyasi.FileName);
                    resimDosya.resimUzanti = Path.GetExtension(resimYolu);
                    resimDosya.resimBoyutu = resimDosyasi.ContentLength.ToString();
                    resimDosya.resimTuru = resimDosyasi.ContentType;
                    RsmYukle.ResimYukle.AddObject(resimDosya);
                    RsmYukle.SaveChanges();
                }
            }
            catch (Exception exeption)
            {

                exeption.Message.ToString();
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            return View(RsmYukle.ResimYukle.ToList().Where(x => x.resimId == id).SingleOrDefault());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Delete(int id,FormCollection collect)
        {
            try
            {
                ResimYukle resimDosya = new ResimYukle();
                resimDosya = RsmYukle.ResimYukle.ToList().Where(x => x.resimId == id).SingleOrDefault();
                RsmYukle.ResimYukle.DeleteObject(resimDosya);
                RsmYukle.SaveChanges();
                return View("Index", RsmYukle.ResimYukle.ToList());
            }
            catch (Exception exeption)
            {

                exeption.Message.ToString();
            }
            return View("Index", RsmYukle.ResimYukle.ToList());
        }

        public ActionResult Detay(int id)
        {
            return View(RsmYukle.ResimYukle.ToList().Where(x => x.resimId == id).SingleOrDefault());
        }
    }
}
