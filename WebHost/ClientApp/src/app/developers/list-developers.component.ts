import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { DeveloperRepoService } from '../developer-repo.service';
import { ActivatedRoute } from '@angular/router';
import { Developer } from '../models/Developer';
import { FilterModel } from '../models/FilterModel';
import { CollectionResult } from '../collection-result';
import { AssignModel } from '../models/AssignModel';
import { AssignService } from '../assign.service';

@Component({
  selector: 'app-list-developers',
  templateUrl: './list-developers.component.html',
  styleUrls: ['./list-developers.component.css']
})
export class ListDevelopersComponent implements OnInit {

  @Input() perPage: number = 25;
  @Input() isModal: boolean = false;
  @Input() data: CollectionResult<Developer>;
  @Input() project: string = null;//this is context
  @Input() set: string = "all";
  @Input() keywords: string = null;
  @Input() isPagination: boolean = true;
  //@Output() assign: EventEmitter<AssignModel> = new EventEmitter<AssignModel>();
  developers: Developer[];
  currentPage: number;
  totalPages: number;
  totalCount: number;

  constructor(private repo: DeveloperRepoService, private router: ActivatedRoute, private assign: AssignService) {
    assign.anotherField.subscribe(this.onAssignChanged.bind(this));
  }
  get hasContext(): boolean {
    if (!this.project) return false;
    return true;
  }
  get hasParentData() {
    if (!this.data) return false;
    return true;
  }

  ngOnInit() {
    if (this.hasParentData) {
      this.totalPages = this.countPages(this.data.totalCount);
      this.totalCount = this.data.totalCount;
      this.currentPage = 1;
    }
    this.loadPage(1);
  }

  onAssignChanged(model: AssignModel) {
    if (this.hasContext) {
      this.loadPage(this.currentPage);
    }
  }

  private onLoad(result: CollectionResult<Developer>): void {
    if (result) {
      this.developers = result.values;
      this.totalPages = this.isPagination ? this.countPages(result.totalCount) : 1;
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
    this.currentPage = page;
    var filter = new FilterModel();
    filter.skip = this.perPage * (page - 1);
    filter.take = this.perPage;
    filter.sort = "fullName";

    if (this.set)
      filter.set = this.set;


    if (this.hasContext) filter.context = this.project;

    if (this.keywords) filter.keywords = this.keywords;

    if (this.hasParentData) {
      this.developers = this.data.values.slice(filter.skip, filter.skip + filter.take);
    }
    else {
      this.repo.get(filter).subscribe(this.onLoad.bind(this));
    }
  }

  public nextPage(): void {
    if (this.totalPages > this.currentPage) {
      this.loadPage(this.currentPage + 1);
    }
  }
  public previousPage(): void {
    if (this.currentPage > 1) {
      this.loadPage(this.currentPage - 1);
    }
  }
}
