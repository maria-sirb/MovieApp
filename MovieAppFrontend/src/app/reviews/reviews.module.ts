import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReviewsComponent } from './reviews/reviews.component';
import { RouterModule } from '@angular/router';
import { VoteComponent } from './vote/vote.component';




@NgModule({
  declarations: [
    ReviewsComponent,
    VoteComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
   ReviewsComponent
  ]
})
export class ReviewsModule { }
