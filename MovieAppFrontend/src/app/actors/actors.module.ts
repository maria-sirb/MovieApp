import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActorComponent } from './actor/actor.component';
import { ActorDetailComponent } from './actor-detail/actor-detail.component';
import { AppRoutingModule } from '../app-routing.module';
import { Form, FormsModule } from '@angular/forms';
import { MoviesModule } from '../movies/movies.module';
import { PaginationComponent } from '../pagination/pagination.component';
import { PaginationModule } from '../pagination/pagination.module';



@NgModule({
  declarations: [
    ActorComponent,
    ActorDetailComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    FormsModule,
    MoviesModule,
    PaginationModule
  ],
  exports : [
    ActorComponent,
    ActorDetailComponent
  ]
})
export class ActorsModule { }
