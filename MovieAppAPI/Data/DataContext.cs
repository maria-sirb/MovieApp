using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Models;

namespace MovieAppAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 
        }  
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Genre> Genres { get; set; }    
        public DbSet<Director> Directors { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<MovieGenre> MovieGenres{ get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });
            modelBuilder.Entity<MovieGenre>()
                .HasOne(m => m.Movie)
                .WithMany(mg => mg.MovieGenres)
                .HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<MovieGenre>()
               .HasOne(m => m.Genre)
               .WithMany(mg => mg.MovieGenres)
               .HasForeignKey(g => g.GenreId);

            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });
            modelBuilder.Entity<MovieActor>()
                .HasOne(m => m.Movie)
                .WithMany(ma => ma.MovieActors)
                .HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<MovieActor>()
               .HasOne(m => m.Actor)
               .WithMany(ma => ma.MovieActors)
               .HasForeignKey(a => a.ActorId);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("user");
        }

    }
    
}
