import { Component, OnInit, EventEmitter } from '@angular/core';
import { Project } from '../models/Project';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectRepoService } from '../services/project-repo.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateProjectComponent } from './create-project.component';


@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {
  project: Project;

  bsModalRef: BsModalRef;

  constructor(
    private modalService: BsModalService,
    private route: ActivatedRoute,
    private router: Router,
    private repo: ProjectRepoService) {
  }

  get projectUrl() {
    return this.route.snapshot.params['id'];
  }

  ngOnInit() {
    this.repo.single(this.projectUrl).subscribe(x => this.project = x);
  }

  onUpdate(val: Project) {
    this.project = val;
    this.bsModalRef.hide();
    this.router.navigate(['/project/' + this.project.url]);
  }

  public edit() {
    var initialState = {
      project: this.project,
    };
    this.bsModalRef = this.modalService.show(CreateProjectComponent, { initialState });

    var emiter: EventEmitter<Project> = this.bsModalRef.content.update;
    emiter.subscribe(this.onUpdate.bind(this));
  }
}
