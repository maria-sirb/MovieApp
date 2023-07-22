import { NgModule} from '@angular/core';
import { CommonModule} from '@angular/common';
import { MoviesComponent } from './movies.component';
import { MovieDetailComponent } from './movie-detail/movie-detail.component';
import { AppRoutingModule } from '../app-routing.module';
import { GenreComponent } from './genre/genre.component';
import { AllFormsModule } from '../forms/all-forms.module';
import { FormsModule } from '@angular/forms';
import { AppModule } from '../app.module';
import { ReviewsModule } from '../reviews/reviews.module';


@NgModule({
  declarations: [
    MoviesComponent,
    MovieDetailComponent,
    GenreComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    AllFormsModule,
    FormsModule,
    ReviewsModule
  ],

  exports : [
   MoviesComponent,
   MovieDetailComponent
  ]
  
})

export class MoviesModule { }
