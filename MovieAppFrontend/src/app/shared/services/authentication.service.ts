import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private baseUrl : string =  'https://localhost:7172/api/User';
  private userPayload;

  constructor(private client : HttpClient, private jwtHelper : JwtHelperService) { 
    this.userPayload = this.decodeToken();
  }
  
  getUser(username : string) : Observable<any>{
    return this.client.get<any>(this.baseUrl + '/username/' + username);
  }

  getUserById(userId : number) : Observable<any>{
    return this.client.get<any>(this.baseUrl + "/id/" + userId);
  }

  signupUser(user : any) : Observable<any>{
    return this.client.post<any>(this.baseUrl + '/register', user, {observe : 'response'});
  }

  loginUser(user : any) : Observable<any>{
    return this.client.post<any>(this.baseUrl + '/authenticate', user, {observe : 'response'});
  }

  editUser(user : User){
    const formData = new FormData();
    Object.entries(user).forEach(([key, value]) => {
      formData.append(key, value);
    });
    return this.client.put<any>(this.baseUrl + "/" + user.userId, formData, {observe : 'response'});
  }

  deleteUser(userId : number) : Observable<any>{
    return this.client.delete<any>(this.baseUrl + "/" + userId);
  }
  
  storeToken(tokenValue : string){
    localStorage.setItem('token', tokenValue);
    this.userPayload = this.decodeToken();
  }

  getToken(){
    return localStorage.getItem('token');
  }
  
  isLoggedIn() : boolean{
    return !!this.getToken();
  }

  logout(){
    localStorage.removeItem('token');
    this.userPayload = null;
  }

  isTokenExpired() : boolean{
    const token = this.getToken()??"";  
    return this.jwtHelper.isTokenExpired(token);
  }

  decodeToken(){
    const token = this.getToken()??"";  
    return this.jwtHelper.decodeToken(token);
  }

  getUsernameFromToken(){
    if(this.userPayload)
      return this.userPayload.unique_name;
  }

  getRoleFromToken(){
    if(this.userPayload)
      return this.userPayload.role;
  }
  getIdFromToken(){
    if(this.userPayload)
      return this.userPayload.nameid;
  }
}
