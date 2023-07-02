import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { DirectorService } from 'src/app/shared/services/director.service';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-director-form',
  templateUrl: './director-form.component.html',
  styleUrls: ['./director-form.component.css']
})
export class DirectorFormComponent implements OnInit{

  @Output() onSubmision = new EventEmitter<any>();
  director = {
    directorId : 0,
    name : "",
    photo : "",
    born : new Date('01/01/0001'),
    oscarWins : 0,
    oscarNominations : 0
  }
  id = 0;
  constructor(private directorService : DirectorService, private route : ActivatedRoute, private location : Location) {

  }
  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('directorId'));
    this.directorService.getDirector(this.id).subscribe(director => {
      this.director = director;
     // this.actorToEdit.born = new Date(new Date(actor.born).toISOString().split('T')[0]);
    });
  
  }
  addDirectorSubmit(data : any){
    console.log(data);
    /*let director = {
      directorId : 0,
      name : data.name,
      photo : data.photo,
      born : new Date(data.born),
      oscarWins : Number(data.oscarWins),
      oscarNominations : Number(data.oscarNominations)
    }*/
    //console.log(director);
    console.log(this.director);
    if(data.born == "")
      this.director.born = new Date('01/01/0001');
    console.log(this.director.born);
    if(this.id == 0)
    {
      this.directorService.addDirector(this.director).subscribe(response => console.log(response), error => {
        console.log(error);
        if(error.status == 200)
          this.onSubmision.emit(true);
      })
    }
    else {
      this.director.directorId = this.id;
      this.directorService.updateDirector(this.id, this.director).subscribe(response => 
        {
          console.log(response);
          this.location.back();
        }, 
        error => {
        console.log(error);
        
      })

    }

  }
}
