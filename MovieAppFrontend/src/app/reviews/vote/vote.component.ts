import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/shared/models/user';
import { Vote } from 'src/app/shared/models/vote';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { UserStoreService } from 'src/app/shared/services/user-store.service';
import { VoteService } from 'src/app/shared/services/vote.service';

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html',
  styleUrls: ['./vote.component.css']
})
export class VoteComponent implements OnInit{
  @Input() reviewId? : number;
  currentUserId = 0;
  likesNo = 0;
  dislikesNo = 0;
  currentVote : Vote = {
    voteId : 0,
    isLike : false,
    isDislike : false
  };

  constructor(private voteService : VoteService, private authenticationService : AuthenticationService, private userStoreService : UserStoreService) { 
  }

  ngOnInit() : void{
   
    if(this.reviewId)
      this.voteService.getReviewVotes(this.reviewId).subscribe(votes => 
        votes.forEach(vote => {
          if(vote.isLike) 
            this.likesNo++;
          else if(vote.isDislike)
            this.dislikesNo++;}
        ), error => console.log(error));

    this.userStoreService.getUsernameFromStore().subscribe(res => {
      const username = res || this.authenticationService.getUsernameFromToken();
      this.authenticationService.getUser(username).subscribe(user => {
        if(user == null)
        {
          this.currentVote = {voteId : 0, isLike : false, isDislike : false};
        }
        else if(this.reviewId)
        {
          this.currentUserId = user.userId;
          this.voteService.getUserReviewVote(this.reviewId, user.userId).subscribe(vote => {
            this.currentVote = vote || {voteId : 0, isLike : false, isDislike : false};
          }, error => console.log(error));
        }
      }, error => console.log(error));
    })  
  }

  like(){
    
    if(this.currentVote.isLike){
      this.currentVote.isLike = false;
      this.likesNo--;
    }
    else if(this.currentVote.isDislike){
      this.currentVote.isDislike = false;
      this.currentVote.isLike = true;
      this.likesNo++;
      this.dislikesNo--;
    } 
    else{ 
      this.currentVote.isLike = true;
      this.likesNo++;
    }
    this.vote();
  }
  dislike(){
    
    if(this.currentVote.isDislike){
      this.currentVote.isDislike = false;
      this.dislikesNo--;
    }
    else if(this.currentVote.isLike){
      this.currentVote.isLike = false;
      this.currentVote.isDislike = true;
      this.dislikesNo++;
      this.likesNo--;
    }
    else{
      this.currentVote.isDislike = true;
      this.dislikesNo++;
    }
    this.vote();
  }

  vote()
  {
    if(this.currentVote.voteId == 0)
      this.voteService.addVote(this.currentVote, this.currentUserId, this.reviewId || 0).subscribe(res => console.log(res), error => console.log(error));
    else 
      this.voteService.editVote(this.currentVote).subscribe(res => console.log(res), error => console.log(error));
  }
}
