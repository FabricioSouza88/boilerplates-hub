using Application.Interfaces;
using CoreKit.DataFilter.Models;
using Domain.Interfaces;

namespace Application.Services.BaseService
{
    public interface ICrudService<TEntity, IdType> : IScopedService
        where TEntity : class, IIdentifiableEntity<IdType>
    {
        Task<TEntity?> GetAsync(IdType id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(FilterRequest filter, CancellationToken cancellationToken = default);
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<TEntity> PutAsync(IdType id, object partal, CancellationToken cancellationToken = default);
        Task<TEntity> DeleteAsync(IdType id, CancellationToken cancellationToken = default);
    }
}
