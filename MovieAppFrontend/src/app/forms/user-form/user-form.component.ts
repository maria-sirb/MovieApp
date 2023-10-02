import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';
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
  currentUserId = 0;
  dp = new DefaultPhoto();
  
  constructor(private authService : AuthenticationService, private userStoreService : UserStoreService, private location : Location, private route : ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(paramMap => {
      var userId = Number(paramMap.get('userId'));
      //is user logs out redirect to home page + users can't edit other users profiles
      this.userStoreService.getIdFromStore().subscribe(id => 
      {
          this.currentUserId = Number(id) || this.authService.getIdFromToken() || 0;
          if(this.currentUserId == 0 || this.currentUserId != userId)
            this.router.navigate(['']);
      }
      );
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
    this.authService.editUser(this.user).subscribe(response => {
      this.authService.storeToken(response.body.token);

      const username = this.authService.getUsernameFromToken();
      const role = this.authService.getRoleFromToken();
      const id = this.authService.getIdFromToken();
      this.userStoreService.setUsernameForStore(username);
      this.userStoreService.setRoleForStore(role);
      this.userStoreService.setIdForStore(id);
      this.location.back();
    }, err => {
      this.errors = err.error;
    });
  }

  cancel()
  {
    this.location.back();
  }
}

