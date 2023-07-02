import { Component, OnChanges, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Renderer2 } from '@angular/core';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent {

  activeUrl : string = "";
  constructor(private router : Router, private renderer : Renderer2){

  }

  onClick(event : Event): void {
    //this.activeUrl = this.router.url;
    console.log(event.target);
    console.log(this.activeUrl);
   
    
   
   
  }

}
