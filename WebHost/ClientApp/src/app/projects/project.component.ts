import { Component, OnInit, Input } from '@angular/core';
import { Project } from '../models/Project';
import { ActivatedRoute } from '@angular/router';
import { DeveloperRepoService } from '../developer-repo.service';
import { ProjectRepoService } from '../project-repo.service';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {

  public project: Project;
  constructor(private rotuer:ActivatedRoute,private repo:ProjectRepoService) {
  }
  ngOnInit() {
    this.repo.single(this.rotuer.snapshot.params['id']).subscribe((x=>{this.project=x}).bind(this),this.onGetError.bind(this),this.onSuccess.bind(this));
  }
  onGetError(error:any)
  {

  }
  onSuccess()
  {
  }
}
