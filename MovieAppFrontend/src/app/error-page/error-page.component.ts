import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';

@Component({
  selector: 'app-error-page',
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.css']
})
export class ErrorPageComponent implements OnInit{

 errorCode : number = 0;
 errorDescription : string = "";
 errorMessage : string = "";

 constructor(private route : ActivatedRoute) {}

 ngOnInit(): void {
    
    this.errorCode = Number(this.route.snapshot.paramMap.get("code"));
    
    switch(this.errorCode){
      case(404): 
        this.errorDescription = "Not Found";
        this.errorMessage = "The resource you are looking for was not found."
        break;
      case(403) : 
        this.errorDescription = "Unauthorized";
        this.errorMessage = "You are not authorized to access this resource."
    }
  }

}
