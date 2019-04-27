import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { Project } from '../models/Project';
import { ActivatedRoute, Router } from '@angular/router';
import { DeveloperRepoService } from '../services/developer-repo.service';
import { ProjectRepoService } from '../services/project-repo.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateProjectComponent } from './create-project.component';
import { Developer } from '../models/Developer';
import { CollectionResult } from '../models/collection-result';
import { FilterModel } from '../models/FilterModel';
import { AssignModel } from '../models/AssignModel';
import { AssignService } from '../Services/assign.service';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {
  project: Project;
  bsModalRef: BsModalRef;
  developers: CollectionResult<Developer>;
  keywords: string;

  get start() {
    return Project.formatDate(this.project.startDate);
  }
  get end() { return Project.formatDate(this.project.startDate); }

  constructor(private modalService: BsModalService, private route: ActivatedRoute, private router: Router, private repo: ProjectRepoService, private devs: DeveloperRepoService, private assign: AssignService) {
  }

  get id() {
    return this.route.snapshot.params['id'];
  }
  ngOnInit() {

    this.repo.single(this.id).subscribe((x => { this.project = x; }
    ).bind(this), this.onGetError.bind(this), this.onSuccess.bind(this));
    this.loadProject();
    this.developers = new CollectionResult<Developer>();

  }
  loadProject() {

  }


  onGetError(error: any) {

  }

  // onAssignChanged(model: AssignModel) {
  //   if (model.project != this.project.url) return;
  //   for (var i = 0; i < this.developers.values.length; i++) {
  //     if (this.developers.values[i].url == model.developer) {
  //       this.developers.values.splice(i, 1);
  //       this.developers.totalCount--;
  //       break;
  //     }
  //   }
  // }
  onSuccess() {
  }

  onUpdate(val: Project) {
    this.project = val;
    this.bsModalRef.hide();
    this.router.navigate(['/project/' + this.project.url]);
  }
  
  onSearch(keywords: string) {
    if (typeof keywords === 'string') {
      console.log(keywords);
      this.keywords = keywords;
    }
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
