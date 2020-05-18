using System;

namespace better_travel.Console
{
    public class AccountInformation
    {
        public string InstagramUesrname => _instagramUesrname;

        public string InstagramPassword => _instagramPassword;

        public string BaseUrl => "https://www.instagram.com";

        public string UserNameSelector =>
            "#react-root > section > main > div > article > div > div:nth-child(1) > div > form > div:nth-child(2) > div > label > input";

        public string PasswordSelector =>
            "#react-root > section > main > div > article > div > div:nth-child(1) > div > form > div:nth-child(3) > div > label > input";

        public string SubmitBtnSelector =>
            "#react-root > section > main > div > article > div > div:nth-child(1) > div > form > div:nth-child(4) > button";

        public string PostСloseButton => "div.Igw0E.IwRSH.eGOV_._4EzTm.BI4qX.qJPeX.fm1AK.TxciK.yiMZG > button > svg";

        public string HashTagsBaseClass => ".EZdmt";

        public string[] Hashtag => new[] {"Geeks", "GFG", "travel", "dota", "apex"};

        private readonly string _instagramUesrname = "disayo8154";

        private readonly string _instagramPassword = "disayo8154@inbov03.com";
        /*      private readonly string _instagramUesrname = "yapaw85310";
        private readonly string _instagramPassword = "yapaw85310@box4mls.com";*/
    }
}