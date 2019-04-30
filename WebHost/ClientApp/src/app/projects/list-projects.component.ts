import { Component, OnInit, Input } from '@angular/core';
import { CollectionResult } from '../models/collection-result';
import { Project } from '../models/Project';
import { ProjectRepoService } from '../services/project-repo.service';
import { ActivatedRoute } from '@angular/router';
import { ProjectFilterModel } from '../models/ProjectFilterModel';
import { timingSafeEqual } from 'crypto';

@Component({
  selector: 'app-list-projects',
  templateUrl: './list-projects.component.html',
  styleUrls: ['./list-projects.component.css']
})
export class ListProjectsComponent implements OnInit {
  constructor(private repo: ProjectRepoService) {
  }

  ngOnInit() {
    this.filter = new ProjectFilterModel();
    this.filter.sort = "name";//sort should handle
    this.filter.order = "ascending"; // sort
    this.filter.set = this.set; // parent
    this.filter.take = this.pageSize; // list-panel
    this.filter.skip = 0; //list-panel
    this.filter.keywords = ""; //list-panel}

    if (this.hasContext) this.filter.developerContextUrl = this.developerContextUrl;

    this._initialized=true;

    this.loadData();
  }

  @Input() pageSize: number = 25;

  @Input() set: string = "all";

  @Input() noPanel: boolean = false;

  @Input() developerContextUrl: string = null;

  @Input() filter: ProjectFilterModel;// = new FilterModel();

  private _pageDelta: number = 0;
  projects: Project[];
  page: number = 1;
  collectionSize: number = 0;
  private _initialized = false;

  onPageChange($page: number) {
    this._pageDelta=$page-this.page;

    this.filter.skip=($page-1)*this.pageSize;

    this.loadData();
  }

  onKeywordsChange($keywords: string) {
    this.filter.keywords = $keywords;
    this.loadData();
  }

  get hasContext(): boolean {
    if (!this.developerContextUrl) return false;
    return true;
  }

  get hasItems(): boolean {
    return this.projects && this.projects.length > 0;
  }



  private onDataLoad(result: CollectionResult<Project>): void {

    if (result.values.length == 0) {
      let lastPage = result.totalCount / this.pageSize;
      this.page = Number.isInteger(lastPage) ? lastPage : Math.ceil(lastPage);//1
      if (result.totalCount > 0) {
        this.filter.skip = (this.page - 1) * this.pageSize;
        this.loadData();
      }
    }

    this.projects = result.values;
    this.collectionSize = result.totalCount;
    this.page += this._pageDelta;
    this._pageDelta = 0;
  }

  private loadData() {
    console.log("loading projects with filter:" + JSON.stringify(this.filter));
    this.repo.get(this.filter).subscribe(this.onDataLoad.bind(this));
  }
}
