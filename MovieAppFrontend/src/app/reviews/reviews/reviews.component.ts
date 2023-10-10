import { Component, Input, OnInit } from '@angular/core';
import { forkJoin, switchMap } from 'rxjs';
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
        this.reviewService.getMovieReviews(this.movie.movieId).pipe(
          switchMap(reviews => {
            this.reviews = reviews;
            return forkJoin(reviews.map(review => this.reviewService.getReviewAuthor(review.reviewId)));
          })
        ).subscribe(authors => {
          authors.forEach((author, index) => this.reviews[index].user = author);
        });
    }
    else if(this.userId)
    {
        this.reviewService.getUserReviews(this.userId).pipe(
          switchMap(reviews => {
            this.reviews = reviews;
            return forkJoin(reviews.map(review => this.reviewService.getReviewedMovie(review.reviewId)));
          })
        ).subscribe(movies => {
          movies.forEach((movie, index) => this.reviews[index].movie = movie);
        });
    }
    
  }
  deleteReview(reviewId : number)
  {
    this.reviewService.deleteReview(reviewId).subscribe();
    window.location.reload();
  }
  
}
