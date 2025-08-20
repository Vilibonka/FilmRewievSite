using Microsoft.AspNetCore.Mvc;
using FilmIncelemeProjesi.Models;
using Microsoft.EntityFrameworkCore;
using FilmIncelemeProjesi.Services;


namespace FilmIncelemeProjesi.Controllers
{
    public class FilmController : Controller
    {
        private readonly UygulamaDbContext _context;
        private readonly TmdbService _tmdbService;

        public FilmController(UygulamaDbContext context, TmdbService tmdbService)
        {
            _context = context;
            _tmdbService = tmdbService;
        }

        public async Task<IActionResult> Index()
        {
            var filmler = await _context.Filmler.ToListAsync();
            return View(filmler);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi);

            if (kullanici == null || !kullanici.AdminMi)
                return Unauthorized();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Film film)
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi);

            if (kullanici == null || !kullanici.AdminMi)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var film = await _context.Filmler.FirstOrDefaultAsync(f => f.Id == id);
            if (film == null)
                return NotFound();

            var ortalamaPuan = _context.FilmPuanlari
                .Where(p => p.FilmId == film.Id)
                .Select(p => p.Puan)
                .DefaultIfEmpty()
                .Average();

            var yorumlar = await _context.Yorumlar
                .Where(y => y.FilmId == id)
                .OrderByDescending(y => y.Tarih)
                .ToListAsync();

            ViewBag.Film = film;
            ViewBag.Yorumlar = yorumlar;
            ViewBag.OrtalamaPuan = ortalamaPuan;

            return View(new Yorum { FilmId = film.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YorumEkle(int FilmId, string YorumMetni)
        {
            if (!string.IsNullOrWhiteSpace(YorumMetni))
            {
                var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

                if (string.IsNullOrEmpty(kullaniciAdi))
                    return RedirectToAction("Giris", "Kullanici");

                var yorum = new Yorum
                {
                    FilmId = FilmId,
                    YorumMetni = YorumMetni,
                    Tarih = DateTime.Now,
                    KullaniciAdi = kullaniciAdi
                };

                _context.Yorumlar.Add(yorum);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = FilmId });
        }


        public async Task<IActionResult> Ara(string q)
        {
            var sonuc = string.IsNullOrWhiteSpace(q)
                ? new List<Film>()
                : await _context.Filmler
                .Where(f => f.Ad != null && f.Ad.Contains(q))

                    .ToListAsync();

            ViewBag.Arama = q;
            return View("AramaSonucu", sonuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PuanVer(int filmId, int puan)
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            if (string.IsNullOrEmpty(kullaniciAdi)) return RedirectToAction("Giris", "Kullanici");

            var onceki = _context.FilmPuanlari.FirstOrDefault(p => p.FilmId == filmId && p.KullaniciAdi == kullaniciAdi);
            if (onceki == null)
            {
                var yeni = new FilmPuani
                {
                    FilmId = filmId,
                    Puan = puan,
                    KullaniciAdi = kullaniciAdi
                };
                _context.FilmPuanlari.Add(yeni);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = filmId });
        }

        [HttpGet]
        public IActionResult ApiEkle()
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi);

            if (kullanici == null || !kullanici.AdminMi)
                return Unauthorized();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiEkle(string filmAdi)
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi);

            if (kullanici == null || !kullanici.AdminMi)
                return Unauthorized();

            var film = await _tmdbService.AraVeGetirAsync(filmAdi);

            if (film == null)
            {
                ViewBag.Hata = "Film bulunamadı veya TMDb'den alınamadı.";
                return View();
            }

            var zatenVar = _context.Filmler.Any(f => f.Ad == film.Ad);
            if (zatenVar)
            {
                ViewBag.Hata = "Bu film zaten eklenmiş.";
                return View();
            }

            _context.Filmler.Add(film);
            await _context.SaveChangesAsync();

            ViewBag.Basarili = "Film başarıyla eklendi.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PopulerFilmler()
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi);

            if (kullanici == null || !kullanici.AdminMi)
                return Unauthorized();

            var apiFilmler = await _tmdbService.PopulerFilmleriGetirAsync();
            var mevcutFilmler = _context.Filmler.Select(f => f.Ad).ToHashSet();

            var yeniFilmler = apiFilmler
                .Where(f => !mevcutFilmler.Contains(f.Ad)) // zaten varsa listeleme
                .ToList();

            return View(yeniFilmler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PopulerFilmleriKaydet(List<string> filmAdlari)
        {
            var apiFilmler = await _tmdbService.PopulerFilmleriGetirAsync();
            var eklenecek = apiFilmler
                .Where(f => !string.IsNullOrEmpty(f.Ad) && filmAdlari.Contains(f.Ad!))
                .ToList();

            _context.Filmler.AddRange(eklenecek);
            await _context.SaveChangesAsync();

            TempData["Basarili"] = $"{eklenecek.Count} film başarıyla eklendi.";
            return RedirectToAction("Index");
        }

    }
    
}
