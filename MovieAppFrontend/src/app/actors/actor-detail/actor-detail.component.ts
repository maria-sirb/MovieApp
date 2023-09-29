import { Component, OnInit, Input } from '@angular/core';
import { Actor } from 'src/app/shared/models/actor';
import { ActorsService } from 'src/app/shared/services/actors.service';
import { ActivatedRoute, DefaultUrlSerializer, Router } from '@angular/router';
import { Location } from '@angular/common';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';
import { UserStoreService } from 'src/app/shared/services/user-store.service';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { catchError, of, switchMap } from 'rxjs';
import { Movie } from 'src/app/shared/models/movie';



@Component({
  selector: 'app-actor-detail',
  templateUrl: './actor-detail.component.html',
  styleUrls: ['./actor-detail.component.css']
})
export class ActorDetailComponent {

  actor : Actor | undefined = undefined;
  actorMovies : Movie[] | undefined = undefined;
  dp = new DefaultPhoto();
  userRole = "";

  constructor (private route : ActivatedRoute, private location : Location, private router : Router, private actorService : ActorsService, private userStoreService : UserStoreService, private authenticationService : AuthenticationService) {

  }
  
  ngOnInit() {
    this.getActor();

    this.userStoreService.getRoleFromStore().subscribe(res => {
      this.userRole = res || this.authenticationService.getRoleFromToken();
    })
  }

  getActor()  {
    const id = Number(this.route.snapshot.paramMap.get('actorId'));
    
    this.actorService.getActor(id).pipe(
      switchMap((actor) => {
        this.actor = actor;
        return this.actorService.getActorsMovies(id);
      }),
      catchError((error) => {
        if(error.status == 404)
          this.router.navigate(['/404'])
        return of(error)
      })).subscribe(movies => this.actorMovies = movies);
  }

  deleteActor(actorId : number)
  {
    this.actorService.deleteActor(actorId).subscribe(response => {this.location.back()}, error => console.log(error));
  }
 
}
