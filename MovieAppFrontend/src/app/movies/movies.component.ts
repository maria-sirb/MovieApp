import { AfterContentInit, AfterViewInit, Component, OnInit } from '@angular/core';
import { Movie } from '../shared/models/movie';
import { MovieService } from '../shared/services/movie.service';
import { Genre } from '../shared/models/genre';
import { GenreService } from '../shared/services/genre.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { DefaultPhoto } from '../shared/functions/default-photos';
import { UserStoreService } from '../shared/services/user-store.service';
import { AuthenticationService } from '../shared/services/authentication.service';
import { WatchlistService } from '../shared/services/watchlist.service';
import { of, switchMap } from 'rxjs';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent implements OnInit{

  movies : Movie[] = [];
  selectedMovie? : Movie;
  sortMode = this.route.snapshot.paramMap.get('sortMode');
  pickedGenreId = Number(this.route.snapshot.paramMap.get('genreId'));
  dp = new DefaultPhoto();
  currentUserId = 0;
  currentUserWatchlist : Movie[] | undefined = undefined;

  constructor(private movieService : MovieService, private genreService : GenreService, private userStoreService : UserStoreService, private authService : AuthenticationService, private watchlistService : WatchlistService, private route : ActivatedRoute, private router : Router){
     
  }

  ngOnInit() : void {   

    this.userStoreService.getIdFromStore().pipe(
      switchMap((id) => {
        this.currentUserId = Number(id) || Number(this.authService.getIdFromToken());  
        if(this.currentUserId)
          return this.watchlistService.getUserWatchlist(this.currentUserId);
        this.currentUserWatchlist = [];
        return of(null);
      })
    ).subscribe(watchlist => this.currentUserWatchlist = watchlist || undefined);
 
    if(this.pickedGenreId)
      this.genreService.getGenre(this.pickedGenreId).subscribe(genre => this.displayGenreMovies(genre));
    else
      this.getAllMovies();
    
  }
  getAllMovies() {
    this.movieService.getMovies().subscribe(res => {
      this.movies = res; 
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
      this.movies.sort((movie1, movie2) => ((movie1.releaseYear || 0) < (movie2.releaseYear || 0))? -1 : 1);
    }
    if(this.sortMode == 'releaseYearDesc')  
    {
      this.movies.sort((movie1, movie2) => ((movie1.releaseYear || 0) < ( movie2.releaseYear || 0))? 1 : -1);
    }
    if(this.sortMode == 'imdbRating')  
    {
      this.movies.sort((movie1, movie2) => ((movie1.imdbRating || 0) < (movie2.imdbRating || 0)) ? 1 : -1);
    }
    if(this.sortMode == 'oscarWins')  
    {
      this.movies.sort((movie1, movie2) => {
        if (movie1.oscarWins || 0 < (movie2.oscarWins || 0)) return 1;
        if (movie1.oscarWins || 0 > (movie2.oscarWins || 0)) return -1;
        if (movie1.oscarNominations || 0 < (movie2.oscarNominations || 0)) return 1;
        if (movie1.oscarNominations || 0 > (movie2.oscarNominations || 0)) return -1;
        return 0;
      });
    }
  }
  
 
  
}
