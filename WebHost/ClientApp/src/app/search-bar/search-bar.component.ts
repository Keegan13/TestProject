import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SearchModel } from '../models/SearchModel';
import { ActivatedRoute, Router, NavigationStart } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit {
  @Input() type: string;
  @Input() set: string;
  @Input() context: string;
  @Input() text: string;
  @Output() search: EventEmitter<SearchModel> = new EventEmitter<SearchModel>();
  searchForm: FormGroup;

  get keywords() { return this.searchForm.get('keywords'); }

  constructor(private route: ActivatedRoute, private fb: FormBuilder, private router: Router) {
    router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        this.keywords.setValue("");
        // Hide loading indicator
      }
    });

  }

  ngOnInit() {
    if (!this.context) this.context = "";
    if (!this.set) this.set = "";
    if (!this.type) this.type = "any";
    this.searchForm = this.fb.group({
      keywords: ['']
    });
  }

  onClick() {
    var model = new SearchModel();
    model.context = this.context;
    model.keywords = this.keywords.value;
    model.set = this.set;
    model.type = this.type;
    this.search.emit();
  }
}
