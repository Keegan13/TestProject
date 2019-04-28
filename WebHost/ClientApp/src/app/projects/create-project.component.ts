import { Component, OnInit, Inject, Input, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProjectRepoService } from '../services/project-repo.service';
import { Project } from '../models/Project';
import { Router } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { Developer } from '../models/Developer';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css']
})
export class CreateProjectComponent implements OnInit {
  private dateConfig: Partial<BsDatepickerConfig>;
  public createForm: FormGroup;
  @Input() isEdit: boolean;
  @Input() project: Project;
  @Input() update: EventEmitter<Project> = new EventEmitter<Project>();
  get name() { return this.createForm.get("name"); }
  get description() { return this.createForm.get("description"); }
  get startDate() { return this.createForm.get('startDate'); }
  get endDate() { return this.createForm.get('endDate'); }
  get status() { return this.createForm.get('status'); }
  constructor(private fb: FormBuilder, private repo: ProjectRepoService, private router: Router) { }

  ngOnInit() {

    this.dateConfig = Object.assign({});
    if (this.project) this.isEdit = true;
    if (!this.isEdit) this.isEdit = false;
    if (this.isEdit && this.project) this.initEdit(); else this.initCreate();


  }
  private initEdit() {
    this.createForm = this.fb.group({
      name: [this.project.name, Validators.required],
      description: [this.project.description],
      startDate: [new Date(this.project.startDate), Validators.required],
      endDate: [new Date(this.project.endDate), Validators.required],
      status: [this.project.status]
    });
  }
  private initCreate() {
    this.createForm = this.fb.group({
      name: ['', Validators.required],
      description: ['no description'],
      startDate: ['01/01/2019', Validators.required],
      endDate: ['01/01/2019', Validators.required],
      status: [0]
    });
  }

  public onSubmit() {
    if (this.createForm.valid) {
      var action = this.isEdit ? this.repo.update.bind(this.repo) : this.repo.create.bind(this.repo);
      var proj = new Project(this.createForm);
      proj.url = this.isEdit && this.project ? this.project.url : null
      action(proj).subscribe(((x) => {
        this.project = x;
        if (this.project != null) {
          if (this.update.observers.length > 0)
            this.update.emit(this.project);
          else
            this.router.navigate(['/project/' + this.project.url]);
        }
      }).bind(this), this.onSumbitError.bind(this), this.onSubmitSuccess.bind(this));
    }
  }

  public onSumbitError(error: any): void {
    var errors = error.error.errors;
    if (errors) {
      for (var field in errors) {
        this.addFieldErrors(field, errors[field]);
      }
    }
    switch (error.status) {
      case 404:
        this.createForm.setErrors({ "serverError": "Service can't be reached" });
        break;
      default:
        this.createForm.setErrors({ "serverError": "Whoops something went wrong" });
        break;
    }
    console.log(error);

  }
  addFieldErrors(field: string, messages: string[]) {
    field = field.charAt(0).toLowerCase() + field.substring(1);
    this.createForm.get(field).setErrors({ 'serverError': messages.join("<br/>") });
  }

  onSubmitSuccess() {
    console.log("submited succesfully");
  }
}
