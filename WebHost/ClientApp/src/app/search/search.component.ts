import { Component, OnInit, Input, EventEmitter, Output, ViewChild } from '@angular/core';
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
import { ListDevelopersComponent } from '../developers/list-developers.component';
import { ListProjectsComponent } from '../projects/list-projects.component';
import { ProjectStatusComponent } from '../project-status/project-status.component';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  @Input() type: string;
  @Input() set: string;
  @Input() context: string;
  @Input() isModal: boolean = false;
  @ViewChild(ListDevelopersComponent) developersComp: ListDevelopersComponent;
  @ViewChild(ListProjectsComponent) projectsComp: ListProjectsComponent;

  

  // developers: CollectionResult<Developer> = new CollectionResult<Developer>();
  // projects: CollectionResult<Developer> = new CollectionResult<Developer>();
  searchForm: FormGroup;

  get keywords(): string { return this.searchForm.get('keywords').value; }
  set keywords(val: string) { this.searchForm.get('keywords').setValue(val); }

  // get hasProjects(): boolean { return this.developers.totalCount > 0 }
  // get hasDevelopers() { return this.projects.totalCount > 0 }

  get isInContext() {
    if (!this.context) return false;
    return true;
  }

  get devRender(): boolean {
    return this.type == 'any' || this.type == 'developer';
  }
  get projRender(): boolean {
    return this.type == 'any' || this.type == 'project';
  }


  show: boolean = true;
  constructor(private route: ActivatedRoute, private fb: FormBuilder, private devRepo: DeveloperRepoService, private projRepo: ProjectRepoService, private router: Router) {
    router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        this.keywords = "";
        // Hide loading indicator
      }
    });
  }

  ngOnInit() {
    if (this.isModal) {
      this.show = false;
    }
    if (!this.context) this.context = "";
    if (!this.set) this.set = 'all';
    this.searchForm = this.fb.group({
      keywords: ['']
    });
  }


  clear() {

    if (this.developersComp) {
      this.developersComp.disabled = true;
      this.developersComp.keywords = null;
    }
    if (this.projectsComp) {
      this.projectsComp.disabled = true;
      this.projectsComp.keywords = null;
    }
  }
  onClick() {
    console.log("click");
  }
  fetch() {
    if (this.searchForm.touched) {
      var filter = new FilterModel();
      filter.keywords = this.keywords;
      filter.context = this.context;
      filter.set = this.set;
      this.clear();

      if (this.devRender) {
        this.developersComp.disabled = false;
        this.developersComp.keywords = this.keywords;
        this.developersComp.update();
      }

      if (this.projRender) {
        this.developersComp.disabled = false;
        this.projectsComp.keywords = this.keywords;
        this.projectsComp.update();
      }
    }

    // if (this.searchForm.touched) {
    //   this.clear();
    //   this.show = true;

    //   if (this.type == 'any' || this.type == "developer") {
    //     this.developersComp.keywords = this.keywords;
    //     this.developersComp.update();
    //   }

    //   if (this.type == 'any' || this.type == 'project')
    //     this.developersComp.keywords = this.keywords;
    // }
  }

  mouseLeave() {
    if (this.isModal) {
      this.clear();
      this.show = false;
    }
  }


}
