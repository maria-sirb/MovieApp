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
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });
            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId);
            modelBuilder.Entity<MovieGenre>()
               .HasOne(mg => mg.Genre)
               .WithMany(g => g.MovieGenres)
               .HasForeignKey(mg => mg.GenreId);

            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });
            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId);
            modelBuilder.Entity<MovieActor>()
               .HasOne(ma => ma.Actor)
               .WithMany(a => a.MovieActors)
               .HasForeignKey(ma => ma.ActorId);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("user");

            modelBuilder.Entity<Review>()
                .ToTable(r => r.HasCheckConstraint("CK_Review_RatingMax", "Rating < 11"));
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Watchlist>()
                .HasKey(w => new{w.UserId, w.MovieId});
            modelBuilder.Entity<Watchlist>()
                .HasOne(w => w.User)
                .WithMany(u => u.Watchlist)
                .HasForeignKey(w => w.UserId);
            modelBuilder.Entity<Watchlist>()
                .HasOne(w => w.Movie)
                .WithMany(m => m.Watchlist)
                .HasForeignKey(w => w.MovieId);

        }

    }
    
}
