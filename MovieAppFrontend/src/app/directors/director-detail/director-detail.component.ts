import { Component, Input, OnInit} from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Director } from 'src/app/shared/models/director';
import { DirectorService } from 'src/app/shared/services/director.service';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';
import { UserStoreService } from 'src/app/shared/services/user-store.service';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { catchError, of, switchMap } from 'rxjs';
import { Movie } from 'src/app/shared/models/movie';

@Component({
  selector: 'app-director-detail',
  templateUrl: './director-detail.component.html',
  styleUrls: ['./director-detail.component.css']
})
export class DirectorDetailComponent implements OnInit{

  director : Director | undefined = undefined;
  directorMovies : Movie[] | undefined = undefined;
  dp = new DefaultPhoto();
  userRole = "";

  constructor(private route : ActivatedRoute, private location : Location, private router : Router, private directorService : DirectorService, private userStoreService : UserStoreService, private authenticationService : AuthenticationService) {

  }

  ngOnInit(): void {
    this.getDirector();

    this.userStoreService.getRoleFromStore().subscribe(res => {
      this.userRole = res || this.authenticationService.getRoleFromToken();
    })
  }

  getDirector() {

    const id = Number(this.route.snapshot.paramMap.get('directorId'));
    
    this.directorService.getDirector(id).pipe(
      switchMap(director => {
        this.director = director;
        return this.directorService.getDirectorMovies(id);
      }),
      catchError((error) => {
        if(error.status == 404)
          this.router.navigate(['/404']);
        return of(error);
      })
    ).subscribe(movies => this.directorMovies = movies);
  }
  
  deleteDirector(directorId : number)
  {
    this.directorService.deleteDirector(directorId).subscribe(response => {this.location.back()}, error => console.log(error));
  }
  

}
