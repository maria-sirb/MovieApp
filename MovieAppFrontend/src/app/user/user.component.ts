import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../shared/services/authentication.service';
import { User } from '../shared/models/user';
import { Movie } from '../shared/models/movie';
import { WatchlistService } from '../shared/services/watchlist.service';
import { UserStoreService } from '../shared/services/user-store.service';
import { DefaultPhoto } from '../shared/functions/default-photos';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit{
  
  user : User =  {
    userId: 0,
    username: ''
  }
  userId = 0;
  watchlist : Movie[] = [];//the watchlist of the user whose profile is being viewed
  currentUserWatchlist : Movie[] | undefined = undefined// the watchlist of the currently active user
  currentUserId = 0;
  dp = new DefaultPhoto();

  constructor(private authService : AuthenticationService, private userStoreService : UserStoreService, private watchlistService : WatchlistService, private route : ActivatedRoute, private router : Router) {}

  ngOnInit(): void {
    
    this.route.paramMap.subscribe(paramMap => 
      {
        this.userId = Number(paramMap.get('userId'));
        this.authService.getUserById(this.userId).subscribe(user => this.user = user, error => error.status == 404 ? this.router.navigate(['/404']) : console.log(error));
        this.watchlistService.getUserWatchlist(this.userId).subscribe(watchlist => this.watchlist = watchlist, error => console.log(error));
    });

    this.userStoreService.getIdFromStore().subscribe(id =>
      {  
        this.currentUserId = Number(id) || Number(this.authService.getIdFromToken());
        if(this.currentUserId)
          this.watchlistService.getUserWatchlist(this.currentUserId).subscribe(watchlist => this.currentUserWatchlist = watchlist, error => console.log(error));
        else
          this.currentUserWatchlist = [];
      }
    )
   
  }



}
