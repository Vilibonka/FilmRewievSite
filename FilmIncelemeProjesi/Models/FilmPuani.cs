using System.ComponentModel.DataAnnotations;

namespace FilmIncelemeProjesi.Models
{
    public class FilmPuani
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Puan { get; set; }

        public int FilmId { get; set; }
        public Film? Film { get; set; }

        public string? KullaniciAdi { get; set; }
    }
}
