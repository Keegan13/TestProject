import { AbstractControl } from '@angular/forms';

export class Project {

  public url: string;
  public name: string;
  public description: string;
  public startDate: Date;
  public endDate: Date;
  public status: string;
  public developerContextUrl:string;

  constructor() {


  }
  public static fromForm(form: AbstractControl): Project {
    let newProj = new Project();
    newProj.name = form.get('name').value;
    newProj.description = form.get('description').value;
    newProj.startDate = new Date(form.get('startDate').value);
    newProj.endDate = new Date(form.get('endDate').value);
    newProj.status = form.get('status').value;
    return newProj;
  }
}
