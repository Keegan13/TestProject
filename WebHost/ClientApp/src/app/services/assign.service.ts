import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { AssignModel } from '../models/AssignModel';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AssignService {
  header = new HttpHeaders({
    'Content-Type': 'application/json'
  });
  private someField = new BehaviorSubject<AssignModel>(null);
  anotherField = this.someField.asObservable();
  constructor(private http: HttpClient) { }

  public requestAssign(model: AssignModel): Observable<AssignModel> {
    return this.http.post<AssignModel>("api/assign", model, { headers: this.header });
  }
  send(model: AssignModel) {
    this.someField.next(model);
  }
  reset()
  {
    
  }


}
