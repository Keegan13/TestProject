import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FilterModel } from '../models/FilterModel';

@Component({
  selector: 'app-list-controls',
  templateUrl: './list-controls.component.html',
  styleUrls: ['./list-controls.component.css']
})

export class ListControlsComponent implements OnInit {

  //
  @Input() collectionSize: number;
  @Input() page: number;
  @Input() pageSize: number;
  @Output() pageChange: EventEmitter<number> = new EventEmitter();
  @Input() filter: FilterModel;//ref
  //@Output() filterChange: EventEmitter<FilterModel> = new EventEmitter();

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
  get from() {
    return (this.page - 1) * this.pageSize + 1;
  }
  get to() {
    if (this.pageCount == this.page) {
      return this.collectionSize;
    }
    return this.page * this.pageSize;
  }

  constructor() { }

  ngOnInit() {
    if (!this.filter) {
      this.filter = new FilterModel();
    }

  }

  onSearch(keywords: string) {
    this.filter.keywords = keywords;
    this.pageChange.emit(this.page);

  }
  nextPage() {
    if (this.hasNext) {
      this.filter.skip = this.page * this.pageSize;
      this.filter.take = this.pageSize;
      this.pageChange.emit(this.page + 1);
      console.log(JSON.stringify(this.filter))
    }
  }
  previousPage() {
    if (this.hasPrevious) {
      this.filter.skip = (this.page - 2) * this.page;
      this.filter.take = this.pageSize;
      this.pageChange.emit(this.page - 1);
      console.log(JSON.stringify(this.filter))
    }
  }
}
