import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';
import { Genre } from '../models/genre';
import { Movie } from '../models/movie';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  
  genresUrl = environment.apiUrl + '/Genre';
  constructor(private client : HttpClient) { }
  getGenres() : Observable<Genre[]>{
    return this.client.get<Genre[]>(this.genresUrl);
  }
  getGenreMovies(genreId : number) : Observable<Movie[]> {
    return this.client.get<Movie[]>(this.genresUrl + '/movie/' + genreId);
  }
  getGenre(genreId : number) : Observable<Genre> {
    return this.client.get<Genre>(this.genresUrl + '/' + genreId);
  }
}
