import { Component, OnInit } from '@angular/core';
import { DirectorService } from 'src/app/shared/services/director.service';
import { Director } from 'src/app/shared/models/director';
import { ActivatedRoute, Router } from '@angular/router';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';

@Component({
  selector: 'app-director',
  templateUrl: './director.component.html',
  styleUrls: ['./director.component.css']
})
export class DirectorComponent implements OnInit{

  directors : Director[] = [];
  sortMode = this.route.snapshot.paramMap.get('sortMode');
  dp = new DefaultPhoto();
  constructor(private directorService : DirectorService, private route : ActivatedRoute, private router : Router) {

  }
  ngOnInit(): void {
    
    this.directorService.getDirectors().subscribe(directors => this.directors = directors);
  }
  updateSort(sortOption : any)
  {
    this.sortMode = sortOption;
    this.router.navigate([`../${this.sortMode}`], { relativeTo: this.route });
    console.log(this.sortMode);
    this.sortActors();
  }
  sortActors()
  {
    console.log(this.sortMode);
    if(this.sortMode == 'none')
    {
      this.directors.sort((director1, director2) => director1.directorId < director2.directorId? -1 : 1);
    }
    if(this.sortMode == 'nameAsc')  
    {
      this.directors.sort((director1, director2) => director1.name < director2.name? -1 : 1);
    }
    if(this.sortMode == 'nameDesc')  
    {
      this.directors.sort((director1, director2) => director1.name < director2.name? 1 : -1);
    }
    if(this.sortMode == 'oscarWins')  
    {
      this.directors.sort((director1, director2) => {
        if (director1.oscarWins < director2.oscarWins) return 1;
  if (director1.oscarWins > director2.oscarWins) return -1;
  if (director1.oscarNominations < director2.oscarNominations) return 1;
  if (director1.oscarNominations > director2.oscarNominations) return -1;
  return 0;
      });
    }
  }
  
 

}
