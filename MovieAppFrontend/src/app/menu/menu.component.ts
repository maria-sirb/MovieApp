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
    //subscribe to the userStore so when the store username/role values change, the username and role properties will get updated without refreshing the page
    this.userStoreService.getUsernameFromStore().subscribe(res => {
      //in case of page refresh, the userStore observable will be empty, so we get the username/role properties values from the token in local storage
      this.username = res || this.authenticationService.getUsernameFromToken();
      if(this.username)
        this.authenticationService.getUser(this.username).subscribe(user => {this.userId = user.userId; console.log(user)}, error => console.log(error));
    })

    this.userStoreService.getRoleFromStore().subscribe(res => {
      this.role = res || this.authenticationService.getRoleFromToken();
    })

  }

  logout(){
    this.authenticationService.logout();
    this.userStoreService.setRoleForStore("");
    this.userStoreService.setUsernameForStore("");
    /*if(this.router.url.split("/").includes('user') || this.router.url.split("/").includes('admin'))
     { 
        this.router.navigate(['/']);
    }*/

  }

}
