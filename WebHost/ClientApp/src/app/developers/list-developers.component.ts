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

  @Input() perPage: number = 25;
  @Input() isModal: boolean = false;
  //@Input() data: CollectionResult<Developer>;
  @Input() project: string = null;//this is context
  @Input() set: string = "all";
  @Input() isPagination: boolean = true;


  private _keywords = '';

  @Input()
  set keywords(name: string) {
    if (this._keywords != name) {
      this._keywords = name;
      this.update();
    }
  }

  get keywords(): string { return this._keywords; }
  //@Output() assign: EventEmitter<AssignModel> = new EventEmitter<AssignModel>();
  @Input() disabled: boolean = false;
  developers: Developer[];

  currentPage: number=1;
  totalPages: number=1;
  totalCount: number=0;

  constructor(private repo: DeveloperRepoService, private router: ActivatedRoute, private assign: AssignService) {
    assign.anotherField.subscribe(this.onAssignChanged.bind(this));
  }
  get hasContext(): boolean {
    if (!this.project) return false;
    return true;
  }

  get hasItems() {
    if (!this.developers) return false;
    if (this.developers.length == 0) return false;
    return true;
  }


  ngOnInit() {
    if (!this.perPage) this.perPage = 25;
    // if (this.hasParentData) {
    //   this.totalPages = this.countPages(this.data.totalCount);
    //   this.totalCount = this.data.totalCount;
    //   this.currentPage = 1;
    // }
    if (!this.isModal) this.isModal = false;
    this.currentPage = 1;
    this.update();
    //this.loadPage(1);
  }

  onAssignChanged(model: AssignModel) {
    if (this.hasContext) {
      this.loadPage(this.currentPage);
    }
  }

  public update(): void {
    this.loadPage(this.currentPage);
  }

  private onLoad(result: CollectionResult<Developer>): void {
    if (result) {

      this.developers = result.values;

      this.totalPages = this.isPagination ? this.countPages(result.totalCount)
        : 1;
      if (this.currentPage > this.totalPages) {
        this.currentPage--;
        this.update();

      }
      this.totalCount = result.totalCount;
    }
  }

  private countPages(total: number) {
    let count = Math.ceil(total / this.perPage);
    if (count - 1 == total / this.perPage) {
      return count - 1;
    }
    return count;
  }

  private loadPage(page: number) {
    if (this.disabled) return;
    this.currentPage = page;

    var filter = new FilterModel();
    filter.skip = this.perPage * (page - 1);
    filter.take = this.perPage;
    filter.sort = "fullName";

    if (this.set) filter.set = this.set;
    if (this.hasContext) filter.context = this.project;
    if (this.keywords) filter.keywords = this.keywords;

    this.repo.get(filter).subscribe(this.onLoad.bind(this));
    // if (this.hasParentData) {
    //   this.developers = this.data.values.slice(filter.skip, filter.skip + filter.take);
    // }
    // else {
    //   this.repo.get(filter).subscribe(this.onLoad.bind(this));
    // }
  }


  public get from() {
    if(this.totalPages=1)
    {
      return this.totalCount;
    }

    return (this.currentPage-1) * this.perPage + 1;
  }
  public get to() {
    if (this.hasNext) {
      return (this.currentPage) * this.perPage;
    }
    return this.totalCount;
  }

  public nextPage(): void {
    if (this.hasNext)
      this.loadPage(this.currentPage + 1);
  }
  public get hasPrevious() {
    return this.currentPage > 1;
  }
  public get hasNext() {
    return this.currentPage < this.totalPages;
  }

  public previousPage(): void {
    if (this.hasPrevious) {
      this.loadPage(this.currentPage - 1);
    }
  }
}
