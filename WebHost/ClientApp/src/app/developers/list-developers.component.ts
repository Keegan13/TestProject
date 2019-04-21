import { Component, OnInit, Input } from '@angular/core';
import { DeveloperRepoService } from '../developer-repo.service';
import { ActivatedRoute } from '@angular/router';
import { Developer } from '../models/Developer';
import { FilterModel } from '../models/FilterModel';
import { CollectionResult } from '../collection-result';
import { AssignService } from '../assign.service';

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
  developers: Developer[];
  currentPage: number;
  totalPages: number;


  constructor(private repo: DeveloperRepoService, private router: ActivatedRoute) {
  }

  ngOnInit() {

    if (this.perPage === undefined) this.perPage = 10;
    if (this.data !== undefined) {
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
      this.totalPages = Math.ceil(result.totalCount / this.perPage);
      console.log(result.totalCount);
    }
  }
  private loadPage(page: number) {
    this.currentPage = page;
    var filter = new FilterModel();
    filter.skip = this.perPage * (page - 1);
    filter.take = this.perPage;
    filter.sortColumn = "fullName";
    filter.context = this.project;
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
