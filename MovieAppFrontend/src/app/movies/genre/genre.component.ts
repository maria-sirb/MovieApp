import { Component, Input, OnInit, Output } from '@angular/core';
import { Genre } from 'src/app/shared/models/genre';
import { GenreService } from 'src/app/shared/services/genre.service';
import { EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-genre',
  templateUrl: './genre.component.html',
  styleUrls: ['./genre.component.css']
})
export class GenreComponent implements OnInit{

  genres :Genre[] = [];
  activeGenreId = undefined;
  @Output() pickedGenre = new EventEmitter<number>();
  constructor(private genreService : GenreService, private router : Router, private route : ActivatedRoute) {

  }
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.activeGenreId = params['genreId'];
    })
    this.genreService.getGenres().subscribe(genres => this.genres = genres);
  }
  pickGenre(genre? : Genre){
    this.pickedGenre.emit(genre?.genreId || undefined);
  }
}
