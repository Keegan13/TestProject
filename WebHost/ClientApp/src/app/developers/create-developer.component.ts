import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators, ValidationErrors } from '@angular/forms';
import { DeveloperRepoService } from './../developer-repo.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Developer } from './../models/Developer';
import { Router } from '@angular/router';
@Component({
  selector: 'app-create-developer',
  templateUrl: './create-developer.component.html',
  styleUrls: ['./create-developer.component.css']
})
export class CreateDeveloperComponent implements OnInit {

  createForm: FormGroup;
  develoepr:Developer;
  constructor(private fb: FormBuilder, private http: HttpClient, private repo: DeveloperRepoService,private router: Router) {


  }
  ngOnInit() {
    this.createForm = this.fb.group({
      fullName: ['', Validators.required],
      nickname: ['', Validators.required],
      skills: ['']
    });
  }
  public onSumbitError(error: any): void {
    var errors = error.error.errors;
    errors.forEach((x) => this.addFieldErrors(x.field, x.messages));
  }
  addFieldErrors(field: string, messages: string[]) {
    field = field.charAt(0).toLowerCase() + field.substring(1);
    this.createForm.get(field).setErrors({ 'serverError': messages.join("<br/>") });
  }

  public onSubmitSuccess() {

    console.log("submites succesfully");
  }
  public onSubmit() {
    this.repo.create(new Developer(this.createForm)).subscribe(((x) => {
      this.develoepr = x;
      if (this.develoepr != null) {
        this.router.navigate(['/developer/'+this.develoepr.url]);
      }
    }).bind(this), this.onSumbitError.bind(this), this.onSubmitSuccess.bind(this));
    console.log("submitting form");
  }
  get nickname() { return this.createForm.get("nickname") }
  get fullName() { return this.createForm.get("fullName") }
}
