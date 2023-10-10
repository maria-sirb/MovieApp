import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { of, switchMap } from 'rxjs';
import { Movie } from 'src/app/shared/models/movie';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { MovieService } from 'src/app/shared/services/movie.service';
import { UserStoreService } from 'src/app/shared/services/user-store.service';
import { WatchlistService } from 'src/app/shared/services/watchlist.service';

@Component({
  selector: 'app-movie-list',
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.css']
})
export class MovieListComponent  implements OnInit{

  @Input() movies? : Movie[] | undefined;
  currentUserWatchlist : Movie[] | undefined;
  constructor(private movieService : MovieService, private authService : AuthenticationService, private userStoreService : UserStoreService, private watchlistService : WatchlistService, private route : ActivatedRoute, private router : Router){
     
  }
  ngOnInit() : void {   
    this.userStoreService.getIdFromStore().pipe(
      switchMap(id => {
        var currentUserId = Number(id) || Number(this.authService.getIdFromToken());
        if(!currentUserId){
          return of([]);
        }
        else{
          return this.watchlistService.getUserWatchlist(currentUserId);
        }
      })
    ).subscribe(watchlist => this.currentUserWatchlist = watchlist);
  }
}
