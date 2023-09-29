import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { MovieService } from 'src/app/shared/services/movie.service';
import { Role } from 'src/app/shared/models/role';
import { Movie } from 'src/app/shared/models/movie';
import { Actor } from 'src/app/shared/models/actor';
import { ActorsService } from 'src/app/shared/services/actors.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { catchError, of, switchMap } from 'rxjs';


@Component({
  selector: 'app-role-form',
  templateUrl: './role-form.component.html',
  styleUrls: ['./role-form.component.css']
})
export class RoleFormComponent implements OnInit {

  /*@Output() onSubmision = new EventEmitter<any>();*/
  actorError : boolean = false;
  errors = "";
  role : Role = {
    movieId : 0,
    actorId : 0,
    role : ""
  }
  actor : Actor = {
    actorId : 0,
    name : "",
    photo : "",
    born : new Date('01/01/0001'),
    oscarWins : 0,
    oscarNominations : 0,
    movieActors : []
    };
    movieId = Number(this.route.snapshot.paramMap.get('movieId'));
    actorId = Number(this.route.snapshot.paramMap.get('actorId'));

  constructor(private movieService : MovieService, private actorService : ActorsService, private route : ActivatedRoute, private location : Location){
  }

  ngOnInit(): void {

   if(this.actorId)
    {
      this.actorService.getActor(this.actorId).subscribe(actor => this.actor = actor);
      this.movieService.getMovieRole(this.movieId, this.actorId).subscribe(role => this.role = role);
    }
  }
  
  cancel(){
    this.location.back();
  }

  addRoleSubmit(data : any)
  {
    let inputActor : Actor;
    this.actorService.getActorByName(this.actor.name).pipe(
      catchError((error) => {
        if(error.status == 404)
          this.actorError = true;
        return of();
      }),
      switchMap((actor) =>{
        inputActor =actor;
        if(!this.actorId)
        {
          this.role.movieId = this.movieId;
          this.role.actorId = inputActor.actorId;
          return this.movieService.addMovieRole(this.role);
        }
        else
        {
          return this.movieService.updateMovieRole(this.role);
        }
      })
    ).subscribe(result => this.location.back(), error => this.errors = error.error);

  }

}
