namespace BetterTravel.DataAccess.Abstractions.Entities
{
    public class User : EntityBase<int>
    {
        public long ChatId { get; set; }
    }
}