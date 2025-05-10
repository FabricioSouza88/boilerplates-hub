using Application.Dto;
using AutoMapper;
using StarterApp.Domain.Model;

namespace Application.AutoMapper
{
    public class SampleProfile : Profile
    {
        public SampleProfile() 
        {
            CreateMap<SampleEntity, SampleDto>();
            CreateMap<SampleDto, SampleEntity>();
        }
    }
}
