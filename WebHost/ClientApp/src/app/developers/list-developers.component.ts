import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { DeveloperRepoService } from '../services/developer-repo.service';
import { Developer } from '../models/Developer';
import { CollectionResult } from '../models/collection-result';
import { AssignModel } from '../models/AssignModel';
import { AssignService } from '../Services/assign.service';
import { DeveloperFilterModel } from '../models/DeveloperFilterModel';

@Component({
  selector: 'app-list-developers',
  templateUrl: './list-developers.component.html',
  styleUrls: ['./list-developers.component.css']
})

export class ListDevelopersComponent implements OnInit {

  constructor(
    private repo: DeveloperRepoService,
    private assign: AssignService) {
    assign.anotherField.subscribe(this.onAssignChanged.bind(this));
  }

  ngOnInit() {
    //init filter
    this.filter.sort = "fullName";//sort should handle
    this.filter.order = "ascending"; // sort
    this.filter.set = this.set; // parent
    this.filter.take = this.pageSize; // list-panel
    this.filter.skip = 0; //list-panel
    this.filter.keywords = ""; //list-panel

    if (this.hasContext) this.filter.projectContextUrl = this.projectContextUrl;
    this._initialized = true;
    this.loadData();
  }
  //required 


  //optional


  _projectContextUrl = "";

  @Input() set projectContextUrl(val: string) {
    this._projectContextUrl = val;
    if (this._initialized) {
      this.filter.projectContextUrl = val;
      this.loadData();
    }
  }

  get projectContextUrl(): string {
    return this._projectContextUrl;
  }

  @Input() pageSize: number = 25;

  @Input() set: string = "all";

  @Input() noPanel: boolean = false;

  //internal

  private _initialized: boolean = false;

  private filter: DeveloperFilterModel = new DeveloperFilterModel();

  private _pageDelta: number = 0;

  private developers: Developer[];

  private page: number = 1;

  private collectionSize: number = 0;

  get hasContext(): boolean {
    if (this.projectContextUrl) {

      return true;
    }

    return false;
  }

  get hasItems(): boolean {

    return this.developers && this.developers.length > 0
  }

  //event handlers

  onPageChange($page: number) {
    //current page is updated only if request was successful
    this._pageDelta = $page - this.page;

    this.filter.skip = ($page - 1) * this.pageSize;

    this.loadData();
  }

  onKeywordsChange($keywords: string) {
    this.filter.keywords = $keywords;
    this.loadData();
  }

  onSortChange() {
    //not implemented
  }

  onAssignChanged(model: AssignModel) {
    //temporary
    if (this.hasContext) {
      this.loadData();
    }
  }

  onDataLoaded(result: CollectionResult<Developer>): void {
    if (result.values.length == 0) {
      let lastPage = result.totalCount / this.pageSize;
      this.page = Number.isInteger(lastPage) ? lastPage : Math.ceil(lastPage);//1
      if (result.totalCount > 0) {
        this.filter.skip = (this.page - 1) * this.pageSize;
        this.loadData();
      }
    }

    this.developers = result.values;
    this.collectionSize = result.totalCount;
    this.page += this._pageDelta;
    this._pageDelta = 0;
  }

  //methods 

  loadData() {
    console.log("loading developers with filter: " + JSON.stringify(this.filter));
    this.repo.get(this.filter).subscribe(this.onDataLoaded.bind(this));
  }
}
