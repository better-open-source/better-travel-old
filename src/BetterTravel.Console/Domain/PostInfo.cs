using System;

namespace BetterTravel.Console.Domain
{
    public class PostInfo
    {
        public PostInfo(Descritption description, string imgUrl, string author)
        {
            ImgUrl = imgUrl;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Author = author;
        }
        
        public Descritption Description { get; }
        public string ImgUrl { get; }
        public string Author { get; }

        public override string ToString() =>
            $"Author = {Author} | {Description}";
    }
}