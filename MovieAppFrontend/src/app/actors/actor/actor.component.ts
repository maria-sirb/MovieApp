import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginationComponent } from 'src/app/pagination/pagination.component';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';
import { Actor } from 'src/app/shared/models/actor';
import { PaginationData } from 'src/app/shared/models/paginationData';
import { ActorsService } from 'src/app/shared/services/actors.service';


@Component({
  selector: 'app-actor',
  templateUrl: './actor.component.html',
  styleUrls: ['./actor.component.css']
})
export class ActorComponent implements OnInit{

  actors : Actor[] = [];
  sortMode = "";
  dp = new DefaultPhoto();
  paginationData : PaginationData | undefined = undefined;
  constructor( private actorsService : ActorsService, private route : ActivatedRoute, private router : Router){}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params =>
      {
        this.sortMode = params['sort'] || "";
        this.getActors(params['page'] || 1, params['sort']);
      }
      )
  }

  getActors(pageNumber : number, sortMode : string | null)
  {
    this.actorsService.getActorsPaged(pageNumber, sortMode).subscribe(response => {
      if(response.body)
        this.actors = response.body;
      this.paginationData = JSON.parse(response.headers.get('X-Pagination')||"");
    });
  }

  changePage(pageNumber : number){
    this.router.navigate(['/actors'], {queryParams : {page : pageNumber, sort : this.sortMode}});
  }

  updateSort(sortOption : any)
  {
    this.sortMode = sortOption;
    this.router.navigate(['/actors'], {queryParams : {page : 1, sort : this.sortMode}});
  }
}
