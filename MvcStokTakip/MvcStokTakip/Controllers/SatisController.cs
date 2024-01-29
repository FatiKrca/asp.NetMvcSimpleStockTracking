using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcStokTakip.Models.Entity;
using PagedList;

namespace MvcStokTakip.Controllers
{
    public class SatisController : Controller
    {
        MvcDbStokEntities db = new MvcDbStokEntities();

        // GET: Satis
        public ActionResult Index(int sayfa = 1)
        {

            var deger = db.TBLSATISLAR.ToList().ToPagedList(sayfa, 5);

            return View(deger);
        }

        [HttpGet]
        public ActionResult YeniSatis()
        {
            return View();
        }

        [HttpPost]
        public ActionResult YeniSatis(TBLSATISLAR model)
        {
            if (ModelState.IsValid)
            {
                // Model geçerli ise veritabanına ekleme işlemi yapılabilir
                db.TBLSATISLAR.Add(model);
                db.SaveChanges();

                // Başarılı ekleme sonrasında başka bir sayfaya yönlendirme
                return RedirectToAction("Index");
            }

            // Model geçerli değilse aynı sayfaya geri dön
            return View(model);
        }

        public ActionResult SIL(int id)
        {

            var urun = db.TBLSATISLAR.Find(id);
            var digerTabloReferans = db.TBLSATISLAR.FirstOrDefault(x => x.URUN == id);
       
                db.TBLSATISLAR.Remove(urun);
                db.SaveChanges();

                return RedirectToAction("Index");
            


        }
    }
}
