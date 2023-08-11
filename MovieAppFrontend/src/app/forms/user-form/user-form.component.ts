import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';
import { User } from 'src/app/shared/models/user';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { UserStoreService } from 'src/app/shared/services/user-store.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css']
})
export class UserFormComponent implements OnInit{

  imageSrc : string | null | ArrayBuffer = "";
  imageFile : File | undefined = undefined;
  user : User = {
    userId: 0,
    username : "",
    email : "",
    password : "",
    role : "",
    token : "",
    imageName : "",
    imageSource : "",
    imageFile : undefined
  }
  errors : any = {};
  
  constructor(private authService : AuthenticationService, private userStoreService : UserStoreService, private location : Location, private route : ActivatedRoute) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(paramMap => {
      var userId = Number(paramMap.get('userId'));
      this.authService.getUserById(userId).subscribe(user => this.user = user, error => console.log(error));
    });
  }

  readFile(event : Event)
  {
    const target = event.target as HTMLInputElement;
    if(target.files && target.files[0])
    {
      this.imageFile = target.files[0];
      const file = target.files[0];
      const reader = new FileReader();
      reader.onload = e => this.imageSrc = reader.result;
      reader.readAsDataURL(file);
    }
  }

  deleteProfileImage(){
    this.user.imageSource = "";
    this.imageSrc  = "";
    this.imageFile = undefined;
  }

  updateUser(){
    this.user.imageFile = this.imageFile;
    console.log(this.user);
    this.authService.editUser(this.user).subscribe(response => {
      console.log(response);
      this.authService.storeToken(response.body.token);

      const username = this.authService.getUsernameFromToken();
      const role = this.authService.getRoleFromToken();
      const id = this.authService.getIdFromToken();
      this.userStoreService.setUsernameForStore(username);
      this.userStoreService.setRoleForStore(role);
      this.userStoreService.setIdForStore(id);
      this.location.back();
    }, err => {
      console.log(err);
      this.errors = err.error;
    });
  }

  cancel()
  {
    this.location.back();
  }
}

