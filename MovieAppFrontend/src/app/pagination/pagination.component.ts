import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PaginationData } from '../shared/models/paginationData';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit{

  @Input() paginationData : PaginationData | undefined = undefined;
  @Output() changedPage = new EventEmitter<number>();
  activePage = 1;

  constructor(private route : ActivatedRoute) {}
  ngOnInit(): void {
    this.route.queryParams.subscribe(params =>{
      this.activePage = params['page'] || 1;
    })
  }
  changePage(pageNr : number){
    this.changedPage.emit(pageNr);
  }

}
