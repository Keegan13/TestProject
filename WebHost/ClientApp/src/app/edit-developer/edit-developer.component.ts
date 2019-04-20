import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-edit-developer',
  templateUrl: './edit-developer.component.html',
  styleUrls: ['./edit-developer.component.css']
})
export class EditDeveloperComponent implements OnInit {

  url:string;
  constructor(private router:ActivatedRoute) { }

  ngOnInit() {
    this.url=this.router.snapshot.params['id'];
  }

}
