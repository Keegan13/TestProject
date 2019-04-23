import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { ActivatedRoute, Router, NavigationStart } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';


@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit {
  @Input() text:string="Search";
  @Output() search: EventEmitter<string> = new EventEmitter<string>();
  searchForm: FormGroup;

  get keywords(): string { return this.searchForm.get('keywords').value; }
  set keywords(val: string) { this.searchForm.get('keywords').setValue(val); }
  constructor(private route: ActivatedRoute, private fb: FormBuilder, private router: Router) {
    // router.events.subscribe((event) => {
    //   if (event instanceof NavigationStart) {
    //     this.keywords = "";
    //     // Hide loading indicator
    //   }
    //});
  }

  ngOnInit() {
    this.searchForm = this.fb.group({
      keywords: ['']
    });
  }


  onSubmit() {
    console.log("submit triggered");
    if (this.searchForm.touched) {
      this.search.emit(this.keywords);
    }
  }
  // developers: CollectionResult<Developer> = new CollectionResult<Developer>();
  // projects: CollectionResult<Developer> = new CollectionResult<Developer>();




  // get hasProjects(): boolean { return this.developers.totalCount > 0 }
  // get hasDevelopers() { return this.projects.totalCount > 0 }





}
