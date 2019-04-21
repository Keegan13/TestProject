import { Component, OnInit, Input } from '@angular/core';
import { AssignService } from '../assign.service';
import { HttpHeaders } from '@angular/common/http';
import { AssignModel } from '../models/AssignModel';

@Component({
  selector: 'app-assign-button',
  templateUrl: './assign-button.component.html',
  styleUrls: ['./assign-button.component.css']
})
export class AssignButtonComponent implements OnInit {
  @Input() disabled: boolean;
  @Input() project: string;
  @Input() developer: string;
  @Input() isAssigned: boolean;
  @Input() context: string;

  get text() { return "button text"; }


  constructor(private srv: AssignService) { }

  ngOnInit() {
    if (typeof this.disabled == 'undefined') {
      this.disabled = true;
    }
    if (typeof this.context == 'undefined') {
      this.disabled = true;
    }
    if (typeof this.isAssigned == 'undefined') {
      this.disabled = true;
      this.isAssigned = false;
    }
    if (typeof this.developer == 'undefined' && typeof this.project == 'undefined') {
      this.disabled = true;
    }
  }
  swap(): void {
    if (this.disabled) return;
    var model: AssignModel = null;
    if (this.project) {
      model = new AssignModel(this.project, this.context);
    }
    else
      if (this.developer) {
        model = new AssignModel(this.context, this.developer);
      }

    if (model)
      if (this.isAssigned)
        this.srv.unassign(model);
      else this.srv.assign(model);
  }

}
