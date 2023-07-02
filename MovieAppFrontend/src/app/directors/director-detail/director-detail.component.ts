import { Component, Input, OnInit} from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Director } from 'src/app/shared/models/director';
import { DirectorService } from 'src/app/shared/services/director.service';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';

@Component({
  selector: 'app-director-detail',
  templateUrl: './director-detail.component.html',
  styleUrls: ['./director-detail.component.css']
})
export class DirectorDetailComponent implements OnInit{

  @Input() director? : Director;
  dp = new DefaultPhoto();
  constructor(private route : ActivatedRoute, private location : Location, private directorService : DirectorService) {

  }
  ngOnInit(): void {
    this.getDirector();
  }
  getDirector() {

    const id = Number(this.route.snapshot.paramMap.get('directorId'));
    this.directorService.getDirector(id).subscribe(director => {
      this.director = director;
      this.directorService.getDirectorMovies(id).subscribe(movies => director.movies = movies );
    });
  }
  deleteDirector(directorId : number)
  {
    this.directorService.deleteDirector(directorId).subscribe(response => {console.log(response); this.location.back()}, error => console.log(error));
  }
  

}
