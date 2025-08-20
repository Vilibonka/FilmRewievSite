using Microsoft.AspNetCore.Mvc;
using FilmIncelemeProjesi.Models;

namespace FilmIncelemeProjesi.Controllers
{
    public class SayfaController : Controller
    {
        private readonly UygulamaDbContext _context;

        public SayfaController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Hakkimizda()
        {
            return View();
        }

        public IActionResult Iletisim()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IletisimGonder(string isim, string email, string mesaj)
        {
            var yeniMesaj = new IletisimMesaji
            {
                Isim = isim,
                Email = email,
                Mesaj = mesaj
            };

            _context.IletisimMesajlari.Add(yeniMesaj);
            _context.SaveChanges();

            ViewBag.Onay = true;
            return View("Iletisim");
        }
    }
}
