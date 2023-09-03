import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { UserStoreService } from '../services/user-store.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authenticationService : AuthenticationService, private userStoreService : UserStoreService, private router: Router){

  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    if(this.authenticationService.isLoggedIn())
    {  
      this.userStoreService.getRoleFromStore().subscribe(res => {
        const userRole = res || this.authenticationService.getRoleFromToken(); 
        const { roles }  = route.data;
        if(roles && !roles.includes(userRole))
        {
          this.router.navigate(['']);
          return false;
        }
        return true;
    })
    return true;
  }
    else
    { 
      this.router.navigate(['login']);
      return false;
    }
  }
  
}
