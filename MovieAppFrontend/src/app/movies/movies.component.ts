import { AfterContentInit, AfterViewInit, Component, OnInit } from '@angular/core';
import { Movie } from '../shared/models/movie';
import { MovieService } from '../shared/services/movie.service';
import { Genre } from '../shared/models/genre';
import { GenreService } from '../shared/services/genre.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { DefaultPhoto } from '../shared/functions/default-photos';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent implements OnInit{

  movies : Movie[] = [];
  selectedMovie? : Movie;
  sortMode = this.route.snapshot.paramMap.get('sortMode');
  dp = new DefaultPhoto();
  constructor(private movieService : MovieService, private genreService : GenreService, private route : ActivatedRoute, private router : Router){
     
  }

  ngOnInit() : void {
   
   this.getAllMovies();
    
  }
  getAllMovies() {
    this.movieService.getMovies().subscribe(res => {
      this.movies = res; 
      this.movies.forEach(movie => {
        this.movieService.getMovieGenres(movie.movieId).subscribe(res => movie.movieGenres = res);
        this.movieService.getMovieActors(movie.movieId).subscribe(res => movie.movieActors = res);
      });

      this.sortMovies();
    });
  }
  displayGenreMovies(genre : Genre) {
    
    if(genre == undefined || genre == null)
       this.getAllMovies();
    else
    {
      this.genreService.getGenreMovies(genre.genreId).subscribe(movies => {this.movies = movies; this.sortMovies()});
    }
  }
  onSelect(movie : Movie)
  {
    this.selectedMovie = movie;
  }
  updateSort(sortOption : any)
  {
    this.sortMode = sortOption;
    this.router.navigate([`../${this.sortMode}`], { relativeTo: this.route });
    this.sortMovies();
  }
  sortMovies()
  {
    if(this.sortMode == 'none')
    {
      this.movies.sort((movie1, movie2) => movie1.movieId < movie2.movieId? -1 : 1);
    }
    if(this.sortMode == 'releaseYearAsc')  
    {
      this.movies.sort((movie1, movie2) => movie1.releaseYear < movie2.releaseYear? -1 : 1);
    }
    if(this.sortMode == 'releaseYearDesc')  
    {
      this.movies.sort((movie1, movie2) => movie1.releaseYear < movie2.releaseYear? 1 : -1);
    }
    if(this.sortMode == 'imdbRating')  
    {
      this.movies.sort((movie1, movie2) => movie1.imdbRating < movie2.imdbRating? 1 : -1);
    }
    if(this.sortMode == 'oscarWins')  
    {
      this.movies.sort((movie1, movie2) => {
        if (movie1.oscarWins < movie2.oscarWins) return 1;
        if (movie1.oscarWins > movie2.oscarWins) return -1;
        if (movie1.oscarNominations < movie2.oscarNominations) return 1;
        if (movie1.oscarNominations > movie2.oscarNominations) return -1;
        return 0;
      });
    }
  }
  
 
  
}
