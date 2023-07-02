import { Component } from '@angular/core';
import { MovieService } from '../shared/services/movie.service';
import { ActorsService } from '../shared/services/actors.service';
import { DirectorService } from '../shared/services/director.service';
import { Movie } from '../shared/models/movie';
import { Actor } from '../shared/models/actor';
import { Director } from '../shared/models/director';
import { DefaultPhoto } from '../shared/functions/default-photos';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {

  movies? : Movie[];
  actors? : Actor[];
  directors? : Director[];
  dp = new DefaultPhoto();

  constructor(private movieService : MovieService, private actorService: ActorsService, private directorService : DirectorService){}
  Search (input : string)
  {
    this.movieService.searchMovies(input).subscribe(movies => this.movies = movies);
    this.actorService.searchActors(input).subscribe(actors => this.actors = actors);
    this.directorService.searchDirectors(input).subscribe(directors => this.directors = directors);
  
  }
  
}
