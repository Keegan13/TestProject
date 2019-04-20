import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateProjectComponent } from './create-project.component';

@Component({
  selector: 'app-all-projects',
  templateUrl: './all-projects.component.html',
  styleUrls: ['./all-projects.component.css']
})
export class AllProjectsComponent implements OnInit {
  bsModalRef: BsModalRef;
  constructor(public modalService: BsModalService) { }

  ngOnInit() {

  }
  public create() {
    var initialState = {
      isEdit: false
    };
    this.bsModalRef = this.modalService.show(CreateProjectComponent, { initialState });
  }

}
