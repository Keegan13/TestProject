import { AbstractControl, FormArray } from '@angular/forms';

export class Developer {

  public url: string;
  public fullName: string;
  public nickname: string;
  public tags: string[];
  public project: string;
  constructor() {
  }

  static fromForm(form: AbstractControl) {
    let newDev = new Developer();
    newDev.fullName = form.get('fullName').value;
    newDev.nickname = form.get('nickname').value;
    newDev.url = "";
    newDev.project = "";
    newDev.tags = (form.get('tags') as FormArray).controls.
      map(x => x.value).
      filter((val, index, self) => self.indexOf(val) === index);
    return newDev;
  }
}
