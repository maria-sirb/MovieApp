import { Component, Input, OnInit} from '@angular/core';
import { Movie } from '../../shared/models/movie';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { MovieService } from '../../shared/services/movie.service';
import { Role } from '../../shared/models/role';
import { TrueFalse } from 'src/app/add/add.component';
import { RoleFormComponent } from 'src/app/forms/role-form/role-form.component';
import { Router } from '@angular/router';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';

@Component({
  selector: 'app-movie-detail',
  templateUrl: './movie-detail.component.html',
  styleUrls: ['./movie-detail.component.css']
})
export class MovieDetailComponent implements OnInit {

  @Input() movie?: Movie;
  addRole: TrueFalse = {value : false};
  dp = new DefaultPhoto();
  constructor (private route : ActivatedRoute, private movieService : MovieService, private location : Location, private router : Router)
  {
    this.addRole.value = false;
   
  }
  ngOnInit(): void {
      this.getMovie();
  }
  getMovie()
  {
    const id = Number(this.route.snapshot.paramMap.get('movieId'));
    this.movieService.getMovie(id).subscribe(movie => {
      this.movie = movie;
      this.movieService.getMovieDirector(id).subscribe(director => movie.director = director);
      this.movieService.getMovieActors(id).subscribe(actors => {
        movie.movieActors = actors;
        movie.roles = new Map<number, string>();
        actors.forEach(actor => {
          this.movieService.getMovieRole(id, actor.actorId).subscribe(role => 
            {
              console.log(role);
              console.log(movie.roles);
              movie.roles.set(actor.actorId, role.role)
             
            }
          );
        });
      });
      this.movieService.getMovieGenres(id).subscribe(genres => movie.movieGenres = genres);
     

    });
  }

  Role(movieId : number, actorId : number) {

    let role : Role | undefined;
    this.movieService.getMovieRole(movieId, actorId).subscribe(r => {role = r;
    console.log(role);});
   // console.log(role);
    return role;
  }
  
  toggleButton(button : TrueFalse)
  {
    button.value = !button.value;
  }

  hasSubmitted(submitted : any, button : TrueFalse)
  {
    console.log(submitted);
    if(submitted)
      this.toggleButton(button);
  }
  deleteMovie(movieId : number)
  {
    this.movieService.deleteMovie(movieId).subscribe(response => {console.log(response); this.location.back()}, error => console.log(error));
  }
  deleteRole(movieId : number, actorId : number)
  {
    this.movieService.deleteMovieRole(movieId, actorId).subscribe(response => {
      console.log(response);
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
        this.router.navigate([`/movie-detail/${movieId}`]);
    }); 
    }, 
    error => console.log(error));
  }
  
  
}

 


