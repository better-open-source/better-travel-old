namespace BetterTravel.DataAccess.Models
{
    public class TourInfo : BaseEntity
    {
        public string PostUrl { get; set; }
        public string ImgUrl { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Text { get; set; }
        public string HashTags { get; set; }
    }
}