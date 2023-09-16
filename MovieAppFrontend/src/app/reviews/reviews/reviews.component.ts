import { Component, Input, OnInit } from '@angular/core';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';
import { Movie } from 'src/app/shared/models/movie';
import { Review } from 'src/app/shared/models/review';
import { User } from 'src/app/shared/models/user';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { ReviewsService } from 'src/app/shared/services/reviews.service';
import { UserStoreService } from 'src/app/shared/services/user-store.service';

@Component({
  selector: 'app-reviews',
  templateUrl: './reviews.component.html',
  styleUrls: ['./reviews.component.css']
})
export class ReviewsComponent implements OnInit{

  @Input() movie? : Movie;
  @Input() userId? : number;
  reviews : Review[] = [];
  dp = new DefaultPhoto();
  currentUserId : number = 0;

  constructor(private reviewService : ReviewsService,private authService : AuthenticationService, private userStoreService : UserStoreService){

  }
  ngOnInit(): void {
    this.userStoreService.getIdFromStore().subscribe(res => {
      this.currentUserId = res || this.authService.getIdFromToken() || 0;
    })

    if(this.movie)
    {
       this.reviewService.getMovieReviews(this.movie.movieId).subscribe(reviews => 
        {
          this.reviews = reviews;
          this.reviews.forEach(review => this.reviewService.getReviewAuthor(review.reviewId).subscribe(author => review.user = author, error => console.log(error)));
        }, error => console.log(error));
    }
    else if(this.userId)
    {
      this.reviewService.getUserReviews(this.userId).subscribe(reviews => 
        {
          this.reviews = reviews
          this.reviews.forEach(review => this.reviewService.getReviewedMovie(review.reviewId).subscribe(movie => review.movie = movie, error => console.log(error)));
        }, error => console.log(error));
    }
  }
  deleteReview(reviewId : number)
  {
    this.reviewService.deleteReview(reviewId).subscribe();
    window.location.reload();
  }
  
}
