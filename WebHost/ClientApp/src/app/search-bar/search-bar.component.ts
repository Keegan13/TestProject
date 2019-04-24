import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit {
  private invalid: boolean = false;
  @Input() text: string = "Search";
  @Output() searchEvent: EventEmitter<string> = new EventEmitter<string>();
  searchForm: FormGroup;

  get keywords(): string { return this.searchForm.get('keywords').value; }
  set keywords(val: string) {
    this.searchForm.get('keywords').setValue(val);
    this.invalid = false;
  }
  constructor(private fb: FormBuilder) {
  }
  ngOnInit() {
    this.searchForm = this.fb.group({
      keywords: ['']
    });
  }
  onSubmit() {
    console.log("submit triggered");
    if (this.searchForm.touched) {
      this.searchEvent.emit(this.keywords);
    } else {
      this.invalid = true;
    }
  }
}
