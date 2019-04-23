import { Component, OnInit, Input } from '@angular/core';
@Component({
  selector: 'app-active-projects',
  templateUrl: './active-projects.component.html'
})
export class ActiveProjectsComponent implements OnInit {
  @Input() isPagination: boolean = true;
  @Input() perPage: number = 25;
  constructor() { }
  ngOnInit() {

  }
}
