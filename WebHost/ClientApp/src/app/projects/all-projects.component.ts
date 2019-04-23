import { Component, OnInit, Input } from '@angular/core';
@Component({
  selector: 'app-all-projects',
  templateUrl: './all-projects.component.html',
  styleUrls: ['./all-projects.component.css']
})
export class AllProjectsComponent implements OnInit {
  @Input() isPagination: boolean = true;
  @Input() perPage: number = 25;
  
  constructor() { }

  ngOnInit() {

  }
}
