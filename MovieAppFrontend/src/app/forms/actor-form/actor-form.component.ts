import { Component, Output,EventEmitter, OnInit } from '@angular/core';
import { ActorsService } from 'src/app/shared/services/actors.service';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { UserStoreService } from 'src/app/shared/services/user-store.service';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-actor-form',
  templateUrl: './actor-form.component.html',
  styleUrls: ['./actor-form.component.css']
})
export class ActorFormComponent implements OnInit {

  @Output() onSubmision = new EventEmitter<any>();
  actor = {
    actorId : 0,
    name : "",
    photo : "",
    born : new Date('01/01/0001'),
    oscarWins : 0,
    oscarNominations : 0
  }
  id = 0;
  errors = "";
  userRole = "";

  constructor(private actorService : ActorsService, private route : ActivatedRoute, private location : Location) {

  }
  ngOnInit(): void {

    this.id = Number(this.route.snapshot.paramMap.get('actorId'));
    if(this.id)
    {
      this.actorService.getActor(this.id).subscribe(actor => {
        this.actor = actor;
      });
    }
  
  }

  addActorSubmit(data : any){

    if(this.id == 0)
    {
      this.actorService.addActor(this.actor).subscribe(response => this.onSubmision.emit(true),  error => this.errors = error.error);
      
    }
    else 
    {
      this.actor.actorId = this.id;
      this.actorService.uppdateActor(this.id, this.actor).subscribe(response => {
        this.location.back();
        },  error => this.errors = error.error)
    }

  }

}
