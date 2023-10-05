using AutoMapper;
using MovieAppAPI.Dtos;
using MovieAppAPI.Models;
using Org.BouncyCastle.Asn1.X509.Qualified;

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
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();
            CreateMap<UserSignupDto, User>();
            CreateMap<ReviewDto, Review>();
            CreateMap<Review, ReviewDto>();
            CreateMap<VoteDto, Vote>();
            CreateMap<Vote, VoteDto>();
            CreateMap<WatchlistDto, Watchlist>();
            CreateMap<Watchlist, WatchlistDto>();
            CreateProjection<MovieActor, CastMemberDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(ma => ma.Actor.Name))
                .ForMember(dto => dto.Photo, conf => conf.MapFrom(ma => ma.Actor.Photo));
            CreateMap(typeof(PagedResult<>), typeof(PaginationDataDto));

        }
    }
}
