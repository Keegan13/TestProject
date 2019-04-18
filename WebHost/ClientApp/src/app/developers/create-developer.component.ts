import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Developer } from './../models/Developer';
@Component({
  selector: 'app-create-developer',
  templateUrl: './create-developer.component.html',
  styleUrls: ['./create-developer.component.css']
})
export class CreateDeveloperComponent implements OnInit {

  createForm: FormGroup;
  constructor(private fb: FormBuilder, private http: HttpClient) {


  }
  ngOnInit() {
    this.createForm = this.fb.group({
      fullName: ['', Validators.required],
      nickname: ['', Validators.required],
      skills: ['']
    });
  }
  onSubmit() {
    var dev = JSON.stringify(new Developer(this.createForm));
    console.log(dev);
    $.ajax({
      url: "/developers/create",
      type: "POST",
      contentType: "application/json",
      dataType: "json",
      data: dev,

      success: function (result) {
        console.log(result);
      },

      error: function (xhr, resp, text) {
        console.log(xhr, resp, text);
      }
    });

    console.log("submitting form");
  }
  get nickname() { return this.createForm.get("nickname") }
  get fullName() { return this.createForm.get("fullName") }

}
