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
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

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
  movie = {
      movieId : 0,
      title : "",
      imdbRating: 0,
      runTime : 0,
      releaseYear : 0,
      summary: "",
      storyLine: "",
      oscarWins: 0,
      oscarNominations: 0,
      poster : "",
  };
  id = Number(this.route.snapshot.paramMap.get('movieId'));
  constructor(private genreService : GenreService, private directorService : DirectorService, private movieService : MovieService, private route : ActivatedRoute, private location : Location){
  }
  ngOnInit(): void {

    this.genreService.getGenres().subscribe(genres => this.genreOptions = genres);
     //id = Number(this.route.snapshot.paramMap.get('movieId'));
    console.log(this.id);
    if(this.id)
    {
      this.movieService.getMovie(this.id).subscribe(movie => this.movie = movie);
      this.movieService.getMovieDirector(this.id).subscribe(director => this.director = director);
      console.log(this.director);
      this.movieService.getMovieGenres(this.id).subscribe(genres => this.checkedGenres = genres);
      console.log(this.checkedGenres);
      
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
    
    console.log(data);
    this.directorService.getDirectorByName(data.director).subscribe(d => {this.director = d;
      console.log(this.director);
      if(this.director.directorId == 0)
      this.directorError = true;
      else
      this.directorError = false;
      if(this.id != 0)
      {
         this.movie.movieId = this.id;
         this.movieService.updateMovie(this.id, this.movie, this.checkedGenres, this.director).subscribe(response =>
          {
            console.log(response);
            this.location.back();
          }, 
          error => {
            console.log(error); 
          if(error.status == 200)
          this.onSubmision.emit(true);
          });
      }
      else
      {
        this.movieService.addMovie(this.movie, this.checkedGenres, this.director).subscribe(response =>
          console.log(response), 
          error => {
            console.log(error.status); 
          if(error.status == 200)
          this.onSubmision.emit(true);
        });
      }
      
    },
    error => {this.directorError = true;});
    
   // console.log(this.checkedGenres);  

  }

}
