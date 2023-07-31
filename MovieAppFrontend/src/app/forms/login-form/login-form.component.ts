import { Component, EventEmitter, Output } from '@angular/core';
import { Location } from '@angular/common';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { Route, Router } from '@angular/router';
import { UserStoreService } from 'src/app/shared/services/user-store.service';

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

  constructor(private authenticationService : AuthenticationService, private router : Router, private location : Location, private userStoreService : UserStoreService)
  {

  }
  login(data : any){
    this.authenticationService.loginUser(this.user).subscribe(response => {
    
      console.log(response);
      this.authenticationService.storeToken(response.body.token);

      const username = this.authenticationService.getUsernameFromToken();
      const role = this.authenticationService.getRoleFromToken();
      const id = this.authenticationService.getIdFromToken();
      this.userStoreService.setUsernameForStore(username);
      this.userStoreService.setRoleForStore(role);
      this.userStoreService.setIdForStore(id);

      //this.router.navigate(['']);
      this.location.back();
    }, error => {this.errors = error.error; console.log(error)})
  }

  cancel(){
   // this.router.navigate(['']);
   this.location.back();
  }

}
