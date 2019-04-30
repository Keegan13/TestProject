import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-list-controls',
  templateUrl: './list-controls.component.html',
  styleUrls: ['./list-controls.component.css']
})

export class ListControlsComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  //required

  @Input() collectionSize: number;

  @Input() page: number;

  @Input() pageSize: number;

  //events

  @Output() pageChange: EventEmitter<number> = new EventEmitter();

  @Output() keywordsChange: EventEmitter<string> = new EventEmitter();

  
  //internal 

  get pageCount(): number {
    let count = this.collectionSize / this.pageSize;
    return Number.isInteger(count) ? count : Math.ceil(count);
  }

  get hasNext(): boolean {
    return this.page < this.pageCount;
  }

  get hasPrevious(): boolean {
    return this.page > 1;
  }

  get from():number {
    return (this.page - 1) * this.pageSize + 1;
  }

  get to():number {
    if (this.pageCount == this.page) {
      return this.collectionSize;
    }
    return this.page * this.pageSize;
  }

  //event handlers
  onSearch(keywords: string) {
    this.keywordsChange.emit(keywords);
  }

  onNextPage() {
    if (this.hasNext) {
      this.pageChange.emit(this.page + 1);
    }
  }

  onPreviousPage() {
    if (this.hasPrevious) {
      this.pageChange.emit(this.page - 1);
    }
  }
}
