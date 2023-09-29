import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { DirectorService } from 'src/app/shared/services/director.service';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

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
  errors = "";

  constructor(private directorService : DirectorService, private route : ActivatedRoute, private router : Router, private location : Location) {

  }
  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('directorId'));
    if(this.id)
    {
      this.directorService.getDirector(this.id).subscribe(director => {
      this.director = director;
      });
    }
  
  }

  addDirectorSubmit(data : any){
    if(data.born == "")
      this.director.born = new Date('01/01/0001');
    if(this.id == 0)
    {
      this.directorService.addDirector(this.director).subscribe(createdId => {
        this.onSubmision.emit(true);
        this.router.navigate(['/director-detail/' + createdId]);

      },  error => this.errors = error.error)
    }
    else {
      this.director.directorId = this.id;
      this.directorService.updateDirector(this.id, this.director).subscribe(response => 
        {
          this.location.back();
        }, 
        error => this.errors = error.error)

    }

  }
}
