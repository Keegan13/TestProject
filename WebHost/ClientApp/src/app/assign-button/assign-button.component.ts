import { Component, OnInit, Input, EventEmitter, Output, Renderer2, ElementRef } from '@angular/core';
import { AssignService } from '../assign.service';
import { HttpHeaders, HttpResponse } from '@angular/common/http';
import { AssignModel } from '../models/AssignModel';
import * as octicons from 'octicons';
import { updateLocale } from 'ngx-bootstrap/chronos/public_api';
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
  @Input() size: number;
  @Input() assignChanged: EventEmitter<AssignModel> = new EventEmitter<AssignModel>();
  octicon: string;
  color: string;

  //@Output() changed: EventEmitter<AssignModel>;

  constructor(private srv: AssignService, private elementRef: ElementRef, private renderer: Renderer2) {



  }
  //hover
  mouseEnter() {
    this.octicon = this.isAssigned ? 'x' : "checklist";
    this.color = this.isAssigned ? "red" : "green";
    this.update();
  }
  mouseLeave() {
    this.octicon = "checklist";

    this.color = this.isAssigned ? "#007bff" : "grey";
    this.update();
  }
  update() {
    const el: HTMLElement = this.elementRef.nativeElement;
    var button = el.firstElementChild;
    button.innerHTML = octicons[this.octicon].toSVG();
    const icon: Node = button.firstChild;
    this.renderer.setStyle(icon, 'color', this.color);
    this.renderer.setStyle(icon, 'width', this.size);
    this.renderer.setStyle(icon, 'height', '100%');
    button.setAttribute('title', this.isAssigned ? "Unassign developer" : 'Assign developer');
  }

  ngOnInit() {
    //this.changed=new EventEmitter();
    if (this.project === undefined || !this.project || typeof this.developer === 'undefined' || !this.developer || typeof this.isAssigned === "undefined") {
      this.disabled = true;
    }
    if (!this.size) this.size = 26;
    if (this.isAssigned === null)
      this.isAssigned = false;
    this.octicon = "checklist";
    this.color = this.isAssigned ? "#007bff" : "grey";
    this.update();
  }
  swap(): void {
    if (this.disabled) return;
    var model: AssignModel = new AssignModel(this.project, this.developer, !this.isAssigned);
    this.srv.requestAssign(model).subscribe(this.handleResponse.bind(this));
  }
  
  handleResponse(model: AssignModel) {
    if (this.isAssigned != model.isAssigned) {
      this.srv.send(model);
      //this.assignChanged.emit(model);
    }
    this.isAssigned = model.isAssigned;
    this.update();
  }



}
