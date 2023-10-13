import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  userId : number = 0;

  constructor(private authenticationService : AuthenticationService, private userStoreService : UserStoreService, private route : ActivatedRoute, private router : Router){
  
  }

  ngOnInit() : void {

    if(this.authenticationService.isTokenExpired())
      this.logout();
    //subscribe to the userStore so when the store username/role values change, the username and role properties will get updated without refreshing the page
    this.userStoreService.getRoleFromStore().subscribe(res => {
      this.role = res || this.authenticationService.getRoleFromToken(); //in case of page refresh, the userStore observable will be empty, so we get the username/role/id properties values from the token in local storage
    })
    this.userStoreService.getUsernameFromStore().subscribe(res => {
      this.username = res || this.authenticationService.getUsernameFromToken();
    })
    this.userStoreService.getIdFromStore().subscribe(res => {
      this.userId = Number(res) || Number(this.authenticationService.getIdFromToken());
    })

  }

  logout(){
    this.authenticationService.logout();
    this.userStoreService.setRoleForStore("");
    this.userStoreService.setUsernameForStore("");
    this.userStoreService.setIdForStore("");
    if(this.router.url.split("/").includes('user') || this.router.url.split("/").includes('admin'))
    { 
        this.router.navigate(['/']);
    }

  }

  deleteAccount(){
    this.authenticationService.deleteUser(this.userId).subscribe(response => this.logout);
  }

}
