import { Component, OnInit, Input } from '@angular/core';
import { Actor } from 'src/app/shared/models/actor';
import { ActorsService } from 'src/app/shared/services/actors.service';
import { ActivatedRoute, DefaultUrlSerializer } from '@angular/router';
import { Location } from '@angular/common';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';



@Component({
  selector: 'app-actor-detail',
  templateUrl: './actor-detail.component.html',
  styleUrls: ['./actor-detail.component.css']
})
export class ActorDetailComponent {

  @Input() actor? : Actor;
  dp = new DefaultPhoto();
  constructor (private route : ActivatedRoute, private location : Location, private actorService : ActorsService) {

  }
  ngOnInit() {
    this.getActor();
  }
  getActor()  {
    const id = Number(this.route.snapshot.paramMap.get('actorId'));
    this.actorService.getActor(id).subscribe(actor => {
      this.actor = actor;
      this.actorService.getActorsMovies(id).subscribe(movies => actor.movieActors = movies);
    });
  }
  deleteActor(actorId : number)
  {
    this.actorService.deleteActor(actorId).subscribe(response => {console.log(response); this.location.back()}, error => console.log(error));
  }
 
}
