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
    public class UrunlerController : Controller
    {
        // GET: urunler
        MvcDbStokEntities db = new MvcDbStokEntities();

        public ActionResult Index(int sayfa = 1)
        {
            var MesajSilSuccess = TempData["MesajSilSuccess"];
            var MesajSilFail = TempData["MesajSilFail"];
            var UrunEklemeBaşarılı = TempData["UrunEklemeBaşarılı"];
            var UrunGuncellemeBaşarılı = TempData["UrunGuncellemeBaşarılı"];
            if (MesajSilSuccess != null || MesajSilFail != null || UrunEklemeBaşarılı != null || UrunGuncellemeBaşarılı != null)
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
                else if (UrunEklemeBaşarılı != null)
                {
                    ViewBag.UrunEklemeBaşarılı = 1;

                }
                else if (UrunGuncellemeBaşarılı != null)
                {
                    ViewBag.UrunGuncellemeBaşarılı = 1;

                }
            }

            //var deger = db.TBLURUNLER.ToList();
            var deger = db.TBLURUNLER.ToList().ToPagedList(sayfa, 5);

            return View(deger);
        }

        [HttpGet]
        public ActionResult YeniUrün()
        {

            var UrunZatenVar = TempData["UrunZatenVar"];

            if(UrunZatenVar != null)
            {
                ViewBag.UrunZatenVar = 1;
            }

            List<SelectListItem> degerler = (from item in db.TBLKATEGORILER.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = item.KATEGORIAD,
                                                 Value = item.KATEGORIID.ToString(),
                                             }).ToList() ;
            ViewBag.dgr = degerler;
            return View();
        }
        [HttpPost]
        public ActionResult YeniUrün(TBLURUNLER P1)
        {
         
            var ktg = db.TBLKATEGORILER.Where(x => x.KATEGORIID == P1.TBLKATEGORILER.KATEGORIID).FirstOrDefault();

            P1.TBLKATEGORILER = ktg;
            P1.URUNAD = P1.URUNAD.ToUpper();
            P1.MARKA = P1.MARKA.ToUpper();

            var existingUrun = db.TBLURUNLER.FirstOrDefault(x => x.URUNAD == P1.URUNAD && x.MARKA == P1.MARKA && x.TBLKATEGORILER.KATEGORIID == P1.TBLKATEGORILER.KATEGORIID);

            if(existingUrun != null)
            {
                // Eğer Urun zaten varsa, consola mesaj yaz
                TempData["UrunZatenVar"] = 1;

                return RedirectToAction("YeniUrün");
            }
            else
            {

                db.TBLURUNLER.Add(P1);
                db.SaveChanges();
                TempData["UrunEklemeBaşarılı"] = 1;
                return RedirectToAction("Index");
            }

        }

        public ActionResult SIL(int id)
        {

            var urun = db.TBLURUNLER.Find(id);
            var digerTabloReferans = db.TBLSATISLAR.FirstOrDefault(x => x.URUN==id);
            if(digerTabloReferans != null)
            {
                TempData["MesajSilFail"] = 1;
                return RedirectToAction("Index");
            }
            else
            {
                db.TBLURUNLER.Remove(urun);
                db.SaveChanges();
                TempData["MesajSilSuccess"] = 1;

                return RedirectToAction("Index");
            }

          
        }
        public ActionResult UrunGetir(int id)
        {
            List<SelectListItem> degerler = (from item in db.TBLKATEGORILER.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = item.KATEGORIAD,
                                                 Value = item.KATEGORIID.ToString(),
                                             }).ToList();
            ViewBag.dgr = degerler;

            var urn = db.TBLURUNLER.Find(id);
            return View("UrunGetir", urn);
        }


        public ActionResult Guncelle(TBLURUNLER p1)
        {
            var urn = db.TBLURUNLER.Find(p1.URUNID);
            urn.URUNAD = p1.URUNAD;
            urn.MARKA = p1.MARKA;
            urn.FIYAT = p1.FIYAT;
            urn.STOK = p1.STOK;

            var ktg = db.TBLKATEGORILER.Where(m=>m.KATEGORIID==p1.TBLKATEGORILER.KATEGORIID).FirstOrDefault();
            urn.URUNKATEGORI = ktg.KATEGORIID;

            db.SaveChanges();
            TempData["UrunGuncellemeBaşarılı"] = 1;


            return RedirectToAction("Index");
        }
    }
}