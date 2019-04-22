import { Component, OnInit, Input } from '@angular/core';
import { DeveloperRepoService } from '../developer-repo.service';
import { ActivatedRoute } from '@angular/router';
import { Developer } from '../models/Developer';
import { FilterModel } from '../models/FilterModel';
import { CollectionResult } from '../collection-result';
import { AssignModel } from '../models/AssignModel';

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
  ngOnInit() {
    //perPage
    if (this.perPage === undefined) this.perPage = 10;
    //isModal
    if (typeof this.isModal === 'undefined') this.isModal = false;
    //project
    //set
    if (this.data !== undefined) {
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
      this.totalPages = Math.ceil(data.totalCount / this.perPage);
      if (this.totalPages - 1 == data.totalCount / this.perPage) {
        this.totalPages += 1;
      }
    }
  }
  private loadPage(page: number) {
    this.currentPage = page;
    var filter = new FilterModel();
    filter.skip = this.perPage * (page - 1);
    filter.take = this.perPage;
    filter.sort = "fullName";
    filter.context = this.project;
    filter.set = this.set;
    this.repo.get(filter).subscribe(this.parseResult.bind(this));
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
