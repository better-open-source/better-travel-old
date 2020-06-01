namespace BetterTravel.DataAccess.Abstractions.Entities
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}