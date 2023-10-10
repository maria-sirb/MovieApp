import { Injectable } from '@angular/core';
import { Movie } from '../models/movie';
import { Genre } from '../models/genre';
import { Actor } from '../models/actor';
import {HttpClient, HttpParams, HttpResponse} from '@angular/common/http';
import {Observable} from 'rxjs';
import { Role } from '../models/role';
import { Director } from '../models/director';
import { environment } from 'src/environments/environment';
import { CastMember } from '../models/castMember';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  moviesUrl : string = environment.apiUrl + "/Movie";
  movieRoleUrl : string = environment.apiUrl + '/MovieActor/';
 // movieDirectorUrl : string =  environment.apiUrl + '/Director/';
  
  allMovies : Movie[] = [];
  constructor(private client : HttpClient) { 
   
  }

  getMovies() : Observable<Movie[]> {
    return this.client.get<Movie[]>(this.moviesUrl);
  }
  searchMovies(input : string){

    let query = encodeURIComponent(input.trim())
    return this.client.get<Movie[]>(this.moviesUrl + '?input=' + query);
  }
  getMovie(id : number) : Observable<Movie>{
    return this.client.get<Movie>(this.moviesUrl + '/' + id);
  }
  getMoviesPaged(pageNumber : number, orderBy : string | null, genreId : number | null) : Observable<HttpResponse<Movie[]>>{
    let queryParams = new HttpParams();
    queryParams = queryParams.append("PageNumber", pageNumber);
    queryParams = queryParams.append("PageSize", 25);
    if(orderBy)
      queryParams = queryParams.append("OrderBy", orderBy);
    if(genreId)
      queryParams = queryParams.append("GenreId", genreId);
    return this.client.get<Movie[]>(this.moviesUrl + '/paged', {params : queryParams, observe : 'response'});
  }
  getMovieGenres(movieId : number) : Observable<Genre[]> {
    return this.client.get<Genre[]>(this.moviesUrl + '/genre/' + movieId);
  }
  getMovieActors(movieId : number) : Observable<Actor[]> {
    return this.client.get<Actor[]>(this.moviesUrl + '/actor/' + movieId);
  }
  getMovieRole(movieId : number, actorId : number) : Observable<Role>{
    return this.client.get<Role>(this.movieRoleUrl + movieId + '/' + actorId);
  }
  getMovieCast(movieId : number) : Observable<CastMember[]>{
    return this.client.get<CastMember[]>(this.moviesUrl + '/cast/' + movieId);
  }
  getMovieDirector(movieId :number) : Observable<Director>{
    return this.client.get<Director>(this.moviesUrl + "/director/" + movieId);
  }
  addMovie(movie : Movie, genres :Genre[], director : Director) : Observable<number>{

    let genreParams : string = "";
    let queryParams = new HttpParams();
    queryParams = queryParams.append('directorId', director.directorId);
     genres.forEach(genre => {
      genreParams += `&genreIds=${genre.genreId}`;
      queryParams = queryParams.append('genreIds', genre.genreId);
    });
    
   
    return this.client.post<any>(this.moviesUrl, movie, {params : queryParams});
  }

  updateMovie(id : number, movie : Movie, genres : Genre[], director : Director) : Observable<number> {
    let genreParams : string = "";
    let queryParams = new HttpParams();
    queryParams = queryParams.append('directorId', director.directorId);
     genres.forEach(genre => {
      genreParams += `&genreIds=${genre.genreId}`;
      queryParams = queryParams.append('genreIds', genre.genreId);
    });
    
   
    return this.client.put<number>(this.moviesUrl + '/' + id, movie, {params : queryParams});
    
  }
  deleteMovie(movieId : number) : Observable<any>{
    return this.client.delete<any>(this.moviesUrl + '/' + movieId);
  }
  addMovieRole(role : Role) : Observable<any> {
    return this.client.post<any>(this.movieRoleUrl, role);
  }
  updateMovieRole(role : Role) : Observable<any> {
    return this.client.put<any>(this.movieRoleUrl + role.movieId + '/' + role.actorId, role);
  }
  deleteMovieRole(movieId : number, actorId : number) : Observable<any> {
    return this.client.delete<any>(this.movieRoleUrl + movieId + '/' + actorId);
  }
  
}
