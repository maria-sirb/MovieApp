import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { Renderer2 } from '@angular/core';
import { AuthenticationService } from '../shared/services/authentication.service';
import { UserStoreService } from '../shared/services/user-store.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent {

  username : string= "";
  role : string = "";

  constructor(private authenticationService : AuthenticationService, private userStoreService : UserStoreService){
  
  }

  ngOnInit() : void {
    //subscribe to the userStore so when the store username/role values change, the username and role properties will get updated without refreshing the page
    this.userStoreService.getUsernameFromStore().subscribe(res => {
      //in case of page refresh, the userStore observable will be empty, so we get the username/role properties values from the token in local storage
      this.username = res || this.authenticationService.getUsernameFromToken();
    })

    this.userStoreService.getRoleFromStore().subscribe(res => {
      this.role = res || this.authenticationService.getRoleFromToken();
    })
  }

  logout(){
    this.authenticationService.logout();
    this.userStoreService.setRoleForStore("");
    this.userStoreService.setUsernameForStore("");
  }

}
