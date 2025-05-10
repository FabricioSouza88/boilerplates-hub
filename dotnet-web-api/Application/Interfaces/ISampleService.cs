using StarterApp.Domain.Model;

namespace Application.Interfaces
{
    public interface ISampleService
    {
        Task<SampleEntity?> GetAsync(long id, CancellationToken cancellationToken = default);
        Task<IEnumerable<SampleEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<SampleEntity> CreateAsync(SampleEntity entity, CancellationToken cancellationToken = default);
        Task<SampleEntity> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
