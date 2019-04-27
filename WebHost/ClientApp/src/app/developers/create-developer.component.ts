import { EventEmitter, Component, OnInit, Inject, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DeveloperRepoService } from '../services/developer-repo.service';
import { Developer } from './../models/Developer';
import { Router } from '@angular/router';
import { routerNgProbeToken } from '@angular/router/src/router_module';

@Component({
  selector: 'app-create-developer',
  templateUrl: './create-developer.component.html',
  styleUrls: ['./create-developer.component.css']
})
export class CreateDeveloperComponent implements OnInit {
  public createForm: FormGroup;
  @Input() isEdit: boolean;
  @Input() developer: Developer;
  @Input() update: EventEmitter<Developer> = new EventEmitter<Developer>();
  get nickname() { return this.createForm.get("nickname") }
  get fullName() { return this.createForm.get("fullName") }
  constructor(private fb: FormBuilder, private repo: DeveloperRepoService, private router: Router) { }
  ngOnInit() {
    if (this.developer) this.isEdit = true;
    if (!this.isEdit) this.isEdit = false;
    if (this.isEdit && this.developer) this.initEdit(); else this.initCreate();
  }
  private initCreate() {
    this.createForm = this.fb.group({
      fullName: ['', Validators.required],
      nickname: ['', Validators.required],
      //skills: ['']
    });
  }
  private initEdit() {
    this.createForm = this.fb.group({
      fullName: [this.developer.fullName, Validators.required],
      nickname: [this.developer.nickname, Validators.required],
      //skills: ['']
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
          if (this.update.observers.length > 0)
            this.update.emit(this.developer);
          else
            this.router.navigate(['/developer/' + this.developer.url]);
        }
      }).bind(this), this.onSumbitError.bind(this), this.onSubmitSuccess.bind(this));
    }
  }




  public onSumbitError(error: any): void {

    if (error.error) {
      for (var field in error.error) {
        this.addFieldErrors(field, error.error[field]);
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
  }
  addFieldErrors(field: string, messages: string[]) {
    field = field.charAt(0).toLowerCase() + field.substring(1);
    this.createForm.get(field).setErrors({ 'serverError': messages.join("<br/>") });
  }

  public onSubmitSuccess() {
    console.log("submited succesfully");
  }
}
