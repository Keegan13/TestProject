import { AbstractControl, FormArray } from '@angular/forms';

export class Developer {

  public url: string;
  public fullName: string;
  public nickname: string;
  public skills: string[];
  public project: string;
  constructor() {
  }

  static fromForm(form: AbstractControl) {
    let newDev = new Developer();
    newDev.fullName = form.get('fullName').value;
    newDev.nickname = form.get('nickname').value;
    //this.skills = Developer.parseSkills(form.get('skills').value);
    newDev.url = "";
    newDev.project = "";
    newDev.skills = (form.get('skills') as FormArray).controls.
      map(x => x.value).
      filter((val, index, self) => self.indexOf(val) === index);
    return newDev;
  }
}
