import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { Router } from '@angular/router';
import { User } from 'src/app/shared/models/user';
import { UserSignup } from 'src/app/shared/models/userSignup';

@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent implements OnInit{

  user : UserSignup = {
    username : "",
    email : "",
    password : ""
  }
  errors : any = {};

  constructor(private authenticationService : AuthenticationService, private router : Router, private location : Location){ }

  ngOnInit(): void {
    if(this.authenticationService.getIdFromToken())
      this.router.navigate(['']);
  }

  signup(){
    this.authenticationService.signupUser(this.user).subscribe((response) => this.router.navigate(['login']), (error) => this.errors = error.error);
  }

  cancel(){
    this.location.back();
  }
}
