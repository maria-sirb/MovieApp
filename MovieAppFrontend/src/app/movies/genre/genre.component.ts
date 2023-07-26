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
  activeGenreId = Number(this.route.snapshot.paramMap.get('genreId'));
  @Input() sortMode? : string | null;
  constructor(private genreService : GenreService, private router : Router, private route : ActivatedRoute) {

  }
  ngOnInit(): void {
    console.log(this.sortMode);
    this.genreService.getGenres().subscribe(genres => this.genres = genres);
  }
  pickGenre(genre? : Genre) {
   // this.onGenrePicked.emit(genre);
   console.log(genre);
   this.router.navigateByUrl('actors', {skipLocationChange: true})
  .then(()=> this.router.navigate([`movies/${genre?.genreId??0}/${this.sortMode}`]));
   
  }
}
