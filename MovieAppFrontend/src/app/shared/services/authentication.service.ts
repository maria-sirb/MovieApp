import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  baseUrl : string =  'https://localhost:7172/api/User';
  constructor(private client : HttpClient) { }

  signupUser(user : any) : Observable<any>{
    return this.client.post<any>(this.baseUrl + '/register', user, {observe : 'response'});
  }

  loginUser(user : any) : Observable<any>{
    return this.client.post<any>(this.baseUrl + '/authenticate', user, {observe : 'response'});
  }
  
  storeToken(tokenValue : string){
    localStorage.setItem('token', tokenValue);
  }

  getToken(){
    return localStorage.getItem('token');
  }
  
  isLoggedIn() : boolean{
    return !!this.getToken();
  }

  logout(){
    localStorage.removeItem('token');
  }
}
