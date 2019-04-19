import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators, ValidationErrors } from '@angular/forms';
import { DeveloperRepoService } from './../developer-repo.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Developer } from './../models/Developer';
import { ProjectRepoService } from '../project-repo.service';
import { FilterModel } from './../models/FilterModel';
import { Project } from '../models/Project';
import { errorHandler } from '@angular/platform-browser/src/browser';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css']
})
export class CreateProjectComponent implements OnInit {
  public createForm: FormGroup;
  private submitResult: Project;
  constructor(private fb: FormBuilder, private repo: ProjectRepoService) { }

  ngOnInit() {
    this.createForm = this.fb.group({
      name: ['', Validators.required],
      description: ['no description'],
      startDate: ['01/01/2019'],
      endDate: ['01/01/2019']
    });
  }

  public onSumbitError(error: any): void {
    switch(error.status)
    {
      case 404: 
      this.createForm.setErrors({"serverError":"Service can't be reached"});
      break;
      default: 
      this.createForm.setErrors({"serverError":"Whoops something went wrong"});
      break;
    }
    var errors = error.error.errors;
    if (errors)
      errors.forEach((x) => this.addFieldErrors(x.field, x.messages));
  }
  addFieldErrors(field: string, messages: string[]) {
    field = field.charAt(0).toLowerCase() + field.substring(1);
    this.createForm.get(field).setErrors({ 'serverError': messages.join("<br/>") });
  }

   onSubmitSuccess() {
    //redirect to view
     console.log("Form submited, recieved response : '{0}', now should redirect to project view", JSON.stringify(this.submitResult));
  }

  


  public onSubmit() {
    if (this.createForm.valid) {
      this.repo.create(new Project(this.createForm)).subscribe((x) => { this.submitResult = x; }, this.onSumbitError.bind(this), this.onSubmitSuccess.bind(this));
      console.log("submitting form ");
    }
    else
      console.log("cannot submit form invalid");
  }
  get name() { return this.createForm.get("name"); }
  get description() { return this.createForm.get("description"); }
  get startDate() { return this.createForm.get('startDate'); }
  get endDate() { return this.createForm.get('endDate'); }
}
