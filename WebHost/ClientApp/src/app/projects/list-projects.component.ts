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
  @Input() isModal:boolean;
  @Input() data:CollectionResult<Project>;
  @Input() developer: string;
  projects:Project[];
  currentPage: number;
  totalPages: number;

  constructor(private repo: ProjectRepoService, private router: ActivatedRoute) {
        }

  ngOnInit() {
    if(!this.isModal)this.isModal=false;
    if(this.perPage===undefined)this.perPage=10;
    if(this.data!==undefined)
    {
      this.currentPage=1;
      this.parseResult(this.data);
    }
    else
    {
      this.loadPage(1);
    }
    
  }
  private parseResult(result: CollectionResult<Project>): void {
    if (result) {
      this.projects = result.values;
      this.totalPages = Math.ceil(result.totalCount / this.perPage);
      console.log(result.totalCount);
    }
  }
  private loadPage(page: number) {
    this.currentPage = page;
    var filter = new FilterModel();
    filter.skip = this.perPage*(page-1);
    filter.take = this.perPage;
    filter.sortColumn = "name";
    filter.context=this.developer;
    this.repo.get(filter).subscribe(this.parseResult.bind(this));
  }
  public nextPage(): void {
    if (this.totalPages > this.currentPage) {   
      this.loadPage(this.currentPage+1);
    }
  }

  public previousPage(): void {
    if (this.currentPage > 1) {
      this.loadPage(this.currentPage-1);
    }
  }


}
