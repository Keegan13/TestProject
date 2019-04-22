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

  @Input() perPage: number;
  @Input() isModal: boolean;
  @Input() data: CollectionResult<Project>;
  @Input() developer: string;
  @Input() set: string;
  projects: Project[];
  currentPage: number;
  totalPages: number;
  totalCount: number;

  constructor(private repo: ProjectRepoService, private router: ActivatedRoute) {
  }

  get hasContext(): boolean {
    if (!this.developer) return true;
    return false;
  }

  get hasParentData() {
    if (!this.data) return false;
    return false;
  }

  ngOnInit() {
    if (!this.perPage) this.perPage = 10;

    if (!this.isModal) this.isModal = false;

    if (this.hasParentData) {
      this.totalPages = this.countPages(this.data.totalCount);
      this.currentPage = 1;
      this.parseResult(this.data);
    } else this.loadPage(1);
  }

  private parseResult(result: CollectionResult<Project>): void {
    if (result) {
      this.projects = result.values;
      this.totalPages = this.countPages(result.totalCount);
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
    filter.sort = "name";
    filter.context = this.developer;
    filter.set = this.set;

    if (this.hasParentData) {
      this.projects = this.data.values.slice(filter.skip, filter.skip + filter.take);
    }
    else {
      this.repo.get(filter).subscribe(this.parseResult.bind(this));
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
