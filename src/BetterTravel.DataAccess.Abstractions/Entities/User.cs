using System;

namespace BetterTravel.DataAccess.Abstractions.Entities
{
    public class User : EntityBase<int>
    {
        public long ChatId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LanguageCode { get; set; }
        public bool IsBot { get; set; }
        
        public bool IsSubscribed { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}