using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;

namespace FilmesApi.Profiles
{
    public class FilmeProfile : Profile
    {
        public FilmeProfile()
        {
            CreateMap<CreateFilmeDto, FilmeModel>();
            CreateMap<UpdateFilmeDto, FilmeModel>();

            CreateMap<FilmeModel, UpdateFilmeDto>();
            CreateMap<FilmeModel, ReadFilmeDto>();
        }
    }
}
