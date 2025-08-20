using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FilmIncelemeProjesi.Models;

namespace FilmIncelemeProjesi.Services
{
    public class TmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = ""; // <== Buraya TMDb'den alınan kişisel API KEY gelecek. Bir süre sonra güvenlik için keyi yeniden oluşturucam.

        public TmdbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Film>> PopulerFilmleriGetirAsync()
        {
            var url = $"https://api.themoviedb.org/3/movie/popular?api_key={_apiKey}&language=tr-TR&page=1";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<Film>();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var results = doc.RootElement.GetProperty("results");

            var filmler = new List<Film>();

            foreach (var item in results.EnumerateArray())
            {
                var kategori = "Genel";
                if (item.TryGetProperty("genre_ids", out var genres) && genres.GetArrayLength() > 0)
                {
                    var genreId = genres[0].GetInt32();
                    kategori = GenreIdToName(genreId);
                }

                var film = new Film
                {
                    Ad = item.GetProperty("title").GetString(),
                    Aciklama = item.GetProperty("overview").GetString(),
                    PosterUrl = "https://image.tmdb.org/t/p/w500" + item.GetProperty("poster_path").GetString(),
                    YayinTarihi = DateTime.TryParse(item.GetProperty("release_date").GetString(), out var date) ? date : DateTime.Today,
                    Kategori = kategori
                };

                filmler.Add(film);
            }

            return filmler;
        }

        public async Task<Film?> AraVeGetirAsync(string filmAdi)
        {
            var url = $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&query={filmAdi}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var ilkSonuc = root.GetProperty("results").EnumerateArray().FirstOrDefault();
            if (ilkSonuc.ValueKind == JsonValueKind.Undefined)
                return null;

            var kategori = "Genel";
            if (ilkSonuc.TryGetProperty("genre_ids", out var genreArray) && genreArray.GetArrayLength() > 0)
            {
                var genreId = genreArray[0].GetInt32();
                kategori = GenreIdToName(genreId);
            }

            return new Film
            {
                Ad = ilkSonuc.GetProperty("title").GetString(),
                Aciklama = ilkSonuc.GetProperty("overview").GetString(),
                PosterUrl = "https://image.tmdb.org/t/p/w500" + ilkSonuc.GetProperty("poster_path").GetString(),
                YayinTarihi = DateTime.TryParse(ilkSonuc.GetProperty("release_date").GetString(), out var date) ? date : DateTime.Today,
                Kategori = kategori
            };

        }

        private string GenreIdToName(int id)
        {
            return id switch
            {
                28 => "Aksiyon",
                12 => "Macera",
                878 => "Bilim Kurgu",
                18 => "Dram",
                35 => "Komedi",
                80 => "Suç",
                27 => "Korku",
                _ => "Genel"
            };
        }



    }
}
