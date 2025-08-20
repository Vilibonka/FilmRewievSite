namespace FilmIncelemeProjesi.Models
{
    public class FilmDetayViewModel
    {
        public Film? Film { get; set; }
        public List<Yorum>? Yorumlar { get; set; }
        public Yorum? YeniYorum { get; set; }
    }
}
