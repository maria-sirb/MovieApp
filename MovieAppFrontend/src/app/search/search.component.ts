import { Component, OnInit } from '@angular/core';
import { MovieService } from '../shared/services/movie.service';
import { ActorsService } from '../shared/services/actors.service';
import { DirectorService } from '../shared/services/director.service';
import { Movie } from '../shared/models/movie';
import { Actor } from '../shared/models/actor';
import { Director } from '../shared/models/director';
import { DefaultPhoto } from '../shared/functions/default-photos';
import { UserStoreService } from '../shared/services/user-store.service';
import { AuthenticationService } from '../shared/services/authentication.service';
import { WatchlistService } from '../shared/services/watchlist.service';
import { BehaviorSubject, Subject, combineLatest, debounceTime, distinct, distinctUntilChanged, switchMap } from 'rxjs';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit{

  movies? : Movie[];
  actors? : Actor[];
  directors? : Director[];
  dp = new DefaultPhoto();
  inputSubject = new Subject<string>();
  noResults = false;

  constructor(private movieService : MovieService, private actorService: ActorsService, private directorService : DirectorService){}
  ngOnInit(): void {
     this.inputSubject.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      switchMap(input => {
        return combineLatest([
          this.movieService.searchMovies(input),
          this.actorService.searchActors(input),
          this.directorService.searchDirectors(input)
        ])
      })
     ).subscribe(([movies, actors, directors]) => {
      this.movies = movies;
      this.actors = actors;
      this.directors = directors;
      if(!movies.length && !actors.length && !directors.length)
        this.noResults = true;
    });
  }
  
  Search (input : string)
  {
    this.inputSubject.next(input);
    this.noResults = false;
  }
}
