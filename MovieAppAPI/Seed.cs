using MovieAppAPI.Data;
using MovieAppAPI.Models;
using System.Collections.Generic;

namespace MovieAppAPI
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }   
        public void SeedDataContext()
        {
            
            if (!dataContext.MovieActors.Any())
            {
                var movieActors = new List<MovieActor>()
                {
                     new MovieActor()
                    {
                        Movie = new Movie()
                        {
                            Title = "Pulp Fiction",
                            ReleaseYear = 1994,
                            RunTime = 154,
                            ImdbRating = 8.8,
                            Summary = "The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption.",
                            StoryLine = "Jules Winnfield (Samuel L. Jackson) and Vincent Vega (John Travolta) are two hit men who are out to retrieve a suitcase stolen from their employer, mob boss Marsellus Wallace (Ving Rhames). Wallace has also asked Vincent to take his wife Mia (Uma Thurman) out a few days later when Wallace himself will be out of town. Butch Coolidge (Bruce Willis) is an aging boxer who is paid by Wallace to lose his fight. The lives of these seemingly unrelated people are woven together comprising of a series of funny, bizarre and uncalled-for incidents.",
                            Director = new Director()
                            {
                                Name = "Quentin Tarrantino"
                            },
                            MovieGenres = new List<MovieGenre>()
                            {
                                new MovieGenre { Genre = new Genre() {Name = "Crime" } },
                                new MovieGenre {Genre = new Genre() {Name = "Drama"}}
                            }

                        },
                        Actor = new Actor()
                        {
                            Name = "John Travolta"

                        },
                        Role = "Vincent Vega"

                    },
                    new MovieActor() 
                    {
                        Movie = new Movie()
                        {
                            Title = "Inception",
                            ReleaseYear = 2010,
                            RunTime = 148,
                            ImdbRating = 8.8,
                            Summary = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O., but his tragic past may doom the project and his team to disaster.",
                            StoryLine = "Dom Cobb is a skilled thief, the absolute best in the dangerous art of extraction, stealing valuable secrets from deep within the subconscious during the dream state, when the mind is at its most vulnerable. Cobb's rare ability has made him a coveted player in this treacherous new world of corporate espionage, but it has also made him an international fugitive and cost him everything he has ever loved. Now Cobb is being offered a chance at redemption. One last job could give him his life back but only if he can accomplish the impossible, inception. Instead of the perfect heist, Cobb and his team of specialists have to pull off the reverse: their task is not to steal an idea, but to plant one. If they succeed, it could be the perfect crime. But no amount of careful planning or expertise can prepare the team for the dangerous enemy that seems to predict their every move. An enemy that only Cobb could have seen coming.",
                            Director = new Director()
                            {
                                Name = "Christopher Nolan"
                            },
                            MovieGenres = new List<MovieGenre>()
                            {
                                new MovieGenre { Genre = new Genre() {Name = "Action" } },
                                new MovieGenre {Genre = new Genre() {Name = "Sci-Fi"}},
                                new MovieGenre { Genre = new Genre() {Name = "Adventure" } }
                            }

                        },
                        Actor = new Actor()
                        {
                            Name = "Leonardo DiCaprio"

                        },
                        Role = "Dom Cobb"
                    },
                    new MovieActor()
                    {
                       Movie = new Movie()
                       {
                           Title = "Modern Times",
                            ReleaseYear = 1936,
                            RunTime = 87,
                            ImdbRating = 8.5,
                            Summary = "The Tramp struggles to live in modern industrial society with the help of a young homeless woman.",
                           StoryLine = "Chaplin's last 'silent' film, filled with sound effects, was made when everyone else was making talkies. Charlie turns against modern society, the machine age, (The use of sound in films ?) and progress. Firstly we see him frantically trying to keep up with a production line, tightening bolts. He is selected for an experiment with an automatic feeding machine, but various mishaps leads his boss to believe he has gone mad, and Charlie is sent to a mental hospital - When he gets out, he is mistaken for a communist while waving a red flag, sent to jail, foils a jailbreak, and is let out again. We follow Charlie through many more escapades before the film is out.",
                           Director = new Director()
                            {
                                Name = "Charles Chaplin"
                            },
                            MovieGenres = new List<MovieGenre>()
                            {
                                new MovieGenre { Genre = new Genre() {Name = "Romance" } },
                                new MovieGenre {Genre = new Genre() {Name = "Comedy"}}
                            }
                       },
                       Actor = new Actor()
                       {
                           Name = "Charlie Chaplin"
                       },
                       Role = "A Factory Worker"
                    }
                };

                
                dataContext.MovieActors.AddRange(movieActors);
                dataContext.SaveChanges();

            }
            
        }
    }
}
