import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Review } from '../models/review';
import { query } from '@angular/animations';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
import { Movie } from '../models/movie';

@Injectable({
  providedIn: 'root'
})
export class ReviewsService {

  baseUrl = environment.apiUrl + "/Review";
  constructor(private client : HttpClient) { }

  getMovieReviews(movieId : number) : Observable<any[]>{
    return this.client.get<any[]>(this.baseUrl + "/movie/" + movieId);
  }
  getAverageMovieRating(movieId : number) : Observable<number>{
    return this.client.get<number>(this.baseUrl + "/average/" + movieId);
  } 
  getUserReviews(userId : number) : Observable<Review[]>{
    return this.client.get<Review[]>(this.baseUrl + "/user/" + userId);
  }
  getReviewAuthor(reviewId : number) : Observable<User>{
    return this.client.get<User>(this.baseUrl + "/author/" + reviewId);
  }
  getReviewedMovie(reviewId : number) : Observable<Movie>{
    return this.client.get<Movie>(this.baseUrl + "/reviewed/" + reviewId);
  }
  addReview(userId : number, movieId : number, review : object) : Observable<any>{
    let queryParams = new HttpParams();
    queryParams = queryParams.append("userId", userId);
    queryParams = queryParams.append("movieId", movieId);
    return this.client.post<any>(this.baseUrl, review, {params : queryParams, observe : 'response'});
  }
  deleteReview(reviewId : number) : Observable<any>
  {
    return this.client.delete<any>(this.baseUrl + "/" + reviewId);
  }

}
