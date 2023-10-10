import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import {Observable} from 'rxjs';
import { Actor } from '../models/actor';
import { Movie } from '../models/movie';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ActorsService {

  actorsUrl : string =  environment.apiUrl + '/Actor';
  constructor(private client : HttpClient) { }
  getActors() : Observable<Actor[]>{
      return this.client.get<Actor[]>(this.actorsUrl);
  }
  getActorsPaged(pageNumber : number, orderBy : string | null) : Observable<HttpResponse<Actor[]>>{
    let queryParams = new HttpParams();
    queryParams = queryParams.append("PageNumber", pageNumber);
    queryParams = queryParams.append("PageSize", 25);
    if(orderBy)
      queryParams = queryParams.append("OrderBy", orderBy);
    return this.client.get<Actor[]>(this.actorsUrl + '/paged', {params : queryParams, observe : 'response'});
  }
  getActorByName(name : string) : Observable<Actor> {
    return this.client.get<Actor>(this.actorsUrl + '/actor-name/' + encodeURIComponent(name.trim()));
  }
  searchActors(input : string) : Observable<Actor[]>{
    let query = encodeURIComponent(input.trim())
    return this.client.get<Actor[]>(this.actorsUrl + '?input=' + query);
  }
  getActor(actorId : number) : Observable<Actor>{
    return this.client.get<Actor>(this.actorsUrl + '/' + actorId);
  }
  getActorsMovies(actorId : number) : Observable<Movie[]> {
      return this.client.get<Movie[]>(this.actorsUrl + '/movie/'  + actorId);
  }
  addActor(actor : Actor) : Observable<number>{
    return this.client.post<number>(this.actorsUrl, actor);
  }
  uppdateActor(actorId : number, actor : Actor) : Observable<any>{
    return this.client.put<any>(this.actorsUrl + '/' + actorId, actor, {observe : 'response'});
  }
  deleteActor(actorId : number) : Observable<any>{
    return this.client.delete<any>(this.actorsUrl + '/' + actorId);
  }
}
