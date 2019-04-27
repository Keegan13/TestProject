import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }
  onSearch(keys: string | Event) {

    
    console.log(keys);
    if (typeof keys === 'string') {
      this.router.navigate(['search', keys]);
    }
    else {
      //>>>>????/
      console.log(typeof keys);
    }
  }
}
