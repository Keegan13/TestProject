import { AbstractControl } from '@angular/forms';

export class Project {

  public url: string;
  public name: string;
  public description: string;
  public startDate: Date;
  public endDate: Date;
  public status: string;
  constructor(form: AbstractControl) {

    this.name = form.get('name').value;
    this.description = form.get('description').value;
    this.startDate = new Date(form.get('startDate').value);
    this.endDate = new Date(form.get('endDate').value);
    this.status = form.get('status').value;
  }

  public static formatDate(strDate: any) {
    let date = new Date(strDate);
    let dd = date.getDate();
    let mm = date.getMonth()+1;
    let yyyy = date.getFullYear();
    return (dd < 10 ? '0' + dd : dd) + '/' + (mm < 10 ? '0' + mm : mm) + '/' + yyyy;
  }
}
