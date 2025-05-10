using Application.Interfaces;
using Core.Lib.EventHandler.Interfaces;
using Database;
using Microsoft.EntityFrameworkCore;
using StarterApp.Domain.Model;
using static Application.Constants.AppConstants;

namespace Application.Services;

public class SampleService : ISampleService, IScopedService
{
    private readonly AppDbContext _dbContext;
    private readonly IEventHandler _eventHandler;

    public SampleService(AppDbContext dbContext, IEventHandler eventHandler)
    {
        _dbContext = dbContext;
        _eventHandler = eventHandler;
    }

    public async Task<IEnumerable<SampleEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Samples.ToListAsync(cancellationToken);
    }

    public async Task<SampleEntity?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Samples.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<SampleEntity> CreateAsync(SampleEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Samples.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _eventHandler.PublishEvent(new { Entity = entity }, ProcessName.Sample, ActionName.Insert);

        return entity;
    }

    public async Task<SampleEntity> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Samples.FindAsync(id) ?? throw new KeyNotFoundException();
        _dbContext.Samples.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _eventHandler.PublishEvent(new { Entity = entity }, ProcessName.Sample, ActionName.Delete);

        return entity;
    }
}
