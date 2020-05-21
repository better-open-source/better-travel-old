namespace BetterTravel.Console.Domain
{
    public class PostInfo
    {
        public PostInfo(Descritption description, string imgUrl, string author)
        {
            ImgUrl = imgUrl;
            Description = description;
            Author = author;
        }
        
        public Descritption Description { get; }
        public string ImgUrl { get; }
        public string Author { get; }
    }
}