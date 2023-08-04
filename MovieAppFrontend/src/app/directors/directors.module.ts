import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DirectorComponent } from './director/director.component';
import { DirectorDetailComponent } from './director-detail/director-detail.component';
import { AppRoutingModule } from '../app-routing.module';
import { FormsModule } from '@angular/forms';
import { MoviesModule } from '../movies/movies.module';


@NgModule({
  declarations: [
    DirectorComponent,
    DirectorDetailComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    FormsModule,
    MoviesModule
  ],
  exports: [
    DirectorComponent,
    DirectorDetailComponent
  ]
})
export class DirectorsModule { }
