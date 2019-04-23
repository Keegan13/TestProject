import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-upcomming-projects',
  templateUrl: './upcomming-projects.component.html'
})
export class UpcommingProjectsComponent implements OnInit {
  @Input() isPagination: boolean = true;
  @Input() perPage: number = 25;
  constructor() { }

  ngOnInit() {
  }

}
