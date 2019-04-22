import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { Project } from '../models/Project';
import { ActivatedRoute, Router } from '@angular/router';
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

  get start() {
    return Project.formatDate(this.project.startDate);
  }
  get end() { return Project.formatDate(this.project.startDate); }

  constructor(private modalService: BsModalService, private route: ActivatedRoute, private router: Router, private repo: ProjectRepoService, private devs: DeveloperRepoService) {
  }

  get id() {
    return this.route.snapshot.params['id'];
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
  onUpdate(val: Project) {
    this.project = val;
    //this.bsModalRef.hide();
    this.router.navigate(['/project/' + this.project.url]);
  }
  delete() {

  }
  public edit() {
    var initialState = {
      project: this.project,
      isEdit: true
    };
    this.bsModalRef = this.modalService.show(CreateProjectComponent, { initialState });

    var emiter: EventEmitter<Project> = this.bsModalRef.content.update;
    emiter.subscribe(this.onUpdate.bind(this));
  }
}
