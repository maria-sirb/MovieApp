import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../shared/services/authentication.service';
import { User } from '../shared/models/user';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit{
  
  user : User =  {
    userId: 0,
    username: ''
  }
  userId =  Number(this.route.snapshot.paramMap.get('userId'));
  constructor(private authService : AuthenticationService, private route : ActivatedRoute) {}

  ngOnInit(): void {
    console.log(this.userId);
    this.authService.getUserById(this.userId).subscribe(user => {this.user = user; console.log(user)}, error => console.log(error));
  }



}
