import { Component, EventEmitter, Output } from '@angular/core';
import { Location } from '@angular/common';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent {

  user = {
    userId: 0,
    username : "",
    email : "",
    password : "",
    role : "",
    token : ""
  }
  errors = "";

  constructor(private authenticationService : AuthenticationService, private router : Router)
  {

  }
  login(data : any){
    this.authenticationService.loginUser(this.user).subscribe(response => {
      this.authenticationService.storeToken(response.body.token);
      this.router.navigate(['']).then(() => {
        window.location.reload();
      });
    }, error => this.errors = error.error)
  }

  cancel(){
    this.router.navigate(['']);
  }
}
