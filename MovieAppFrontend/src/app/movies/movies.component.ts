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
import { PaginationData } from '../shared/models/paginationData';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent implements OnInit{

  movies : Movie[] = [];
  sortMode = "";
  pickedGenreId = undefined;
  dp = new DefaultPhoto();
  currentUserId = 0;
  currentUserWatchlist : Movie[] | undefined = undefined;
  paginationData : PaginationData | undefined = undefined;

  constructor(private movieService : MovieService, private genreService : GenreService, private userStoreService : UserStoreService, private authService : AuthenticationService, private watchlistService : WatchlistService, private route : ActivatedRoute, private router : Router){}

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

    this.route.queryParams.subscribe(params => {
      this.sortMode = params['sort'] || "";
      this.pickedGenreId = params['genreId'];
      this.getMovies(params['page'] || 1, params['sort'], params['genreId']);
    })
  }
  getMovies(pageNumber : number, sortMode : string | null, genreId : number | null)
  {
    this.movieService.getMoviesPaged(pageNumber, sortMode, genreId).subscribe(response => {
      if(response.body)
        this.movies = response.body;
      this.paginationData = JSON.parse(response.headers.get('X-Pagination')||"");
    });
  }
  changePage(pageNumber : number)
  {
    this.router.navigate(['/movies'], {queryParams : {page : pageNumber, sort : this.sortMode, genreId : this.pickedGenreId}});
  }
  pickGenre(genreId : number)
  {
    this.router.navigate(['/movies'], {queryParams : {page : 1, sort : this.sortMode, genreId : genreId}});
  }
  updateSort(sortOption : any)
  {
    this.sortMode = sortOption;
    this.router.navigate(['/movies'], {queryParams : {page : 1, sort : this.sortMode, genreId : this.pickedGenreId}});
  }
  
}
