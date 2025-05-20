using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using CoreKit.DataFilter.Models;
using Microsoft.AspNetCore.Mvc;
using StarterApp.Domain.Model;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CrudSampleController : ControllerBase
{
    private readonly ILogger<CrudSampleController> _logger;
    private readonly ISampleService _sampleService;
    private readonly IMapper _mapper;

    public CrudSampleController(ILogger<CrudSampleController> logger, IMapper mapper, ISampleService sampleService)
    {
        _logger = logger;
        _mapper = mapper;
        _sampleService = sampleService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SampleEntity>>> GetAllAsync([FromQuery] SimpleQueryRequest query, CancellationToken cancellationToken = default)
    {
        return Ok(await _sampleService.GetAllAsync(query.ToFilterRequest(), cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SampleEntity>> GetAsync([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var entity = await _sampleService.GetAsync(id, cancellationToken);
        
        if (entity == null)
        {
            _logger.LogError($"Entity {id} not found!");
        }
        
        return Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<SampleEntity>> CreateAsync([FromBody] SampleDto sampleDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SampleEntity>(sampleDto);

        await _sampleService.CreateAsync(entity, cancellationToken);

        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SampleEntity>> UpdatePutAsync([FromRoute] long id, [FromBody] SampleDto sampleDto, CancellationToken cancellationToken = default)
    {
        var entity = await _sampleService.PutAsync(id, sampleDto, cancellationToken);

        return Ok(entity);
    }

    [HttpDelete]
    public async Task<ActionResult<SampleEntity>> DeleteAsync([FromQuery] long id, CancellationToken cancellationToken = default)
    {
        return Ok(await _sampleService.DeleteAsync(id, cancellationToken));
    }
}
