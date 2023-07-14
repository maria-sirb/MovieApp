import { Component, Input, OnInit} from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Director } from 'src/app/shared/models/director';
import { DirectorService } from 'src/app/shared/services/director.service';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';
import { UserStoreService } from 'src/app/shared/services/user-store.service';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-director-detail',
  templateUrl: './director-detail.component.html',
  styleUrls: ['./director-detail.component.css']
})
export class DirectorDetailComponent implements OnInit{

  @Input() director? : Director;
  dp = new DefaultPhoto();
  userRole = "";

  constructor(private route : ActivatedRoute, private location : Location, private directorService : DirectorService, private userStoreService : UserStoreService, private authenticationService : AuthenticationService) {

  }

  ngOnInit(): void {
    this.getDirector();

    this.userStoreService.getRoleFromStore().subscribe(res => {
      this.userRole = res || this.authenticationService.getRoleFromToken();
    })
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
