using Domain.Interfaces;

namespace StarterApp.Domain.Model;

public class SampleEntity : ILongIdentifiableEntity
{
    public long Id { get; set; }

    public required string Name { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public bool Active { get; set; }
}
