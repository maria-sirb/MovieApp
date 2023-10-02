import { ChangeDetectorRef, Component, Input, OnInit} from '@angular/core';
import { Movie } from '../../shared/models/movie';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { MovieService } from '../../shared/services/movie.service';
import { Role } from '../../shared/models/role';
import { TrueFalse } from 'src/app/add/add.component';
import { RoleFormComponent } from 'src/app/forms/role-form/role-form.component';
import { Router } from '@angular/router';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';
import { UserStoreService } from 'src/app/shared/services/user-store.service';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { ReviewsService } from 'src/app/shared/services/reviews.service';
import { forkJoin, map, mergeMap, subscribeOn, switchMap } from 'rxjs';
import { Actor } from 'src/app/shared/models/actor';
import { Director } from 'src/app/shared/models/director';
import { Genre } from 'src/app/shared/models/genre';
import { CastMember } from 'src/app/shared/models/castMember';

@Component({
  selector: 'app-movie-detail',
  templateUrl: './movie-detail.component.html',
  styleUrls: ['./movie-detail.component.css']
})
export class MovieDetailComponent implements OnInit {

  movie : Movie = {
    movieId: 0,
    title: ''
  };
  director : Director | undefined = undefined;
  genres : Genre[] | undefined = undefined;
  cast : CastMember[] | undefined = undefined;
  addRole: TrueFalse = {value : false};
  dp = new DefaultPhoto();
  userRole = "";
  rating = 0;

  constructor (private route : ActivatedRoute, private movieService : MovieService, private location : Location, private router : Router, private userStoreService : UserStoreService, private authenticationService : AuthenticationService, private reviewService : ReviewsService)
  {
    this.addRole.value = false;
   
  }
  ngOnInit(): void {
      this.getMovie();

      this.userStoreService.getRoleFromStore().subscribe(res => {
        this.userRole = res || this.authenticationService.getRoleFromToken();
      })
  }
  getMovie()
  {
    const id = Number(this.route.snapshot.paramMap.get('movieId'));

    this.movieService.getMovie(id).subscribe(movie => {this.movie = movie; console.log(movie)}, error => error.status == 404 ? this.router.navigate(['/404']) : console.log(error));
    this.reviewService.getAverageMovieRating(id).subscribe(rating => this.rating = Math.round(rating * 10) /10, error => console.log(error));
    this.movieService.getMovieDirector(id).subscribe(director => {this.director = director; console.log(director)});
    this.movieService.getMovieGenres(id).subscribe(genres => {this.genres = genres; console.log(genres)});
    this.movieService.getMovieCast(id).subscribe(cast => {this.cast = cast; console.log(cast)});

  }
  
  deleteMovie(movieId : number)
  {
    this.movieService.deleteMovie(movieId).subscribe(response => {console.log(response); this.location.back()}, error => console.log(error));
  }

  deleteRole(movieId : number, actorId : number)
  {
    this.movieService.deleteMovieRole(movieId, actorId).subscribe(response => {
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
        this.router.navigate([`/movie-detail/${movieId}`]);
    }); 
    }, 
    error => console.log(error));
  }
  
  
}

 


