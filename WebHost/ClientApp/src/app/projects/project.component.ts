import { Component, OnInit, Input } from '@angular/core';
import { Project } from '../models/Project';
import { ActivatedRoute } from '@angular/router';
import { DeveloperRepoService } from '../developer-repo.service';
import { ProjectRepoService } from '../project-repo.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateProjectComponent } from './create-project.component';
import { Developer } from '../models/Developer';
import { CollectionResult } from '../collection-result';
import { FilterModel } from '../models/FilterModel';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {
  project: Project;
  bsModalRef: BsModalRef;
  developers: CollectionResult<Developer>;
  get start() { return this.formatDate(this.project.startDate); }
  get end() { return this.formatDate(this.project.startDate); }

  formatDate(strDate: any) {
    let date = new Date(strDate);
    let dd = date.getDate();
    let mm = date.getMonth();
    let yyyy = date.getFullYear();
    return (dd < 10 ? '0' + dd : dd) + '/' + (mm < 10 ? '0' + mm : mm) + '/' + yyyy;
  }
  constructor(private modalService: BsModalService, private router: ActivatedRoute, private repo: ProjectRepoService, private devs: DeveloperRepoService) {
  }

  get id() {
    return this.router.snapshot.params['id'];
  }
  ngOnInit() {
    this.repo.single(this.id).subscribe((x => {
      this.project = x;
    }
    ).bind(this), this.onGetError.bind(this), this.onSuccess.bind(this));

  }
  loadProjects() {
    var filter = new FilterModel();
    filter.context = this.id;
    filter.set = 'associated';
    this.devs.get(filter).subscribe(((x) => { this.developers = x; }).bind(this));
  }
  onGetError(error: any) {

  }
  onSuccess() {
  }
  delete() {

  }
  public edit() {
    var initialState = {
      project: this.project,
      isEdit: true
    };
    this.bsModalRef = this.modalService.show(CreateProjectComponent, { initialState });
    this.bsModalRef.content.project = this.project;
  }
}
