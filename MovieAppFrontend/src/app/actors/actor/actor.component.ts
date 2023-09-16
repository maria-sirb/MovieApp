import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';
import { Actor } from 'src/app/shared/models/actor';
import { ActorsService } from 'src/app/shared/services/actors.service';


@Component({
  selector: 'app-actor',
  templateUrl: './actor.component.html',
  styleUrls: ['./actor.component.css']
})
export class ActorComponent implements OnInit{

  actors : Actor[] = [];
  sortMode = this.route.snapshot.paramMap.get('sortMode');  
  dp = new DefaultPhoto();
  constructor( private actorsService : ActorsService, private route : ActivatedRoute, private router : Router){

  }
  ngOnInit(): void {
    this.actorsService.getActors().subscribe(actors => {this.actors = actors; this.sortActors()});
  }
  updateSort(sortOption : any)
  {
    this.sortMode = sortOption;
    this.router.navigate([`../${this.sortMode}`], { relativeTo: this.route });
    this.sortActors();
  }
  sortActors()
  {
    if(this.sortMode == 'none')
    {
      this.actors.sort((actor1, actor2) => actor1.actorId < actor2.actorId? -1 : 1);
    }
    if(this.sortMode == 'nameAsc')  
    {
      this.actors.sort((actor1, actor2) => actor1.name < actor2.name? -1 : 1);
    }
    if(this.sortMode == 'nameDesc')  
    {
      this.actors.sort((actor1, actor2) => actor1.name < actor2.name? 1 : -1);
    }
    if(this.sortMode == 'oscarWins')  
    {
      this.actors.sort((actor1, actor2) => {
        if (actor1.oscarWins < actor2.oscarWins) return 1;
  if (actor1.oscarWins > actor2.oscarWins) return -1;
  if (actor1.oscarNominations < actor2.oscarNominations) return 1;
  if (actor1.oscarNominations > actor2.oscarNominations) return -1;
  return 0;
      });
    }
  }
  
 
}
