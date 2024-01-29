using MvcStokTakip.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcStokTakip.Models.Entity;
using PagedList;
using PagedList.Mvc;


namespace MvcStokTakip.Controllers
{
    public class MusteriController : Controller
    {
        // GET: Musteri
        MvcDbStokEntities db = new MvcDbStokEntities();

        public ActionResult Index(int sayfa = 1)
        {
            var MesajSilSuccess = TempData["MesajSilSuccess"];
            var MesajSilFail = TempData["MesajSilFail"];
            var MusteriEklemeBaşarılı = TempData["MusteriEklemeBaşarılı"];
            var MusteriGuncellemeBaşarılı = TempData["MusteriGuncellemeBaşarılı"];
            if (MesajSilSuccess != null || MesajSilFail != null || MusteriEklemeBaşarılı != null || MusteriGuncellemeBaşarılı !=null)
            {
                ViewBag.Mesaj = 1;

                if (MesajSilFail != null)
                {
                    ViewBag.MesajSilFail = 1;

                }
                else if (MesajSilSuccess != null)
                {
                    ViewBag.MesajSilSuccess = 1;

                }
                else if (MusteriEklemeBaşarılı != null)
                {
                    ViewBag.MusteriEklemeBaşarılı = 1;

                }
                else if (MusteriGuncellemeBaşarılı != null)
                {
                    ViewBag.MusteriGuncellemeBaşarılı = 1;

                }
            }
            //var deger = db.TBLMUSTERILER.ToList();
            var deger = db.TBLMUSTERILER.ToList().ToPagedList(sayfa, 5);


            return View(deger);
        }

        [HttpGet]
        public ActionResult YeniMusteri()
        {

            return View();
        }
        [HttpPost]
        public ActionResult YeniMusteri(TBLMUSTERILER P1)
        {
            if (!ModelState.IsValid)
            {
                return View("YeniMusteri");
            }

            P1.MUSTERIAD = P1.MUSTERIAD.ToUpper();
            P1.MUSTERISOYAD = P1.MUSTERISOYAD.ToUpper();


            // Musteri adıyla eşleşen bir Musteri var mı kontrol et
            var existingMusteri = db.TBLMUSTERILER.FirstOrDefault(x => x.MUSTERIAD.ToUpper() == P1.MUSTERIAD.ToUpper() && x.MUSTERISOYAD.ToUpper() == P1.MUSTERISOYAD.ToUpper());

            if (existingMusteri != null)
            {
                // Eğer Musteri zaten varsa, consola mesaj yaz
                ViewBag.MusteriVar = 1;

                return View("YeniMusteri");

            }
            else
            {
                // Musteri zaten yoksa, ekleyerek devam et
                db.TBLMUSTERILER.Add(P1);
                db.SaveChanges();
                TempData["MusteriEklemeBaşarılı"] = 1;
                return RedirectToAction("Index");


            }


        }
        public ActionResult SIL(int id)
        {


            var musteri = db.TBLMUSTERILER.Find(id);
            var digerTabloReferans = db.TBLSATISLAR.FirstOrDefault(x => x.MUSTERI == id);

            if (digerTabloReferans != null)
            {
                TempData["MesajSilFail"] = 1;
                return RedirectToAction("Index");

            }
            else
            {
                db.TBLMUSTERILER.Remove(musteri);
                db.SaveChanges();
                TempData["MesajSilSuccess"] = 1;

                return RedirectToAction("Index");

            }

        }
        public ActionResult MusteriGetir(int id)
        {
            var mus = db.TBLMUSTERILER.Find(id);
            return View("MusteriGetir", mus);
        }
        public ActionResult Guncelle(TBLMUSTERILER p1)
        {
            var ktgr = db.TBLMUSTERILER.Find(p1.MUSTERIID);
            ktgr.MUSTERIAD = p1.MUSTERIAD;
            ktgr.MUSTERISOYAD = p1.MUSTERISOYAD;
            db.SaveChanges();
            TempData["MusteriGuncellemeBaşarılı"] = 1;


            return RedirectToAction("Index");
        }
    }
}