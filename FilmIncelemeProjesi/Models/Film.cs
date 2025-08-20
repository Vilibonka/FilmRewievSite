using System.ComponentModel.DataAnnotations;

namespace FilmIncelemeProjesi.Models
{
    public class Film
    {
        public int Id { get; set; }

        [Required]
        public string? Ad { get; set; }

        public string? Aciklama { get; set; }

        [Display(Name = "Yayın Tarihi")]
        public DateTime YayinTarihi { get; set; }

        public string? PosterUrl { get; set; }

        public string? Kategori { get; set; }
    }
}
