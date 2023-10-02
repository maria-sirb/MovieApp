import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { Router } from '@angular/router';
import { User } from 'src/app/shared/models/user';

@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent implements OnInit{

  user : User = {
    userId: 0,
    username : "",
    email : "",
    password : "",
    role : "",
    token : "",
    imageName : "",
    imageSource : ""
  }
  errors : any = {};

  constructor(private authenticationService : AuthenticationService, private router : Router, private location : Location){ }

  ngOnInit(): void {
    if(this.authenticationService.getIdFromToken())
      this.router.navigate(['']);
  }

  signup(data : any){
    this.authenticationService.signupUser(this.user).subscribe((response) => this.router.navigate(['login']), (error) => this.errors = error.error);
  }

  cancel(){
    this.location.back();
  }
}
