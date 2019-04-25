import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { DeveloperRepoService } from '../developer-repo.service';
import { ActivatedRoute } from '@angular/router';
import { Developer } from '../models/Developer';
import { FilterModel } from '../models/FilterModel';
import { CollectionResult } from '../collection-result';
import { AssignModel } from '../models/AssignModel';
import { AssignService } from '../assign.service';
import { hasAlignedHourOffset } from 'ngx-bootstrap/chronos/units/offset';

@Component({
  selector: 'app-list-developers',
  templateUrl: './list-developers.component.html',
  styleUrls: ['./list-developers.component.css']
})
export class ListDevelopersComponent implements OnInit {
  //do not refresh page until it loads
  private _pageDelta: number = 0;

  @Input() pageSize: number = 25;
  @Input() isModal: boolean = false;
  
  //@Input() data: CollectionResult<Developer>;
  @Input() project: string = null;//this is context
  @Input() set: string = "all";
  @Input() noPanel: boolean = false;
  filter: FilterModel = new FilterModel();
  developers: Developer[];
  page: number = 1;
  collectionSize: number = 0;


  onPageChange($page: number) {
    if ($page > this.page) {
      this._pageDelta = 1;
    }
    if ($page < this.page) {
      this._pageDelta = -1;
    }

    this.loadData();
  }


  constructor(private repo: DeveloperRepoService, private router: ActivatedRoute, private assign: AssignService) {
    assign.anotherField.subscribe(this.onAssignChanged.bind(this));
  }
  get hasContext(): boolean {
    if (!this.project) return false;
    return true;
  }
  get hasItems(): boolean { return this.developers && this.developers.length > 0 }
  ngOnInit() {
    //default
    if (!this.pageSize) this.pageSize = 25;
    if (!this.set) this.set = "all";
    this.page = 1;
    //init filter
    if (this.hasContext) this.filter.context = this.project;
    this.filter.sort = "fullName";//sort should handle
    this.filter.order = "ascending"; // sort
    this.filter.set = this.set; // parent
    this.filter.take = this.pageSize; // list-panel
    this.filter.skip = 0; //list-panel
    this.filter.keywords = ""; //list-panel
    this.loadData();
  }

  onAssignChanged(model: AssignModel) {
    if (this.hasContext) {
      this.loadData();
    }
  }

  public update(): void {
    this.loadData();
  }

  private onLoad(result: CollectionResult<Developer>): void {
    this.developers = result.values;
    if (result) {
      //if data received
      this.collectionSize = result.totalCount;

      // do nothigs
      if (this.collectionSize == 0) {
        this._pageDelta = 0;
        this.page = 1;
        //this.loadData();
        return;
      }

      if (result.values.length == 0) {
        let lastPage = result.totalCount / this.pageSize;

        this.page = Number.isInteger(lastPage) ? lastPage : Math.ceil(lastPage);

        this.filter.skip = (this.page - 1) * this.pageSize;
        this._pageDelta = 0;
        this.loadData();
      }

      this.page += this._pageDelta;
      this._pageDelta = 0;
      this.developers = result.values;
    }
    else {
      //if no data
      console.log("handle this later TODO");
    }
    this._pageDelta = 0;
  }

  private loadData() {
    this.repo.get(this.filter).subscribe(this.onLoad.bind(this));
  }
}
