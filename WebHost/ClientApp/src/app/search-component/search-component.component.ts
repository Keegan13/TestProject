import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-search-component',
  templateUrl: './search-component.component.html',
  styleUrls: ['./search-component.component.css']
})
export class SearchComponentComponent implements OnInit {
   _projectCount:number;
   _developersCount:number; 
  
  constructor() { }

  ngOnInit() {
  }

  get projectCount(){return this._projectCount;}
  get developerCount(){return this._developersCount;}
  get count(){
    return this._developersCount+this.projectCount;
  }

}
