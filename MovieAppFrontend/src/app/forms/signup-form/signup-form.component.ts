import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent {

  user = {
    userId: 0,
    username : "",
    email : "",
    password : "",
    role : "",
    token : ""
  }
  errors : any = {};

  constructor(private authenticationService : AuthenticationService, private router : Router, private location : Location)
  {

  }

  signup(data : any){
    this.authenticationService.signupUser(this.user).subscribe((response) => this.router.navigate(['login']), (error) => this.errors = error.error);
  }

  cancel(){
    //this.router.navigate(['']);
    this.location.back();
  }
}
