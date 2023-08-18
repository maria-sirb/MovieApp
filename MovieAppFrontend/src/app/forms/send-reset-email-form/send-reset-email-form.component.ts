import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-send-reset-email-form',
  templateUrl: './send-reset-email-form.component.html',
  styleUrls: ['./send-reset-email-form.component.css']
})
export class SendResetEmailFormComponent {

  errors = "";
  emailSent = false;
  constructor(private authService : AuthenticationService, private location : Location, private route : ActivatedRoute) {
  }

  submit(data : any){
    this.authService.sendResetEmail(data.email).subscribe(res => this.emailSent = true, err => this.errors = err.error);
  }
  cancel(){
    this.location.back();
   }
}
