import { EventEmitter, Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DeveloperRepoService } from '../services/developer-repo.service';
import { Developer } from './../models/Developer';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-create-developer',
  templateUrl: './create-developer.component.html',
  styleUrls: ['./create-developer.component.css']
})
export class CreateDeveloperComponent implements OnInit {

  validation_messages = {
    'fullName': [
      { type: 'required', message: 'Full name is required' },
      { type: 'maxlength', message: 'Full name should be less than 250 characters long' },
      { type: 'minlength', message: 'Full name should have more than 2 characters' }
    ],
    'nickname': [
      { type: 'required', message: 'Nickname is required' },
      { type: 'maxlength', message: 'Nickname should be less than 150 character' },
      { type: 'minlength', message: 'Nickname shoud have more than 5 character' }
    ]
  };

  createForm: FormGroup;

  @Input() isEdit: boolean = false;

  @Input() developer: Developer = null;

  @Input() update: EventEmitter<Developer> = new EventEmitter<Developer>();

  get nickname() { return this.createForm.get("nickname") }

  get fullName() { return this.createForm.get("fullName") }

  constructor(private router: Router, private fb: FormBuilder, private repo: DeveloperRepoService) { }

  ngOnInit() {

    if (this.developer) this.isEdit = true;

    if (!this.isEdit) this.isEdit = false;

    this.initForm();
  }

  initForm() {

    let fullNameInit = '';
    let nickNameInit = '';

    if (this.isEdit && this.developer) {
      fullNameInit = this.developer.fullName;
      nickNameInit = this.developer.nickname;
    }

    this.createForm = this.fb.group({
      fullName: [fullNameInit, Validators.compose([
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(250)
      ])],

      nickname: [nickNameInit, Validators.compose([
        Validators.required,
        Validators.minLength(5),
        Validators.pattern('^([^-]+)$'), //any character excep dash
        Validators.maxLength(150)
      ])],
      //skills: ['']
    });

  }

  onSubmit() {
    if (this.createForm.valid) {
      let developer = new Developer(this.createForm);

      if (this.isEdit) {

        developer.url = this.developer.url;
        this.repo.update(developer).subscribe(this.onSubmitResult.bind(this
        ), this.onSubmitError.bind(this
        ));
      }
      else {
        this.repo.create(developer).subscribe(this.onSubmitResult.bind(this
        ), this.onSubmitError.bind(this
        ));

      }
    }
    this.markFormGroupTouched(this.createForm);
  }

  onSubmitResult(developer: Developer) {
    this.developer = developer;
    if (this.developer) {
      this.fullName.setValue(this.developer.fullName);
      this.nickname.setValue(this.developer.nickname);
    }
    if (this.update.observers.length > 0)
      this.update.emit(this.developer);
    else
      this.router.navigate(['developer', this.developer.url]);
  }

  onSubmitError(error: HttpErrorResponse): void {
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
