import { Component, OnInit, Inject, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DeveloperRepoService } from './../developer-repo.service';
import { Developer } from './../models/Developer';
import { Router } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
@Component({
  selector: 'app-create-developer',
  templateUrl: './create-developer.component.html',
  styleUrls: ['./create-developer.component.css']
})
export class CreateDeveloperComponent implements OnInit {
  public createForm: FormGroup;
  @Input() isEdit: boolean;
  @Input() developer: Developer;
  get nickname() { return this.createForm.get("nickname") }
  get fullName() { return this.createForm.get("fullName") }
  constructor(public bsModalRef: BsModalRef, private fb: FormBuilder, private repo: DeveloperRepoService, private router: Router) { }
  ngOnInit() {
    if (this.developer) this.isEdit = true;
    if (!this.isEdit) this.isEdit = false;
    if (this.isEdit && this.developer) this.initEdit(); else this.initCreate();
  }
  private initCreate() {
    this.createForm = this.fb.group({
      fullName: ['', Validators.required],
      nickname: ['', Validators.required],
      skills: ['']
    });
  }
  private initEdit() {
    this.createForm = this.fb.group({
      fullName: [this.developer.fullName, Validators.required],
      nickname: [this.developer.nickname, Validators.required],
      skills: ['']
    });
  }
  public onSubmit() {
    if (this.createForm.valid) {
      var action = this.isEdit ? this.repo.update.bind(this.repo) : this.repo.create.bind(this.repo);
      var dev = new Developer(this.createForm);
      dev.url = this.isEdit && this.developer ? this.developer.url : null
      action(dev).subscribe(((x) => {
        this.developer = x;
        if (this.developer != null) {
          this.bsModalRef.hide();
          this.router.navigate(['/developer/' + this.developer.url]);
          
        }
      }).bind(this), this.onSumbitError.bind(this), this.onSubmitSuccess.bind(this));
    }
  }

  public onSumbitError(error: any): void {
    switch (error.status) {
      case 404:
        this.createForm.setErrors({ "serverError": "Service can't be reached" });
        break;
      default:
        this.createForm.setErrors({ "serverError": "Whoops something went wrong" });
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

  public onSubmitSuccess() {
    console.log("submited succesfully");
  }
}
