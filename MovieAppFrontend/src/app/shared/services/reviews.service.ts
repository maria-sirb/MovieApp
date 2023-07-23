import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Review } from '../models/review';
import { query } from '@angular/animations';

@Injectable({
  providedIn: 'root'
})
export class ReviewsService {

  baseUrl = "https://localhost:7172/api/Review";
  constructor(private client : HttpClient) { }

  getMovieReviews(movieId : number) : Observable<any[]>{
    return this.client.get<any[]>(this.baseUrl + "/movie/" + movieId);
  }
  getAverageMovieRating(movieId : number) : Observable<number>{
    return this.client.get<number>(this.baseUrl + "/average/" + movieId);
  } 
  getUserReviews(userId : number) : Observable<any[]>{
    return this.client.get<any[]>(this.baseUrl + "/user/" + userId);
  }
  getReviewAuthor(reviewId : number) : Observable<any>{
    return this.client.get<any>(this.baseUrl + "/author/" + reviewId);
  }
  addReview(userId : number, movieId : number, review : object) : Observable<any>{
    let queryParams = new HttpParams();
    queryParams = queryParams.append("userId", userId);
    queryParams = queryParams.append("movieId", movieId);
    return this.client.post<any>(this.baseUrl, review, {params : queryParams, observe : 'response'});
  }

}
