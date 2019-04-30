import { Component, OnInit, Input, EventEmitter, Output, Renderer2, ElementRef } from '@angular/core';
import { AssignService } from '../Services/assign.service';
import { AssignModel } from '../models/AssignModel';
import * as octicons from 'octicons';

@Component({
  selector: 'app-assign-button',
  templateUrl: './assign-button.component.html',
  styleUrls: ['./assign-button.component.css']
})

export class AssignButtonComponent implements OnInit {
  //Required
  @Input() projectUrl: string;

  @Input() developerUrl: string;

  @Input() isAssigned: boolean = false;

  //Optional
  octicon: string = "checklist";

  color: string = "gray";

  size: number = 26;

  //internal
  disabled: boolean=true;

  constructor(
    private assignService: AssignService,
    private elementRef: ElementRef,
    private renderer: Renderer2) { }

  //ok
  mouseEnter() {
    this.octicon = this.isAssigned ? 'x' : "checklist";
    this.color = this.isAssigned ? "red" : "green";
    this.update();
  }
  //ok
  mouseLeave() {
    this.octicon = "checklist";
    this.color = this.isAssigned ? "#007bff" : "grey";
    this.update();
  }
  //ok 
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
    if (this.projectUrl && this.developerUrl) {
      this.disabled=false;
      this.color=this.isAssigned ? "#007bff" : "grey"
      this.update();
    }
    else {
      this.disabled = true;
    }
  }

  swap() {
    console.log("entered swap");
    if (!this.disabled) {
      console.log("entered assign");
      this.assignService
        .requestAssign(new AssignModel(
          this.projectUrl,
          this.developerUrl,
          !this.isAssigned))
        .subscribe(this.handleResponse.bind(this));
    }
  }

  handleResponse(model: AssignModel) {
    if (this.isAssigned != model.isAssigned) {
      this.assignService.send(model);
      this.isAssigned = model.isAssigned;
      this.update();
    }
  }
}
