import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';
import { Actor } from '../models/actor';
import { Movie } from '../models/movie';

@Injectable({
  providedIn: 'root'
})
export class ActorsService {

  actorsUrl : string =  'https://localhost:7172/api/Actor';
  searchActorsUrl : string =  'https://localhost:7172/api/Actor?input=';
  actorMoviesUrl : string = 'https://localhost:7172/api/Actor/movie/';
  actorNameUrl = 'https://localhost:7172/api/Actor/actor-name/';
  constructor(private client : HttpClient) { }
  getActors() : Observable<Actor[]>{
      return this.client.get<Actor[]>(this.actorsUrl);
  }
  getActorByName(name : string) : Observable<Actor> {
    return this.client.get<Actor>(this.actorNameUrl + encodeURIComponent(name.trim()));
  }
  searchActors(input : string) : Observable<Actor[]>{
    let query = encodeURIComponent(input.trim())
    return this.client.get<Actor[]>(this.searchActorsUrl + query);
  }
  getActor(actorId : number) : Observable<Actor>{
    return this.client.get<Actor>(this.actorsUrl + '/' + actorId);
  }
  getActorsMovies(actorId : number) : Observable<Movie[]> {
      return this.client.get<Movie[]>(this.actorMoviesUrl + actorId);
  }
  addActor(actor : object) : Observable<any>{
    console.log(actor);
    return this.client.post<any>(this.actorsUrl, actor, {observe : 'response'});
  }
  uppdateActor(actorId : number, actor : object) : Observable<any>{
    console.log(actor);
    return this.client.put<any>(this.actorsUrl + '/' + actorId, actor, {observe : 'response'});
  }
  deleteActor(actorId : number) : Observable<any>{
    return this.client.delete<Actor>(this.actorsUrl + '/' + actorId);
  }
}
