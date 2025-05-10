namespace Domain.Interfaces;

public interface IIdentifiableEntity<IdType>
{
    IdType Id { get; set; }
}
