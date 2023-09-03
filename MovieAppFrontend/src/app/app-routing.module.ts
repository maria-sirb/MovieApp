import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MoviesComponent } from './movies/movies.component';
import { MovieDetailComponent } from './movies/movie-detail/movie-detail.component';
import { ActorComponent } from './actors/actor/actor.component';
import { ActorDetailComponent } from './actors/actor-detail/actor-detail.component';
import { DirectorComponent } from './directors/director/director.component';
import { DirectorDetailComponent } from './directors/director-detail/director-detail.component';
import { SearchComponent } from './search/search.component';
import { AddComponent } from './add/add.component';
import { RoleFormComponent } from './forms/role-form/role-form.component';
import { MovieFormComponent } from './forms/movie-form/movie-form.component';
import { ActorFormComponent } from './forms/actor-form/actor-form.component';
import { DirectorFormComponent } from './forms/director-form/director-form.component';
import { SignupFormComponent } from './forms/signup-form/signup-form.component';
import { LoginFormComponent } from './forms/login-form/login-form.component';
import { AuthGuard } from './shared/guards/auth.guard';
import { ReviewFormComponent } from './forms/review-form/review-form.component';
import { ReviewsComponent } from './reviews/reviews/reviews.component';
import { UserComponent } from './user/user.component';
import { UserFormComponent } from './forms/user-form/user-form.component';
import { ResetPasswordFormComponent } from './forms/reset-password-form/reset-password-form.component';
import { SendResetEmailFormComponent } from './forms/send-reset-email-form/send-reset-email-form.component';



const routes: Routes = [
  {path : '', redirectTo : 'movies/0/none', pathMatch: 'full'},
  {path: 'movies/:genreId/:sortMode', component: MoviesComponent },
  {path: 'movie-detail/:movieId', component : MovieDetailComponent},
  {path : 'actors', redirectTo : '/actors/none', pathMatch: 'full'},
  {path : 'actors/:sortMode', component : ActorComponent},
  {path : 'actor-detail/:actorId', component : ActorDetailComponent},
  {path : 'directors', redirectTo : '/directors/none', pathMatch: 'full'},
  {path: 'directors/:sortMode', component: DirectorComponent},
  {path: 'director-detail/:directorId', component : DirectorDetailComponent},
  {path : 'search', component : SearchComponent},
  {path : 'add', component : AddComponent, canActivate : [AuthGuard], data: {roles: 'admin'}},
  {path : 'add-role/:movieId', component : RoleFormComponent, canActivate : [AuthGuard], data: {roles: 'admin'}},
  {path : 'edit-role/:movieId/:actorId', component : RoleFormComponent, canActivate : [AuthGuard], data: {roles: 'admin'}},
  {path : 'edit-movie/:movieId', component : MovieFormComponent, canActivate : [AuthGuard], data: {roles: 'admin'}},
  {path : 'edit-actor/:actorId', component : ActorFormComponent, canActivate : [AuthGuard], data: {roles: 'admin'}},
  {path : 'edit-director/:directorId', component : DirectorFormComponent, canActivate : [AuthGuard], data: {roles: 'admin'}},
  {path: 'signup', component : SignupFormComponent},
  {path : 'login', component : LoginFormComponent},
  {path : 'add-review/:movieId', component : ReviewFormComponent, canActivate : [AuthGuard]},
  {path : ':role/:userId', component : UserComponent},
  {path : 'profile/edit/:userId', component : UserFormComponent, canActivate : [AuthGuard]},
  {path: 'send-reset-email', component : SendResetEmailFormComponent},
  {path : 'reset/:email/:emailToken', component : ResetPasswordFormComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
