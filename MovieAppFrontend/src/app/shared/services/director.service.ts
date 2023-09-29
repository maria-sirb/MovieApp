import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Director } from '../models/director';
import { Movie } from '../models/movie';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DirectorService {

  directorsUrl : string = environment.apiUrl + '/Director';
  constructor(private client : HttpClient) { }

  getDirectors() : Observable<Director[]> {
    return this.client.get<Director[]>(this.directorsUrl);
  }
  getDirectorByName(name : string) : Observable<Director> {
    return this.client.get<Director>(this.directorsUrl + '/director-name/' + encodeURIComponent(name.trim()));
  }
  searchDirectors(input : string) : Observable<Director[]>{
    let query = encodeURIComponent(input.trim())
    return this.client.get<Director[]>(this.directorsUrl + '?input=' + query);
  }
  getDirector(directorId : number) : Observable<Director>{
    return this.client.get<Director>(this.directorsUrl + '/' + directorId);
  }
  getDirectorMovies(directorId : number) : Observable<Movie[]> {
    return this.client.get<Movie[]>(this.directorsUrl + '/movie/' + directorId);
  }
  addDirector(director : object) : Observable<number>{
    return this.client.post<number>(this.directorsUrl, director);
  }
  updateDirector(directorId : number, director : object) : Observable<any>{
    return this.client.put<any>(this.directorsUrl + '/' + directorId, director, {observe : 'response'});
  }
  deleteDirector(actorId : number) : Observable<any>{
    return this.client.delete<any>(this.directorsUrl + '/' + actorId);
  }
}
