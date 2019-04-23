import { Component, OnInit, Input } from '@angular/core';
import { CollectionResult } from '../collection-result';
import { Project } from '../models/Project';
import { ProjectRepoService } from '../project-repo.service';
import { ActivatedRoute } from '@angular/router';
import { FilterModel } from '../models/FilterModel';

@Component({
  selector: 'app-list-projects',
  templateUrl: './list-projects.component.html',
  styleUrls: ['./list-projects.component.css']
})
export class ListProjectsComponent implements OnInit {

  @Input() perPage: number = 25;
  @Input() isModal: boolean;
  //@Input() data: CollectionResult<Project>;
  @Input() developer: string;
  @Input() set: string = "all";
  @Input() isPagination: boolean = true;

  @Input() disabled: boolean = false;

  private _keywords = '';

  @Input()
  set keywords(name: string) {
    if (this._keywords != name) {
      this._keywords = name;
      this.update();
    }
  }

  get keywords(): string { return this._keywords; }



  projects: Project[];

  currentPage: number;
  totalPages: number;


  constructor(private repo: ProjectRepoService, private router: ActivatedRoute) {
  }

  get hasContext(): boolean {
    if (!this.developer) return false;
    return true;
  }

  // get hasParentData() {
  //   if (!this.data) return false;
  //   return false;
  // }
  get hasItems() {
    if (!this.projects) return false;
    if (this.projects.length == 0) return false;
    return true;
  }


  ngOnInit() {
    if (!this.perPage) this.perPage = 10;

    if (!this.isModal) this.isModal = false;
    this.currentPage = 1;
    this.update();
    // if (this.hasParentData) {
    //   this.totalPages = this.countPages(this.data.totalCount);
    //   this.parseResult(this.data);
    // } else this.update();
  }

  private onLoad(result: CollectionResult<Project>): void {
    if (result) {
      this.projects = result.values;
      this.totalPages = this.isPagination ? this.countPages(result.totalCount) : 1;
      //this.totalCount = result.totalCount;
    }
  }

  private countPages(total: number) {
    let count = Math.ceil(total / this.perPage);
    if (count - 1 == total / this.perPage) {
      return count - 1;
    }
    return count;
  }
  public update(): void {
    this.loadPage(this.currentPage);
  }
  private loadPage(page: number) {
    if (this.disabled) return;
    this.currentPage = page;
    var filter = new FilterModel();
    filter.skip = this.perPage * (page - 1);
    filter.take = this.perPage;
    filter.sort = "name";

    if (this.set) filter.set = this.set;
    if (this.hasContext) filter.context = this.developer;
    if (this.keywords) filter.keywords = this.keywords;

    this.repo.get(filter).subscribe(this.onLoad.bind(this));

    // if (this.hasParentData) {
    //   this.projects = this.data.values.slice(filter.skip, filter.skip + filter.take);
    // }
    // else {
    //   this.repo.get(filter).subscribe(this.parseResult.bind(this));
    // }
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
