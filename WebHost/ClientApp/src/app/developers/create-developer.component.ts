import { Component, OnInit, Inject } from '@angular/core';
import {FormControl,FormGroup,FormBuilder,Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
@Component({
  selector: 'app-create-developer',
  templateUrl: './create-developer.component.html',
  styleUrls: ['./create-developer.component.css']
})
export class CreateDeveloperComponent implements OnInit {

  createForm: FormGroup;
  constructor(private fb: FormBuilder,private http: HttpClient) {


   }
  ngOnInit() {
    this.createForm= this.fb.group({
      fullName:['',Validators.required],
      nickname:['',Validators.required],
      skills:this.fb.group({name:['C#']})
    });
  }
  onSubmit()
  {
    this.http.post("/developers/create",this.createForm).subscribe(result=>result);
    console.log("submitting form");
  }
  get nickname(){return this.createForm.get("nickname")}
  get fullName(){return this.createForm.get("fullName")}

}
