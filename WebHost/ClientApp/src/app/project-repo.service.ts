import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Project } from './models/Project';
import { Developer } from './models/Developer';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { getQueryValue } from '@angular/core/src/view/query';
import { FilterModel } from './models/FilterModel';
import { Repository } from './repository';
import { CollectionResult } from './collection-result';

@Injectable({
  providedIn: 'root'
})
export class ProjectRepoService extends Repository<Project>{
  update(entity: Project): Observable<Project> {
    return this.http.post<Project>('/projects/update/'+entity.url,entity,this.getOptions);
  }
  delete(entity: Project): Observable<Project> {
    return this.http.post<Project>('/projects/delete/'+entity.url,entity,this.getOptions);
  }
  getOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  constructor(private http: HttpClient) {
    super();
  }

  handlerError(errror: HttpErrorResponse): void {

  }
  create(proj: Project): Observable<Project> {
    return this.http.post<Project>("api/projects/create", proj, this.getOptions);
  }

  single(key: string): Observable<Project> {
    return this.http.get<Project>("api/project/" + key, this.getOptions);
  }
  getActive(): Observable<Project[]> {
    return this.http.get<Project[]>("api/projects/getactive", this.getOptions);
  }

  get(filter: FilterModel): Observable<CollectionResult<Project>> {
    return this.http.get<CollectionResult<Project>>("api/projects/get?" + this.getQueryStringFromObject(filter));
  }
  public assign() {

  }
  public unassign() {

  }
  public countPages() {

  }
}

