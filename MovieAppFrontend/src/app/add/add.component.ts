import { Component } from '@angular/core';

export interface TrueFalse {
  value : boolean;
}

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.css']
})

export class AddComponent {
  

  addMovie: TrueFalse = {value : false};
  addActor: TrueFalse = {value : false};
  addDirector: TrueFalse = {value : false};
  constructor() {

    this.addMovie.value = false;
    this.addActor.value = false;
    this.addDirector.value = false;
  }

  toggleButton(button : TrueFalse)
  {
    button.value = !button.value;
  } 
  hasSubmitted(submitted : boolean, button : TrueFalse)
  {
    console.log(submitted);
    if(submitted)
      this.toggleButton(button);
  }
  
}
