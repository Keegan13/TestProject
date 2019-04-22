import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-project-status',
  templateUrl: './project-status.component.html',
  styleUrls: ['./project-status.component.css']
})
export class ProjectStatusComponent implements OnInit {
  @Input() status:number;
  constructor() { }

  ngOnInit() {
  }

}
