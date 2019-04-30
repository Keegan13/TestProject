import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-search-page',
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.css']
})
export class SearchPageComponent implements OnInit {

  keywords: string;

  constructor(private route: ActivatedRoute, private router: Router) { }

  // setDefault(filter: FilterModel) {
  //   filter.context = "";
  //   filter.set = "all";
  //   filter.skip=0;
  //   filter.take=10;
  //   filter.keywords=this.keywords;
  // }




  ngOnInit() {
    // this.setDefault(this.projectFilter);
    // this.setDefault(this.developerFilter);
    // this.projectFilter.sort="name";
    // this.developerFilter.sort="fullName";
    // this.keywords = this.route.snapshot.params['keywords'];
    // this.router.events.subscribe((val) => {
    //   // s
    //   if (val instanceof NavigationStart) {
    //     let exp = new RegExp('^/search/([^/]+)');
    //     if (exp.test(val.url)) {
    //       this.keywords = exp.exec(val.url)[1];
    //       this.developerFilter.keywords=this.keywords;
    //       this.projectFilter.keywords=this.keywords;
    //     }
    //   }
    // });
  }


}
