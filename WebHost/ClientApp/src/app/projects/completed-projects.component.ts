import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-completed-projects',
  templateUrl: './completed-projects.component.html'
})
export class CompletedProjectsComponent implements OnInit {
  @Input() isPagination: boolean = true;
  @Input() perPage: number = 25;
  constructor() { }

  ngOnInit() {
  }

}
