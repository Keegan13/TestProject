import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormControl, FormBuilder, FormGroup } from '@angular/forms';
import { Developer } from '../models/Developer';
import { Project } from '../models/Project';
import { DeveloperRepoService } from '../developer-repo.service';
import { ProjectRepoService } from '../project-repo.service';
import { FilterModel } from '../models/FilterModel';
import { SearchModel } from '../models/SearchModel';
import { MockNgModuleResolver } from '@angular/compiler/testing';
import { CollectionResult } from '../collection-result';
import { ActivatedRoute, Router, NavigationEnd, NavigationStart } from '@angular/router';
import { AssignModel } from '../models/AssignModel';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  @Input() type: string;
  @Input() set: string;
  @Input() context: string;
  @Input() isModal: boolean;

  developers: CollectionResult<Developer>;
  projects: CollectionResult<Project>;
  path: string;
  searchForm: FormGroup;
  constructor(private route: ActivatedRoute, private fb: FormBuilder, private devs: DeveloperRepoService, private projs: ProjectRepoService, private router: Router) {
    router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        this.developers = null;
        this.projects = null;
        this.keywords.setValue("");
        // Hide loading indicator
      }
    });
  }

  ngOnInit() {
    if (!this.context) this.context = "";
    if (!this.set) this.set = 'all';
    this.searchForm = this.fb.group({
      keywords: ['']
    });
  }


  clear() {
    this.developers = null;
    this.projects = null;
  }
  onClick() {
    console.log("click");
  }
  fetch() {
    if (this.searchForm.touched) {
      var filter = new FilterModel();
      filter.keywords = this.keywords.value;
      filter.context = this.context;
      filter.set = this.set;
      this.clear();
      if (this.type == 'any' || this.type == "developer")
        this.devs.get(filter).subscribe(this.hadnleDevs.bind(this));
      if (this.type == 'any' || this.type == 'project')
        this.projs.get(filter).subscribe(this.hadnleProjs.bind(this));
    }
  }
  mouseLeave() {
    if (this.isModal) {
      this.clear();
    }
  }
  private hadnleDevs(data: CollectionResult<Developer>) {
    this.developers = data;
  }

  private hadnleProjs(data: CollectionResult<Project>) {
    this.projects = data;
  }

  // private handleData<T>(data: CollectionResult<T>) {
  //     if(typeof T===typeof 'Developer')

  // }
  get keywords() { return this.searchForm.get('keywords'); }
  get hasProjects() { return this.projects && this.projects.totalCount > 0; }
  get hasDevelopers() { return this.developers && this.developers.totalCount > 0; }
  get isInContext() { return this.context == null; }
}
