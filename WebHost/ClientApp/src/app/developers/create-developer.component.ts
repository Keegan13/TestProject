import { EventEmitter, Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
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

  constrains = {
    'fullName': {
      'maxLength': 150,
      'minLength': 5
    },
    'nickname': {
      'maxLength': 100,
      'minLength': 4
    },
    'skills':
    {
      'maxLength': 50
    }
  }

  validation_messages = {
    'fullName': [
      { type: 'required', message: 'Full name is required' },
      { type: 'maxlength', message: 'Full name must be less than ' + (this.constrains.fullName.minLength + 1) + ' characters long' },
      { type: 'minlength', message: 'Full name must be at least ' + this.constrains.fullName.minLength + ' characters long' }

    ],
    'nickname': [
      { type: 'required', message: 'Nickname is required' },
      { type: 'maxlength', message: 'Nickname must be less than ' + (this.constrains.nickname.maxLength + 1) + ' characters' },
      { type: 'minlength', message: 'Nickname must be at least' + this.constrains.nickname.minLength + ' characters long' },
      { type: "pattern", message: "Full name should not contain -" }
    ],
    'skills': [{ type: 'maxlength', message: "Skill name must be less than " + this.constrains.skills.maxLength + " character" },
    { type: 'required', message: 'Skill name is required or remove' }]
  };

  createForm: FormGroup;

  @Input() isEdit: boolean = false;

  @Input() developer: Developer = null;

  @Input() update: EventEmitter<Developer> = new EventEmitter<Developer>();

  get nickname() { return this.createForm.get("nickname") }

  get fullName() { return this.createForm.get("fullName") }

  get skills() { return this.createForm.get('skills') as FormArray }

  constructor(private router: Router, private fb: FormBuilder, private repo: DeveloperRepoService) { }

  ngOnInit() {

    if (this.developer) this.isEdit = true;

    if (!this.isEdit) this.isEdit = false;

    this.initForm(this.developer);
  }

  initForm(developer: Developer = null) {

    let fullNameInit = '';
    let nickNameInit = '';
    let skillsInit = [];

    if (developer) {
      fullNameInit = developer.fullName;
      nickNameInit = developer.nickname;
      skillsInit = developer.skills;
    }

    this.createForm = this.fb.group({
      fullName: [fullNameInit, Validators.compose([
        Validators.required,
        Validators.minLength(this.constrains.fullName.minLength),
        Validators.maxLength(this.constrains.fullName.maxLength)
      ])],

      nickname: [nickNameInit, Validators.compose([
        Validators.required,
        Validators.minLength(this.constrains.nickname.minLength),
        Validators.pattern('^([^-]+)$'), //any character excep dash
        Validators.maxLength(this.constrains.nickname.maxLength)
      ])],

      skills: this.fb.array([...skillsInit.map(x => this.fb.control(x,
        Validators.compose([Validators.maxLength(this.constrains.skills.maxLength), Validators.required]))
      )])
    });

  }

  onSubmit() {

    if (this.createForm.untouched)
      this.markFormGroupTouched(this.createForm);

    if (this.createForm.valid) {

      let developer = Developer.fromForm(this.createForm);

      console.log("Sending developer: "+JSON.stringify(developer));

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
  }

  addSkillInput(skill?: string) {
    if (this.skills.valid) {
      this.skills.push(this.fb.control(skill ? skill : '',
        Validators.compose([Validators.maxLength(this.constrains.skills.maxLength), Validators.required])));
    }
  }
  removeSkill(index: number) {
    this.skills.removeAt(index);
  }
  onSubmitResult(developer: Developer) {
    if (developer) {
      this.initForm(developer);
      if (this.update.observers.length > 0)
        this.update.emit(this.developer);
      else
        this.router.navigate(['developer', this.developer.url]);
    }
    else {
      console.log("TODO create-developer-component.ts");
    }
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
