import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ResetPassword } from 'src/app/shared/models/resetPassword';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-reset-password-form',
  templateUrl: './reset-password-form.component.html',
  styleUrls: ['./reset-password-form.component.css']
})
export class ResetPasswordFormComponent implements OnInit{

  resetPassword : ResetPassword = {
    email : "",
    emailToken : "",
    newPassword : ""
  }
  errors = "";

  constructor(private authService : AuthenticationService, private router : Router, private route : ActivatedRoute) {
  }

  ngOnInit(): void {
    if(this.authService.getIdFromToken())
      this.router.navigate(['']);
  
    this.resetPassword.email = this.route.snapshot.paramMap.get("email") || "";
    this.resetPassword.emailToken = this.route.snapshot.paramMap.get("emailToken") || "";
  }

  reset(){
    this.authService.resetPasswrod(this.resetPassword).subscribe(res => this.router.navigate(['login']), err => {this.errors = err.error;})
  }
  
  cancel(){
    this.router.navigate(['']);
   }
}
