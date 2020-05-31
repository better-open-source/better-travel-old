using BetterTravel.Domain;

namespace BetterTravel.DataAccess
{
    public class TourInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PostUrl { get; set; }
        public string ImgUrl { get; set; }
        public string Author { get; set; }
        public Descritption Description { get; set; }
    }
}