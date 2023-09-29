import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Vote } from '../models/vote';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VoteService {

  baseUrl = environment.apiUrl + "/Vote";
  constructor(private client : HttpClient) { }

  getReviewVotes(reviewId : number) : Observable<Vote[]>
  {
    return this.client.get<Vote[]>(this.baseUrl + "/" + reviewId);
  }
  getUserReviewVote(reviewId : number, userId : number) : Observable<Vote>
  {
    return this.client.get<Vote>(this.baseUrl + "/" + reviewId + "/" + userId);
  }
  addVote(vote : Vote, userId : number, reviewId : number) : Observable<any>
  {
    let queryParams = new HttpParams();
    queryParams = queryParams.append("userId", userId);
    queryParams = queryParams.append("reviewId", reviewId);
    return this.client.post<any>(this.baseUrl, vote, {params: queryParams, observe : 'response'});
  }
  editVote(vote : Vote) : Observable<any>
  {
    return this.client.put<any>(this.baseUrl + "/" + vote.voteId, vote);
  }
}
