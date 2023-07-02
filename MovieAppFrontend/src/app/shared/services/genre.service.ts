import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';
import { Genre } from '../models/genre';
import { Movie } from '../models/movie';

@Injectable({
  providedIn: 'root'
})
export class GenreService {

  genreMoviesUrl = 'https://localhost:7172/api/Genre/movie/';
  genresUrl = 'https://localhost:7172/api/Genre';
  constructor(private client : HttpClient) { }
  getGenres() : Observable<Genre[]>{
    return this.client.get<Genre[]>(this.genresUrl);
  }
  getGenreMovies(genreId : number) : Observable<Movie[]> {
    return this.client.get<Movie[]>(this.genreMoviesUrl + genreId);
  }
  getGenre(genreId : number) : Observable<Genre> {
    return this.client.get<Genre>(this.genresUrl + '/' + genreId);
  }
}
