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
  private _pageDelta: number=0;

  @Input() pageSize: number = 25;
  @Input() isModal: boolean=false;

  @Input() developer: string=null;
  @Input() set: string = "all";
  @Input() noPanel: boolean = false;

  filter: FilterModel = new FilterModel();
  projects: Project[];
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


  constructor(private repo: ProjectRepoService){
  }

  get hasContext(): boolean {
    if (!this.developer) return false;
    return true;
  }

  get hasItems() {
    if (!this.projects) return false;
    if (this.projects.length == 0) return false;
    return true;
  }


  ngOnInit() {
    if (!this.isModal) this.isModal = false;
    if (!this.pageSize) this.pageSize = 25;
    if (!this.set) this.set = "all";
    this.page = 1;
    //init filter
    if (this.hasContext) this.filter.context = this.developer;
    this.filter.sort = "name";//sort should handle
    this.filter.order = "ascending"; // sort
    this.filter.set = this.set; // parent
    this.filter.take = this.pageSize; // list-panel
    this.filter.skip = 0; //list-panel
    this.filter.keywords = ""; //list-panel
    this.loadData();
  }

  private onLoad(result: CollectionResult<Project>): void {
    this.projects = result.values;
    if (result) {

      this.collectionSize = result.totalCount;

      if (this.collectionSize == 0) {
        this._pageDelta = 0;
        this.page = 1;
        return;
      }
      if(result.values.length==0)
      {
        let lastPage = result.totalCount / this.pageSize;

        this.page = Number.isInteger(lastPage) ? lastPage : Math.ceil(lastPage);

        this.filter.skip = (this.page - 1) * this.pageSize;
        this._pageDelta = 0;
        this.loadData();
      }
      this.page+=this._pageDelta;
      this._pageDelta=0;
      this.projects=result.values;

    }
    else
    {
            //if no data
            console.log("handle this later TODO");
    }
    this._pageDelta = 0;
  }

  public update(): void {
    this.loadData();
  }
  private loadData() {
    this.repo.get(this.filter).subscribe(this.onLoad.bind(this));
  }
}
