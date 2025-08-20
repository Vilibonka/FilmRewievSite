using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmIncelemeProjesi.Models
{
    public class Yorum
    {
        public int Id { get; set; }

        [Required]
        public string? YorumMetni { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;

        public int FilmId { get; set; }

        [ForeignKey("FilmId")]
        public Film? Film { get; set; }

        public string? KullaniciAdi { get; set; }
    }
}
