import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { DeveloperRepoService } from '../services/developer-repo.service';
import { ActivatedRoute } from '@angular/router';
import { Developer } from '../models/Developer';
import { FilterModel } from '../models/FilterModel';
import { CollectionResult } from '../models/collection-result';
import { AssignModel } from '../models/AssignModel';
import { AssignService } from '../Services/assign.service';

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
    if (this.hasContext) this.filter.context = this.projectContextUrl;
    
    this.loadData();
  }
  //required 


  //optional

  _projectContextUrl = "";

  @Input() set projectContextUrl(val: string) {
    this._projectContextUrl = val;
    this.filter.context = val;
    this.loadData();
  }

  get projectContextUrl(): string {
    return this._projectContextUrl;
  }

  @Input() pageSize: number = 25;



  @Input() set: string = "all";

  @Input() noPanel: boolean = false;

  //internal
  filter: FilterModel = new FilterModel();

  private _pageDelta: number = 0;

  private developers: Developer[];

  private page: number = 1;

  private collectionSize: number = 0;

  //properties

  get hasContext(): boolean {
    if (this.projectContextUrl) {

      return true;
    }

    return false;
  }

  get hasItems(): boolean {

    return this.developers && this.developers.length > 0
  }

  //event Handlers
  onPageChange($page: number) {
    //current page is updated only if request was successful
    if ($page > this.page) {
      this._pageDelta = 1;
    }
    if ($page < this.page) {
      this._pageDelta = -1;
    }
    this.loadData();
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
    this.repo.get(this.filter).subscribe(this.onDataLoaded.bind(this));
  }
}
