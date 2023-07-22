import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Vote } from '../models/vote';

@Injectable({
  providedIn: 'root'
})
export class VoteService {

  baseUrl = "https://localhost:7172/api/Vote";
  constructor(private client : HttpClient) { }

  getReviewVotes(reviewId : number) : Observable<any[]>
  {
    return this.client.get<any[]>(this.baseUrl + "/" + reviewId);
  }
  getUserReviewVote(reviewId : number, userId : number) : Observable<any>
  {
    return this.client.get<any>(this.baseUrl + "/" + reviewId + "/" + userId);
  }
  addVote(vote : object, userId : number, reviewId : number) : Observable<any>
  {
    let queryParams = new HttpParams();
    queryParams = queryParams.append("userId", userId);
    queryParams = queryParams.append("reviewId", reviewId);
    return this.client.post<any>(this.baseUrl, vote, {params: queryParams, observe : 'response'});
  }
  editVote(vote : Vote) : Observable<any>
  {
    return this.client.put<Vote>(this.baseUrl + "/" + vote.voteId, vote);
  }
}
