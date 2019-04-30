import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { Developer } from '../models/Developer';
import { ActivatedRoute, Router } from '@angular/router';
import { DeveloperRepoService } from '../services/developer-repo.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateDeveloperComponent } from './create-developer.component';

@Component({
  selector: 'app-developer',
  templateUrl: './developer.component.html',
  styleUrls: ['./developer.component.css']
})

export class DeveloperComponent implements OnInit {

  developer: Developer;

  bsModalRef: BsModalRef;

  constructor(
    private modalService: BsModalService,
    private route: ActivatedRoute,
    private repo: DeveloperRepoService,
    private router: Router) {
  }

  get developerUrl() {
    return this.route.snapshot.params['id'];
  }

  ngOnInit() {
    this.loadDeveloper(this.developerUrl);
  }

  onUpdate(newValue: Developer) {
    this.developer = newValue;
    this.bsModalRef.hide();
    this.router.navigate(['/developer/' + this.developer.url]);
  }

  showEdit() {
    var initialState = Object.assign({}, {
      developer: this.developer,
    });

    this.bsModalRef = this.modalService.show(CreateDeveloperComponent, { initialState });

    var emiter: EventEmitter<Developer> = this.bsModalRef.content.update;

    emiter.subscribe(this.onUpdate.bind(this));
  }
  
  loadDeveloper(url: string) {
    this.repo.single(url).subscribe(x => this.developer = x);
  }

}
