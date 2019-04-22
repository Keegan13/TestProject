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
  @Input() project: string;//this is context
  @Input() data: CollectionResult<Developer>;
  @Input() set: string;
  developers: Developer[];
  currentPage: number;
  totalPages: number;


  constructor(private repo: DeveloperRepoService, private router: ActivatedRoute) {
  }
  get hasContext(): boolean {
    return typeof this.project !== 'undefined' && this.project != null;
  }
  get hasParentData() {
    if (typeof this.data != 'undefined' && this.data) {
      return true;
    }
    return false;
  }
  ngOnInit() {
    //perPage
    if (this.perPage === undefined) this.perPage = 10;
    //isModal
    if (typeof this.isModal === 'undefined' || !this.isModal) this.isModal = false;

    if (this.hasParentData) {
      this.totalPages = this.countPages(this.data.totalCount);
      this.currentPage = 1;
      this.parseResult(this.data);
    }
    else {
      this.loadPage(1);
    }
  }
  private parseResult(data: CollectionResult<Developer>): void {
    if (data) {
      this.developers = data.values;
      this.totalPages = this.countPages(data.totalCount);
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
