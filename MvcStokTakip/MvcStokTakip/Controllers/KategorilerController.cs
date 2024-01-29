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
    public class KategorilerController : Controller
    {
        // GET: Kategoriler
        MvcDbStokEntities db = new MvcDbStokEntities();
        public ActionResult Index(int sayfa = 1)
        {
            var MesajSilSuccess = TempData["MesajSilSuccess"];
            var MesajSilFail = TempData["MesajSilFail"];
            var KategoriEklemeBaşarılı = TempData["KategoriEklemeBaşarılı"];
            var KategoriGuncellendi = TempData["KategoriGuncellendi"];
            if (MesajSilSuccess != null || MesajSilFail !=null || KategoriEklemeBaşarılı != null || KategoriGuncellendi != null)
            {
                ViewBag.Mesaj = 1;

                if (MesajSilFail != null)
                {
                    ViewBag.MesajSilFail = 1;

                }
                else if(MesajSilSuccess != null)
                {
                    ViewBag.MesajSilSuccess = 1;

                }
                else if (KategoriEklemeBaşarılı != null)
                {
                    ViewBag.KategoriEklemeBaşarılı = 1;

                }
                else if (KategoriGuncellendi != null)
                {
                    ViewBag.KategoriGuncellendi = 1;
                }
            }

            // var degerler = db.TBLKATEGORILER.ToList();
            var degerler = db.TBLKATEGORILER.ToList().ToPagedList(sayfa,5);
            return View(degerler);
        }

        [HttpGet]
        public ActionResult YeniKategori()
        {
          
          
            return View();
        }
        [HttpPost]
        public ActionResult YeniKategori(TBLKATEGORILER P1)
        {
            if (!ModelState.IsValid)
            {
                return View("YeniKategori");
            }

            P1.KATEGORIAD = P1.KATEGORIAD.ToUpper();

            // Kategori adıyla eşleşen bir kategori var mı kontrol et
            var existingCategory = db.TBLKATEGORILER.FirstOrDefault(x => x.KATEGORIAD.ToUpper() == P1.KATEGORIAD.ToUpper());

            if (existingCategory != null)
            {
                // Eğer kategori zaten varsa, consola mesaj yaz
                ViewBag.KategoriVar = 1;

                return View("YeniKategori");

            }
            else
            {
                // Kategori zaten yoksa, ekleyerek devam et
                db.TBLKATEGORILER.Add(P1);
                db.SaveChanges();
                TempData["KategoriEklemeBaşarılı"] = 1;
                return RedirectToAction("Index");


            }


        }


        public ActionResult SIL(int id)
        {
        

            var kategori = db.TBLKATEGORILER.Find(id);
            var digerTabloReferans = db.TBLURUNLER.FirstOrDefault(x => x.URUNKATEGORI == id);

            if(digerTabloReferans != null)
            {
                TempData["MesajSilFail"] = 1;
                return RedirectToAction("Index");

            }
            else
            {
                db.TBLKATEGORILER.Remove(kategori);
                db.SaveChanges();
                TempData["MesajSilSuccess"] = 1;

                return RedirectToAction("Index");

            }

        }

   
        public ActionResult KategoriGetir(int id)
        {
            var ktgr = db.TBLKATEGORILER.Find(id);
            return View("KategoriGetir",ktgr);
        }

        public ActionResult Guncelle(TBLKATEGORILER p1)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("KategoriGetir/"+p1.KATEGORIID);
            }

            var ktgr = db.TBLKATEGORILER.Find(p1.KATEGORIID);
            ktgr.KATEGORIAD = p1.KATEGORIAD;
            db.SaveChanges();
            TempData["KategoriGuncellendi"] = 1;

            return RedirectToAction("Index");
        }
    }
}