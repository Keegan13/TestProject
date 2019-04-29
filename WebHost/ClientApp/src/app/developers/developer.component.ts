import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { Developer } from '../models/Developer';
import { ActivatedRoute, Router } from '@angular/router';
import { DeveloperRepoService } from '../services/developer-repo.service';
import { ModalModule, BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateDeveloperComponent } from './create-developer.component';



@Component({
  selector: 'app-developer',
  templateUrl: './developer.component.html',
  styleUrls: ['./developer.component.css']
})
export class DeveloperComponent implements OnInit {
  public developer: Developer;
  bsModalRef: BsModalRef;

  constructor(private modalService: BsModalService, private route: ActivatedRoute, private repo: DeveloperRepoService, private router: Router) {
  }


  ngOnInit() {
    this.repo.single(this.route.snapshot.params['id']).subscribe((x => { this.developer = x }).bind(this), this.onGetError.bind(this), this.onSuccess.bind(this));
  }
  onGetError(error: any) {

  }
  delete() {

  }
  onUpdate(newValue: Developer) {
    this.developer = newValue;
    this.bsModalRef.hide();
    this.router.navigate(['/developer/' + this.developer.url]);
    
  }
  public edit() {
    var initialState = Object.assign({}, {
      developer: this.developer,
      isEdit: true
    });
    this.bsModalRef = this.modalService.show(CreateDeveloperComponent, { initialState });
    var emiter: EventEmitter<Developer> = this.bsModalRef.content.update;
    emiter.subscribe(this.onUpdate.bind(this));
    //this.bsModalRef.content.developer=this.developer;
    //this.bsModalRef.
  }
  onSuccess() {
    if (this.developer) {

    }
  }


}
