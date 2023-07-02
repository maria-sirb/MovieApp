import { Component, OnInit, Output } from '@angular/core';
import { Genre } from 'src/app/shared/models/genre';
import { GenreService } from 'src/app/shared/services/genre.service';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'app-genre',
  templateUrl: './genre.component.html',
  styleUrls: ['./genre.component.css']
})
export class GenreComponent implements OnInit{

  genres :Genre[] = [];
  @Output() onGenrePicked = new EventEmitter<any>();
  constructor(private genreService : GenreService) {

  }
  ngOnInit(): void {
    this.genreService.getGenres().subscribe(genres => this.genres = genres);
  }
  pickGenre(genre? : Genre) {
    this.onGenrePicked.emit(genre);
  }
}
