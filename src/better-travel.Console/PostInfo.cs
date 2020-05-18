namespace better_travel.Console
{
    public class PostInfo
    {
        public Descritption Description { get; set; }
        public string ImgUrl { get; set; }
        public string Author { get; set; }


        public PostInfo(Descritption description, string imgUrl, string author)
        {
            ImgUrl = imgUrl;
            Description = description;
            Author = author;
        }
    }
}