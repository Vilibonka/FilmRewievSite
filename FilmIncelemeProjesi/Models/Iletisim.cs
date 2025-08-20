using System.ComponentModel.DataAnnotations;

namespace FilmIncelemeProjesi.Models
{
    public class IletisimMesaji
    {
        public int Id { get; set; }

        [Required]
        public string? Isim { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Mesaj { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;
    }
}
