import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AssignService } from '../assign.service';
import { HttpHeaders, HttpResponse } from '@angular/common/http';
import { AssignModel } from '../models/AssignModel';

@Component({
  selector: 'app-assign-button',
  templateUrl: './assign-button.component.html',
  styleUrls: ['./assign-button.component.css']
})
export class AssignButtonComponent implements OnInit {
  disabled: boolean;
  @Input() project: string;
  @Input() developer: string;
  @Input() isAssigned: boolean;
  //@Output() changed: EventEmitter<AssignModel>;

  constructor(private srv: AssignService) { }

  ngOnInit() {
    //this.changed=new EventEmitter();
    if (typeof this.project === "undefined" || !this.project || typeof this.developer === 'undefined' || !this.developer || typeof this.isAssigned === "undefined") {
      this.disabled = true;
    }
    if (this.isAssigned === null)
      this.isAssigned = false;
  }
  swap(): void {
    if (this.disabled) return;
    var model: AssignModel = new AssignModel(this.project, this.developer, !this.isAssigned);
    this.srv.requestAssign(model).subscribe(this.handleResponse.bind(this));
  }
  handleResponse(model: AssignModel) {
    this.isAssigned = model.isAssigned;
    //this.changed.emit(model);
  }
}
