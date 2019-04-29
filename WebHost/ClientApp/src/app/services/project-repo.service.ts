import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Project } from '../models/Project';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FilterModel } from '../models/FilterModel';
import { Repository } from './repository';
import { CollectionResult } from './../models/collection-result';

@Injectable({
  providedIn: 'root'
})

export class ProjectRepoService extends Repository<Project>{

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };


  constructor(private http: HttpClient) {
    super();
  }

  update(project: Project): Observable<Project> {
    return this.http.put<Project>('api/project/' + project.url, project, this.httpOptions).pipe(map(item => {
      item.startDate = new Date(item.startDate);
      item.endDate = new Date(item.endDate);
      return Object.assign(new Project(), item);
    }));
  }

  delete(project: Project): Observable<Project> {
    return this.http.delete<Project>('api/project/' + project.url, this.httpOptions);
  }

  create(project: Project): Observable<Project> {
    return this.http.post<Project>("api/project", project, this.httpOptions).pipe(map(item => {
      item.startDate = new Date(item.startDate);
      item.endDate = new Date(item.endDate);
      return Object.assign(new Project(), item);
    }));
  }

  single(name: string): Observable<Project> {

    return this.http.get<Project>("api/project/" + name, this.httpOptions).pipe(map(item => {
      item.startDate = new Date(item.startDate);
      item.endDate = new Date(item.endDate);
      return Object.assign(new Project(), item);
    }));
  }

  get(filter: FilterModel): Observable<CollectionResult<Project>> {

    return this.http.get<CollectionResult<Project>>("api/project?" + this.getQueryStringFromObject(filter)).pipe(map(result => {
      result.values = result.values.map(item => {
        item.startDate = new Date(item.startDate);
        item.endDate = new Date(item.endDate);
        return Object.assign(new Project(), item);
      });
      return result;
    }));
  }
}

