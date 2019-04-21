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
  public assign(model: AssignModel): Observable<HttpResponse<any>> {
    return this.http.post<AssignModel>("/projects/assign", model, { observe: "response", headers: this.header });
  }
  public unassign(model: AssignModel): Observable<HttpResponse<any>> {
    return this.http.post<AssignModel>("/projects/unassign", model, { observe: "response", headers: this.header });
  }

}
