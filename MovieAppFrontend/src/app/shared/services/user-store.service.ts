import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserStoreService {

  private username$ = new BehaviorSubject<string>("");
  private role$ = new BehaviorSubject<string>("");
  private id$ = new BehaviorSubject<string>("");
  constructor() { }

  public getRoleFromStore(){
    return this.role$.asObservable();
  }

  public setRoleForStore(role : string){
    this.role$.next(role);
  }

  public getUsernameFromStore(){
    return this.username$.asObservable();
  }

  public setUsernameForStore(username : string){
    this.username$.next(username);
  }

  public getIdFromStore(){
    return this.id$.asObservable();
  }

  public setIdForStore(id : string){
    this.id$.next(id);
  }
}
