import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {

  @Input() totalCount: number;
  @Input() pageSize: number;
  @Output() pageChanged: EventEmitter<number> =  new EventEmitter<number>();

  constructor() { }

  ngOnInit(): void {
  }

   onPageChange(event: any): void {
      this.pageChanged.emit(event.page);
   }
}
