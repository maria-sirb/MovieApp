import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Director } from '../models/director';
import { Movie } from '../models/movie';

@Injectable({
  providedIn: 'root'
})
export class DirectorService {

  directorsUrl : string = 'https://localhost:7172/api/Director';
  searchDirectorsUrl : string = 'https://localhost:7172/api/Director?input=';
  directorMoviesUrl : string =  'https://localhost:7172/movie/';
  directorNameUrl : string = 'https://localhost:7172/api/Director/director-name/';
  constructor(private client : HttpClient) { }

  getDirectors() : Observable<Director[]> {
    return this.client.get<Director[]>(this.directorsUrl);
  }
  getDirectorByName(name : string) : Observable<Director> {
    return this.client.get<Director>(this.directorNameUrl + encodeURIComponent(name.trim()));
  }
  searchDirectors(input : string) : Observable<Director[]>{
    let query = encodeURIComponent(input.trim())
    return this.client.get<Director[]>(this.searchDirectorsUrl + query);
  }
  getDirector(directorId : number) : Observable<Director>{
    return this.client.get<Director>(this.directorsUrl + '/' + directorId);
  }
  getDirectorMovies(directorId : number) : Observable<Movie[]> {
    return this.client.get<Movie[]>(this.directorMoviesUrl + directorId);
  }
  addDirector(director : object) : Observable<any>{
    return this.client.post<any>(this.directorsUrl, director, {observe : 'response'});
  }
  updateDirector(directorId : number, director : object) : Observable<any>{
    return this.client.put<any>(this.directorsUrl + '/' + directorId, director, {observe : 'response'});
  }
  deleteDirector(actorId : number) : Observable<any>{
    return this.client.delete<Director>(this.directorsUrl + '/' + actorId);
  }
}
