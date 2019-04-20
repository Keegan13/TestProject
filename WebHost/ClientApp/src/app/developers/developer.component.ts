import { Component, OnInit, Input } from '@angular/core';
import { Developer } from '../models/Developer';
import { ActivatedRoute } from '@angular/router';
import { DeveloperRepoService } from '../developer-repo.service';
import { ModalModule, BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { modalConfigDefaults } from 'ngx-bootstrap/modal/modal-options.class';
import { CreateDeveloperComponent } from './create-developer.component';



@Component({
  selector: 'app-developer',
  templateUrl: './developer.component.html',
  styleUrls: ['./developer.component.css']
})
export class DeveloperComponent implements OnInit {
  public developer: Developer;

  bsModalRef: BsModalRef;

  constructor(private modalService: BsModalService, private router: ActivatedRoute, private repo: DeveloperRepoService) {
  }

  @Input()
  inProjectContext: boolean;


  ngOnInit() {
    this.repo.single(this.router.snapshot.params['id']).subscribe((x => { this.developer = x }).bind(this), this.onGetError.bind(this), this.onSuccess.bind(this));
  }
  onGetError(error: any) {

  }
  delete() {

  }
  update() {

  }
  public edit() {
    var initialState = {
      developer: this.developer,
      isEdit: true
    };
    this.bsModalRef=this.modalService.show(CreateDeveloperComponent, { initialState });
    this.bsModalRef.content.developer=this.developer;
  }
  onSuccess() {
    if (this.developer) {

    }
  }


}
