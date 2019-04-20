import { AbstractControl } from '@angular/forms';

export class Project {

  public url:string;
  public name: string;
  public description: string;
  public startDate: Date;
  public endDate: Date;
  public status: string;
  constructor(form: AbstractControl) {

    this.name = form.get('name').value;
    this.description = form.get('description').value;
    this.startDate =new Date(form.get('startDate').value);
    this.endDate=new Date(form.get('endDate').value);
    this.status=form.get('status').value;
  }
}
