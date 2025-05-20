using Application.Interfaces;
using Core.Lib.DataFilter.Linq.Extensions;
using Core.Lib.DataFilter.Models;
using Core.Lib.EventHandler.Interfaces;
using Database;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Application.Constants.AppConstants;

namespace Application.Services.BaseService;

public abstract class BaseCrudService<TEntity, IdType> : ICrudService<TEntity, IdType>, IScopedService
    where TEntity : class, IIdentifiableEntity<IdType>
{
    private readonly AppDbContext _dbContext;
    private readonly IEventHandler _eventHandler;
    private readonly string _entityName;

    public BaseCrudService(AppDbContext dbContext, IEventHandler eventHandler)
    {
        _dbContext = dbContext;
        _eventHandler = eventHandler;
        _entityName = typeof(TEntity).Name;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbContext.Set<TEntity>()
            .AddAsync(entity, cancellationToken);
        var createResult = await _dbContext.SaveChangesAsync(cancellationToken);

        _eventHandler.PublishEvent(new { Entity = entry.Entity, CreateResult = createResult }, _entityName, ActionName.Insert);

        return entry.Entity;
    }

    public virtual async Task<TEntity> DeleteAsync(IdType id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Set<TEntity>()
            .FindAsync([id], cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();

        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _eventHandler.PublishEvent(new { Entity = entity }, _entityName, ActionName.Delete);

        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(FilterRequest filter, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>()
            .AsQueryable()
            .ApplyFilterRequest(filter)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(IdType id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken) ?? throw new KeyNotFoundException();
    }

    public virtual async Task<TEntity> PutAsync(IdType id, object objectData, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(objectData);

        // Verifica se a entidade existe no banco
        var exists = await _dbContext.Set<TEntity>().AnyAsync(e => EF.Property<IdType>(e, "Id").Equals(id), cancellationToken);

        if (!exists)
            throw new KeyNotFoundException($"Entity of type {typeof(TEntity).Name} with Id={id} not found.");

        var entity = Activator.CreateInstance<TEntity>() ?? throw new InvalidOperationException($"Cannot instantiate {typeof(TEntity).Name}");

        // Define o valor do Id manualmente
        var idProperty = typeof(TEntity).GetProperty("Id") ?? throw new InvalidOperationException("Entity must have a property named 'Id'");
        idProperty.SetValue(entity, id);

        // Copia os valores do objeto parcial para a entidade
        var entry = _dbContext.Attach(entity);
        foreach (var prop in objectData.GetType().GetProperties())
        {
            var targetProp = typeof(TEntity).GetProperty(prop.Name);
            if (targetProp != null && targetProp.CanWrite)
            {
                var value = prop.GetValue(objectData);
                targetProp.SetValue(entity, value);
                entry.Property(prop.Name).IsModified = true;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        _eventHandler.PublishEvent(new { Entity = entity }, _entityName, ActionName.Update);

        return entity;
    }
}
