import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationStart, NavigationError, NavigationEnd } from '@angular/router';
import { routerNgProbeToken } from '@angular/router/src/router_module';

@Component({
  selector: 'app-search-page',
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.css']
})
export class SearchPageComponent implements OnInit {

  keywords: string;
  constructor(private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.keywords = this.route.snapshot.params['keywords'];
    this.router.events.subscribe((val) => {
      // s
      if (val instanceof NavigationStart) {
        let exp = new RegExp('^/search/([^/]+)');
        if (exp.test(val.url)) {
          this.keywords = exp.exec(val.url)[1];
        }
      }
    });
  }


}
