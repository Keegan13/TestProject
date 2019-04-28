import { Component, OnInit, Inject, Input, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProjectRepoService } from '../services/project-repo.service';
import { Project } from '../models/Project';
import { Router } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { HttpErrorResponse, HttpRequest } from '@angular/common/http';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css']
})

export class CreateProjectComponent implements OnInit {

  constrains = {
    'name': {
      'maxLength': 150,
      'minLength': 5
    },
    'description':
    {
      'maxLength': 450
    }
  };


  validation_messages = {
    'name': [
      { type: 'required', message: 'Project name is required' },
      { type: 'minlength', message: "Project name must be at least " + this.constrains.name.minLength + " characters long" },
      { type: 'maxlength', message: "Project name must be less than" + (this.constrains.name.maxLength + 1) + " characters" },
      { type:'pattern',message: "Project name must not contain '-' (dash) characters"}
    ],
    'description': [
      { type: 'maxlength', message: 'Description must be less than ' + (this.constrains.description.maxLength + 1) + ' characters' }
    ],
    'startDate': [
      { type: 'required', message: 'Start date is required' }
    ],
    'endDate': [
      { type: 'required', message: 'End date is required' }
    ]
  };


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
    if (this.project) this.isEdit = true;
    if (!this.isEdit) this.isEdit = false;
    this.initForm();
  }

  initForm() {
    let name = "Test project";
    let description = "No description";
    let startDate = new Date(Date.now());
    let endDate = new Date(Date.now());
    let status = "0";

    if (this.isEdit && this.project) {
      name = this.project.name;
      description = this.project.description;
      startDate = new Date(this.project.startDate);
      endDate = new Date(this.project.endDate);
      status = this.project.status;
    }

    this.createForm = this.fb.group({
      name: [name,
        Validators.compose([
          Validators.required,
          Validators.maxLength(this.constrains.name.maxLength), 
          Validators.pattern('^([^-]+)$'),
          Validators.minLength(this.constrains.name.minLength)])

      ],
      description: [
        description,
        Validators.maxLength(this.constrains.description.maxLength)
      ],

      startDate: [startDate, Validators.required],

      endDate: [endDate, Validators.required],

      status: [status]
    });
  }

  public onSubmit() {
    if (!this.createForm.touched)
      this.markFormGroupTouched(this.createForm);

    if (this.createForm.valid) {
      let project = new Project(this.createForm);
      if (this.isEdit) {
        project.url = this.project.url;
        this.repo.update(project).subscribe(this.onSubmitResult.bind(this), this.onSumbitError.bind(this));
      }
      else {
        this.repo.create(project).subscribe(this.onSubmitResult.bind(this), this.onSumbitError.bind(this));
      }
    }
  }


  onSubmitResult(project: Project) {
    this.project = project;
    if (this.project) {
      this.name.setValue(this.project.name);
      this.description.setValue(this.project.description);
      this.endDate.setValue(this.project.endDate);
      this.startDate.setValue(this.project.startDate);
      this.status.setValue(this.project.status);
    }
    if (this.update.observers.length > 0)
      this.update.emit(this.project);
    else
      this.router.navigate(['project', this.project.url]);
  }

  onSumbitError(error: HttpErrorResponse): void {
    this.handle4XXError(error);
    this.handle5XXError(error);
  }

  handle5XXError(error: HttpErrorResponse) {
    if (error.status >= 500) {
      this.createForm.setErrors({ 'serverError': 'Whoops something went wrong' });
      console.log(JSON.stringify(error));
    }
  }

  handle4XXError(error: HttpErrorResponse) {
    if (error.status >= 400 && error.status < 500) {
      var errors = error.error.errors;
      if (errors) {
        for (var field in errors) {
          var input = this.createForm.get(field.charAt(0).toLowerCase() + field.substring(1));
          if (input) {
            input.setErrors({ 'serverValidationError': errors[field].join('</br>') });
          }
        }
      }
    }
  }
  /**
 * Marks all controls in a form group as touched
 * @param formGroup - The form group to touch
 * src: https://stackoverflow.com/a/44150793
 */
  private markFormGroupTouched(formGroup: FormGroup) {
    (<any>Object).values(formGroup.controls).forEach(control => {
      control.markAsTouched();

      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }
}
