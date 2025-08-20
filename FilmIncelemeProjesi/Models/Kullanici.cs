using System.ComponentModel.DataAnnotations;

namespace FilmIncelemeProjesi.Models
{
    public class Kullanici
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
        public string? KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [DataType(DataType.Password)]
        public string? Sifre { get; set; }

        public bool AdminMi { get; set; } = false;
    }
}
