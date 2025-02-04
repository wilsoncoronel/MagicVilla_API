using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>();//Se puede usar esta opcion 
            CreateMap<VillaDto, Villa>();//Son opciones individuales inversas
            CreateMap<Villa, VillaCreateDto>().ReverseMap();// O solo se puede usar una opcion que combina las dosopciones en una sola
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        }
    }
}
