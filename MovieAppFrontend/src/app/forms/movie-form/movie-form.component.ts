import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Genre } from 'src/app/shared/models/genre';
import { GenreService } from 'src/app/shared/services/genre.service';
import { MovieService } from 'src/app/shared/services/movie.service';
import { Movie } from 'src/app/shared/models/movie';
import { Director } from 'src/app/shared/models/director';
import { DirectorService } from 'src/app/shared/services/director.service';
import { NgModel } from '@angular/forms';
import { NgForm } from '@angular/forms';
import { TrueFalse } from 'src/app/add/add.component';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { catchError, of, switchMap } from 'rxjs';

@Component({
  selector: 'app-movie-form',
  templateUrl: './movie-form.component.html',
  styleUrls: ['./movie-form.component.css']
})
export class MovieFormComponent implements OnInit{

  @Output() onSubmision = new EventEmitter<any>();
  genreOptions : Genre[] = [];
  checkedGenres : Genre[] = [];
  director : Director = {
  directorId : 0,
  name : "",
  photo : "",
  born : new Date('01/01/0001'),
  oscarWins : 0,
  oscarNominations : 0,
  movies : []
  };
  directorError? : boolean = false;
  movie : Movie = {
      movieId : 0,
      title : "",
      imdbRating : undefined,
      runTime : undefined,
      releaseYear : undefined,
      summary: undefined,
      storyLine: undefined,
      oscarWins: undefined,
      oscarNominations: undefined,
      poster : "",
  };
  id = Number(this.route.snapshot.paramMap.get('movieId'));
  errors = "";

  constructor(private genreService : GenreService, private directorService : DirectorService, private movieService : MovieService, private route : ActivatedRoute, private location : Location, private router : Router){
  }
  ngOnInit(): void {

    this.genreService.getGenres().subscribe(genres => this.genreOptions = genres);
     //id = Number(this.route.snapshot.paramMap.get('movieId'));
    console.log(this.id);
    if(this.id)
    {
      this.movieService.getMovie(this.id).subscribe(movie => this.movie = movie);
      this.movieService.getMovieDirector(this.id).subscribe(director => this.director = director);
      this.movieService.getMovieGenres(this.id).subscribe(genres => this.checkedGenres = genres);
    }
  }

  UpdateCheckedGenres(genre : Genre, checked : boolean)
  {
    if(checked)
      this.checkedGenres.push(genre);
     else
       this.checkedGenres.splice(this.checkedGenres.indexOf(genre), 1); 
     console.log(this.checkedGenres);  

  }

  isEnabled(genre : Genre)
  {
    let found : boolean = false;
     this.checkedGenres.forEach(
      current => {
      if(current.name == genre.name)
        {
          found = true;
        }
    });
    return found;
  }

  addMovieSubmit(data : any)
  {
    this.directorService.getDirectorByName(data.director).pipe(
      catchError((error) => {
        this.directorError = true;
        console.log(this.directorError);
        return of();
      }),
      switchMap((director) => {
        this.director = director;
        this.director.directorId == 0 ? this.directorError = true : this.directorError = false;
        if(this.id != 0)
        {
          this.movie.movieId = this.id;
          return this.movieService.updateMovie(this.id, this.movie, this.checkedGenres, this.director);
        }
        else 
        {
          return this.movieService.addMovie(this.movie, this.checkedGenres, this.director);
        }
      })  
    ).subscribe(res => {
      this.onSubmision.emit(true);
      if(typeof(res) === 'number')
        this.router.navigate(['/movie-detail/' + res]);
      else 
        this.location.back();
    })

  }
}
