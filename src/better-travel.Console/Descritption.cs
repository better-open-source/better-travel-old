using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace better_travel.Console
{
    public class Descritption
    {
        public string Date { get; set; }
        public string Text { get; set; }
        public IEnumerable<HtmlNode> Hashtag { get; set; }
    }
}