using AutoMapper;
using MovieAppAPI.Dtos;
using MovieAppAPI.Models;

namespace MovieAppAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Movie, MovieDto>();
            CreateMap<MovieDto, Movie>();
            CreateMap<Genre, GenreDto>();
            CreateMap<GenreDto, Genre>();
            CreateMap<Director, DirectorDto>();
            CreateMap<DirectorDto, Director>();
            CreateMap<Actor, ActorDto>();
            CreateMap<ActorDto, Actor>();
            CreateMap<MovieActor, MovieActorDto>();
            CreateMap<MovieActorDto, MovieActor>();
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();


        }
    }
}
