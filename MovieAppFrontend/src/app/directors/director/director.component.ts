import { Component, OnInit } from '@angular/core';
import { DirectorService } from 'src/app/shared/services/director.service';
import { Director } from 'src/app/shared/models/director';
import { ActivatedRoute, Router } from '@angular/router';
import { DefaultPhoto } from 'src/app/shared/functions/default-photos';
import { PaginationData } from 'src/app/shared/models/paginationData';

@Component({
  selector: 'app-director',
  templateUrl: './director.component.html',
  styleUrls: ['./director.component.css']
})
export class DirectorComponent implements OnInit{

  directors : Director[] = [];
  sortMode = "";
  dp = new DefaultPhoto();
  paginationData : PaginationData | undefined = undefined;

  constructor(private directorService : DirectorService, private route : ActivatedRoute, private router : Router) {}
  
  ngOnInit(): void { 
    this.route.queryParams.subscribe(params => {
      {
        this.sortMode = params['sort'] || "";
        this.getDirectors(params['page'] || 1, params['sort']);
      }
    })
  }
  getDirectors(pageNumber : number, sortMode : string | null)
  {
    this.directorService.getDirectorsPaged(pageNumber, sortMode).subscribe(response => {
      if(response.body)
        this.directors = response.body;
      this.paginationData = JSON.parse(response.headers.get('X-Pagination')||"");
    });
  }
  changePage(pageNumber : number)
  {
    this.router.navigate(['/directors'], {queryParams : {page : pageNumber, sort : this.sortMode}});
  }
  updateSort(sortOption : any)
  {
    this.sortMode = sortOption;
    this.router.navigate(['/directors'], {queryParams : {page : 1, sort : this.sortMode}});
  }
}
