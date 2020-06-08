using System.Collections.Generic;

namespace BetterTravel.Infrastructure.Domain
{
    public class Descritption
    {
        public Descritption(string date, string text, IEnumerable<string> hashTags)
        {
            Date = date;
            Text = text;
            HashTags = hashTags;
        }

        public string Date { get; }
        public string Text { get; }
        public IEnumerable<string> HashTags { get; }

        public override string ToString() => 
            $"Date: {Date} | {Text} | {string.Join(", ", HashTags)}";
    }
}