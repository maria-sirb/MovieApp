import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';
import { Movie } from 'src/app/shared/models/movie';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { UserStoreService } from 'src/app/shared/services/user-store.service';
import { WatchlistService } from 'src/app/shared/services/watchlist.service';

@Component({
  selector: 'app-movie-card',
  templateUrl: './movie-card.component.html',
  styleUrls: ['./movie-card.component.css']
})
export class MovieCardComponent implements OnInit{

  @Input() movie : Movie | undefined;
  @Input() watchlist : Movie[] | undefined;
  currentUserId = 0;
 // watchlist : Movie[] = [];
  dp = new DefaultPhoto();
  isCurrentUserInWatchlist = false;

  constructor(private userStoreService : UserStoreService, private authService : AuthenticationService, private watchlistService : WatchlistService, private router : Router) {
  }

  ngOnInit(): void {
    this.userStoreService.getIdFromStore().subscribe(id => {
      this.currentUserId = Number(id) || Number(this.authService.getIdFromToken()) || 0; 
      if(this.currentUserId == 0)
        this.isCurrentUserInWatchlist = false;
      else if(this.watchlist)
      {
        this.watchlist.forEach(movie => {
        if(movie.movieId == this.movie?.movieId)
          this.isCurrentUserInWatchlist = true;
        })
      }
      
    }); 
   
  }

  addMovieToWatchlist(e : Event) : void {
    e.stopPropagation();
    if(this.currentUserId == 0 || !this.currentUserId)
      this.router.navigate(['/login']);
    else if(this.movie && !this.isCurrentUserInWatchlist)
    { 
      this.watchlistService.addToWatchlist({userId : this.currentUserId, movieId : this.movie.movieId}).subscribe(res => console.log(res), err => console.log(err));
      this.isCurrentUserInWatchlist = true;
    }
    else if(this.movie && this.isCurrentUserInWatchlist)
    {
      this.watchlistService.deleteFromWatchlist(this.currentUserId, this.movie.movieId).subscribe(res => console.log(res), err => console.log(err));
      this.isCurrentUserInWatchlist = false;
    }
  }

  navigateToMovieDetail(){
    this.router.navigate([`/movie-detail/${this.movie?.movieId}`]);
  }

}

