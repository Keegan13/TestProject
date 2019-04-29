import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-active-projects',
  template: "<h3>Active projects</h3><app-list-projects [pageSize]=\"25\"set=\"active\"></app-list-projects>"
})

export class ActiveProjectsComponent {
  constructor() { }
}
