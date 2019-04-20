import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateDeveloperComponent } from './create-developer.component';

@Component({
  selector: 'app-all-developers',
  templateUrl: './all-developers.component.html',
  styleUrls: ['./all-developers.component.css']
})
export class AllDevelopersComponent implements OnInit {
  bsModalRef:BsModalRef;
  constructor(public modalService:BsModalService) { }

  ngOnInit() {
  }
  create()
  {
    var initialState = {
      isEdit: false
    };
    this.bsModalRef=this.modalService.show(CreateDeveloperComponent, { initialState });
    
  }

}
