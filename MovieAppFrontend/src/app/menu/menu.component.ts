import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { Renderer2 } from '@angular/core';
import { AuthenticationService } from '../shared/services/authentication.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent {

  activeUrl : string = "";
  isLoggedIn : boolean = false;
  constructor(private authenticationService : AuthenticationService, private router : Router, private renderer : Renderer2){
    
  }

  ngOnInit() : void {
    this.isLoggedIn = this.authenticationService.isLoggedIn();
  }

  onClick(event : Event): void {
    //this.activeUrl = this.router.url;
    console.log(event.target);
    console.log(this.activeUrl);
  }

  logout(){
    this.authenticationService.logout();
    this.router.navigate(['']).then(() => {
      window.location.reload();
    })
  }

}
