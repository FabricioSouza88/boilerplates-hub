using Application.Services.BaseService;
using StarterApp.Domain.Model;

namespace Application.Interfaces
{
    public interface ISampleService : ICrudService<SampleEntity, long>
    {
    }
}
