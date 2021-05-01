namespace SocialNetwork.Common.ArcLayer.Domain.Entity
{
    public interface IEntityBase<TId>
    {
        TId Id { get; }
    }
}