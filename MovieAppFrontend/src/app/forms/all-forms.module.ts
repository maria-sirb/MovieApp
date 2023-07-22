import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActorFormComponent } from './actor-form/actor-form.component';
import { MovieFormComponent } from './movie-form/movie-form.component';
import { DirectorFormComponent } from './director-form/director-form.component';
import { RoleFormComponent } from './role-form/role-form.component';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from '../app-routing.module';
import { SignupFormComponent } from './signup-form/signup-form.component';
import { LoginFormComponent } from './login-form/login-form.component';
import { ReviewFormComponent } from './review-form/review-form.component';


@NgModule({
  declarations: [
    ActorFormComponent,
    MovieFormComponent,
    DirectorFormComponent,
   RoleFormComponent,
   SignupFormComponent,
   LoginFormComponent,
   ReviewFormComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    AppRoutingModule
  ],
  exports : [
    ActorFormComponent,
    MovieFormComponent,
    DirectorFormComponent,
   RoleFormComponent
  ]
})
export class AllFormsModule { }
