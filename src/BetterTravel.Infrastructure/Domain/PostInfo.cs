using System;

namespace BetterTravel.Infrastructure.Domain
{
    public class PostInfo
    {
        public PostInfo(Descritption description, string imgUrl, string author, string postUrl)
        {
            ImgUrl = imgUrl;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Author = author;
            PostUrl = postUrl;
        }

        public Descritption Description { get; }
        public string ImgUrl { get; }

        public string PostUrl { get; }
        public string Author { get; }

        public override string ToString() =>
            $"Author = {Author} | {Description} | Photo link = {ImgUrl} | Post link = {PostUrl}";
    }
}