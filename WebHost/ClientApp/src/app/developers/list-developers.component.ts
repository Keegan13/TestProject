import { Component, OnInit, Input } from '@angular/core';
import { DeveloperRepoService } from '../developer-repo.service';
import { ActivatedRoute } from '@angular/router';
import { Developer } from '../models/Developer';
import { FilterModel } from '../models/FilterModel';
import { CollectionResult } from '../collection-result';

@Component({
  selector: 'app-list-developers',
  templateUrl: './list-developers.component.html',
  styleUrls: ['./list-developers.component.css']
})
export class ListDevelopersComponent implements OnInit {

  @Input() perPage: number;
  @Input() isModal: boolean;
  @Input() data: CollectionResult<Developer>;
  @Input() project: string;//this is context
  @Input() set: string;
  developers: Developer[];
  currentPage: number;
  totalPages: number;
  totalCount: number;

  constructor(private repo: DeveloperRepoService, private router: ActivatedRoute) {
  }
  get hasContext(): boolean {
    if (!this.project) return true;
    return false;
  }
  get hasParentData() {
    if (!this.data) return false;
    return true;
  }

  ngOnInit() {
    //perPage
    if (!this.perPage) this.perPage = 10;
    //isModal
    if (!this.isModal) this.isModal = false;

    if (this.hasParentData) {
      this.totalPages = this.countPages(this.data.totalCount);
      this.currentPage = 1;
      this.parseResult(this.data);
    }
    else {
      this.loadPage(1);
    }
  }

  private parseResult(result: CollectionResult<Developer>): void {
    if (result) {
      this.developers = result.values;
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
    filter.sort = "fullName";
    filter.context = this.project;
    filter.set = this.set;

    if (this.hasParentData) {
      this.developers = this.data.values.slice(filter.skip, filter.skip + filter.take);
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
