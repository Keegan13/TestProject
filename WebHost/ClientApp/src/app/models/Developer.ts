import { AbstractControl } from '@angular/forms';

export class Developer {

  public url: string;
  public fullName: string;
  public nickname: string;
  //public skills: string[];
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
    return newDev;
  }

  // private static parseSkills(skills: string): string[] {
  //   var output: string[] = [];
  //   skills.split(',').forEach(function (x) { var skill = x.trim(); if (x.length > 0) output.push(skill) });
  //   return output;
  // }

}
