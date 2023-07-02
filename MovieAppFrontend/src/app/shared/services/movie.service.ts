import { Injectable } from '@angular/core';
import { Movie } from '../models/movie';
import { Genre } from '../models/genre';
import { Actor } from '../models/actor';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import { Role } from '../models/role';
import { Director } from '../models/director';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  moviesUrl : string = "https://localhost:7172/api/Movie";
  searchMoviesUrl : string = 'https://localhost:7172/api/Movie?input=';
  movieGenresUrl : string = "https://localhost:7172/api/Movie/genre/";
  movieActorsUrl : string = "https://localhost:7172/api/Movie/actor/";
  movieRoleUrl : string = 'https://localhost:7172/api/MovieActor/';
  movieDirectorUrl : string =  'https://localhost:7172/director/';
  postMovieUrl = 'https://localhost:7172/api/Movie?directorId=1&genreIds=2&genreIds=5&genreIds=19'; 
  
  allMovies : Movie[] = [];
  constructor(private client : HttpClient) { 
   
  }

  getMovies() : Observable<Movie[]> {
    return this.client.get<Movie[]>(this.moviesUrl);
  }
  searchMovies(input : string){

    let query = encodeURIComponent(input.trim())
    return this.client.get<Movie[]>(this.searchMoviesUrl + query);
  }
  getMovie(id : number) : Observable<Movie>{
    return this.client.get<Movie>(this.moviesUrl + '/' + id);
  }
  getMovieGenres(movieId : number) : Observable<Genre[]> {
    return this.client.get<Genre[]>(this.movieGenresUrl + movieId);
  }
  getMovieActors(movieId : number) : Observable<Actor[]> {
    return this.client.get<Actor[]>(this.movieActorsUrl + movieId);
  }
  getMovieRole(movieId : number, actorId : number) : Observable<Role>{
    return this.client.get<Role>(this.movieRoleUrl + movieId + '/' + actorId);
  }
  getMovieDirector(movieId :number) : Observable<Director>{
    return this.client.get<Director>(this.movieDirectorUrl + movieId);
  }
  addMovie(movie : object, genres :Genre[], director : Director) : Observable<any>{

    let genreParams : string = "";
    let queryParams = new HttpParams();
    queryParams = queryParams.append('directorId', director.directorId);
     genres.forEach(genre => {
      genreParams += `&genreIds=${genre.genreId}`;
      queryParams = queryParams.append('genreIds', genre.genreId);
    });
    
   
    return this.client.post<any>( /*'https://cors-anywhere.herokuapp.com/' +*/ this.moviesUrl, movie, {params : queryParams, observe: 'response'});
  }

  updateMovie(id : number, movie : object, genres : Genre[], director : Director) : Observable<any> {
    let genreParams : string = "";
    let queryParams = new HttpParams();
    queryParams = queryParams.append('directorId', director.directorId);
     genres.forEach(genre => {
      genreParams += `&genreIds=${genre.genreId}`;
      queryParams = queryParams.append('genreIds', genre.genreId);
    });
    
   
    return this.client.put<any>(this.moviesUrl + '/' + id, movie, {params : queryParams, observe: 'response'});
    
  }
  deleteMovie(movieId : number) : Observable<any>{
    return this.client.delete<Movie>(this.moviesUrl + '/' + movieId);
  }
  addMovieRole(role : Role) : Observable<any> {
    return this.client.post<Role>(this.movieRoleUrl, role);
  }
  updateMovieRole(role : Role) : Observable<any> {
    return this.client.put<Role>(this.movieRoleUrl + role.movieId + '/' + role.actorId, role);
  }
  deleteMovieRole(movieId : number, actorId : number) : Observable<any> {
    return this.client.delete<Movie>(this.movieRoleUrl + movieId + '/' + actorId);
  }
  
}
