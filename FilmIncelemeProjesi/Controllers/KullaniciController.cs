using Microsoft.AspNetCore.Mvc;
using FilmIncelemeProjesi.Models;

namespace FilmIncelemeProjesi.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly UygulamaDbContext _context;

        public KullaniciController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Kayit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Kayit(Kullanici kullanici)
        {
            if (!ModelState.IsValid)
                return View(kullanici);

            var mevcut = _context.Kullanicilar
                .FirstOrDefault(k => k.KullaniciAdi == kullanici.KullaniciAdi);

            if (mevcut != null)
            {
                ModelState.AddModelError("KullaniciAdi", "Bu kullanıcı adı zaten kullanılıyor.");
                return View(kullanici);
            }

            _context.Kullanicilar.Add(kullanici);
            _context.SaveChanges();
            return RedirectToAction("Giris");
        }

        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Giris(string kullaniciAdi, string sifre)
        {
            var kullanici = _context.Kullanicilar
                .FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi);

            if (kullanici != null && kullanici.Sifre == sifre && !string.IsNullOrEmpty(kullanici.KullaniciAdi))
            {
                HttpContext.Session.SetString("KullaniciAdi", kullanici.KullaniciAdi);
                HttpContext.Session.SetInt32("KullaniciId", kullanici.Id);
                return RedirectToAction("Index", "Film");
            }


            ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Film");
        }
        
        public IActionResult Profil()
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

            if (string.IsNullOrEmpty(kullaniciAdi))
                return RedirectToAction("Giris");

            var yorumlar = _context.Yorumlar
                .Where(y => y.KullaniciAdi == kullaniciAdi)
                .OrderByDescending(y => y.Tarih)
                .ToList();

            return View(yorumlar);
        }


    }
    
}
