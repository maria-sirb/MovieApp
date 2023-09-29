import { Component } from '@angular/core';
import { ReviewsService } from 'src/app/shared/services/reviews.service';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Review } from 'src/app/shared/models/review';
import { empty, switchMap } from 'rxjs';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { UserStoreService } from 'src/app/shared/services/user-store.service';

@Component({
  selector: 'app-review-form',
  templateUrl: './review-form.component.html',
  styleUrls: ['./review-form.component.css']
})
export class ReviewFormComponent {

  stars : number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
  selectedStar : number =  0;
  inputValue : null | number = null;
  review = {
    rating : 0,
    title : "",
    text : ""
  }
  movieId = Number(this.route.snapshot.paramMap.get('movieId'));
  errors = "";

  constructor(private reviewService : ReviewsService, private location : Location, private authenticationService : AuthenticationService, private userStore : UserStoreService, private route : ActivatedRoute) {}
  cancel(){
   this.location.back();
  }

  countStars(star : number){
    this.selectedStar = star;
    this.inputValue = star;
    this.review.rating = star;
  }

  addReview(){
    this.userStore.getIdFromStore().pipe(
      switchMap((id) => {
        const userId = Number(id) || Number(this.authenticationService.getIdFromToken());
        return this.reviewService.addReview(userId, this.movieId, this.review);
      })
    ).subscribe(response => this.location.back(), error => this.errors = error.error);
   
  }
}
