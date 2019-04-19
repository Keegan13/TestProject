import { Component, OnInit } from '@angular/core';
import { Developer } from "./../models/Developer";
import {ActivatedRoute} from '@angular/router';
import { DeveloperRepoService } from '../developer-repo.service';

@Component({
  selector: 'app-dev-view',
  templateUrl: './view-developer.component.html',
  styleUrls: ['./view-developer.component.css']
})
export class ViewDeveloperComponent implements OnInit {
  public Developer: Developer;
  constructor(private rotuer:ActivatedRoute,private repo:DeveloperRepoService) {
  }



  ngOnInit() {
    this.repo.single(this.rotuer.snapshot.params['id']).subscribe((x=>{this.Developer=x}).bind(this),this.onGetError.bind(this),this.onSuccess.bind(this));
  }
  onGetError(error:any)
  {

  }
  onSuccess()
  {
    if(this.Developer)
    {
      
    }
  }

}
