import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { AssignModel } from './models/AssignModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AssignService {
  header = new HttpHeaders({
    'Content-Type': 'application/json'
  });
  constructor(private http: HttpClient) { }

  public requestAssign(model: AssignModel): Observable<AssignModel> {
    return this.http.post<AssignModel>("api/projects/assign", model, { headers: this.header });
  }

}
