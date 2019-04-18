import { Component, OnInit } from '@angular/core';
import {FormBuilder,FormGroup,Validators} from '@angular/forms';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css']
})
export class CreateProjectComponent implements OnInit {
  createForm:FormGroup;
  constructor(private fb: FormBuilder) { }

  ngOnInit() {
      this.createForm=this.fb.group({
        name:['',Validators.required],
        description:[''],
        startDate:[''],
        endDate:['']
      });
  }
  SubmitForm():void
  {
      console.log("submitting form");
  }
}
