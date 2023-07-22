import { Component, Input, OnInit } from '@angular/core';
import { Movie } from 'src/app/shared/models/movie';
import { Review } from 'src/app/shared/models/review';
import { User } from 'src/app/shared/models/user';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { ReviewsService } from 'src/app/shared/services/reviews.service';

@Component({
  selector: 'app-reviews',
  templateUrl: './reviews.component.html',
  styleUrls: ['./reviews.component.css']
})
export class ReviewsComponent implements OnInit{

  @Input() movie? : Movie;
  @Input() userId? : number;
  reviews : Review[] = [];
  constructor(private reviewService : ReviewsService){

  }
  ngOnInit(): void {
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
      this.reviewService.getUserReviews(this.userId).subscribe(result => this.reviews = result, error => console.log(error));
    }
  }
  
}
