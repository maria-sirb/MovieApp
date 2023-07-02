import { Component, Output,EventEmitter, OnInit } from '@angular/core';
import { ActorsService } from 'src/app/shared/services/actors.service';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

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
  constructor(private actorService : ActorsService, private route : ActivatedRoute, private location : Location) {

  }
  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('actorId'));
    this.actorService.getActor(this.id).subscribe(actor => {
      this.actor = actor;
     // this.actorToEdit.born = new Date(new Date(actor.born).toISOString().split('T')[0]);
    });
  
  }
  addActorSubmit(data : any){
    console.log(data);
  
    if(this.id == 0)
    {
      this.actorService.addActor(this.actor).subscribe(response => console.log(response), error => {
        console.log(error);
        if(error.status == 200)
          this.onSubmision.emit(true);
      })
      
    }
    else 
    {
      this.actor.actorId = this.id;
      this.actorService.uppdateActor(this.id, this.actor).subscribe(response => {
        console.log(response);
        this.location.back();
        }, error => {
        console.log(error);
      })
    }

  }
}
