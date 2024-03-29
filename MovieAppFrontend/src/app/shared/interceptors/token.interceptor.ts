import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, tap, throwError } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private authenticationService : AuthenticationService, private router : Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> { 
    const token = this.authenticationService.getToken();
    if(token){
      request = request.clone({
      setHeaders : {Authorization : `Bearer ${token}`}
    })
    }
  
    return next.handle(request).pipe(tap(() => {},
      (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status == 401) {
          this.router.navigate(['login']);
        }
        else if (err.status == 403) {
          this.router.navigate(['/403']);
        }
        else 
          return;
      }
    }));
  }
}
