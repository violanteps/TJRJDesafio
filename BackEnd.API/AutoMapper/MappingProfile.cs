using AutoMapper;
using Domain.Entity;

namespace BackEnd.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LivroModel, LivroEntity>();
            CreateMap<AutorModel, AutorEntity>();
            CreateMap<AssuntoModel, AssuntoEntity>();

        }

    }
}
