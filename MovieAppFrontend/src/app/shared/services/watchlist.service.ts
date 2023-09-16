import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Movie } from '../models/movie';
import { Watchlist } from '../models/watchlist';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WatchlistService {

  baseUrl = environment.apiUrl + "/Watchlist";
  constructor(private client : HttpClient) { }

  getUserWatchlist(userId : number) : Observable<Movie[]>{
    return this.client.get<Movie[]>(this.baseUrl + "/" + userId);
  } 
  addToWatchlist(item : Watchlist) : Observable<any>{
    return this.client.post<Watchlist>(this.baseUrl, item, {observe : 'response'});
  }
  deleteFromWatchlist(userId : number, movieId : number): Observable<any>{
    return this.client.delete<any>(this.baseUrl + '/' + userId + '/' + movieId);
  }
}
