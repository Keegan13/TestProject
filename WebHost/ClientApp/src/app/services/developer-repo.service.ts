import { Injectable } from '@angular/core';
import { Developer } from '../models/Developer';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Repository } from './repository';
import { CollectionResult } from '../models/collection-result';
import { DeveloperFilterModel } from '../models/DeveloperFilterModel';


@Injectable({
  providedIn: 'root'
})

export class DeveloperRepoService extends Repository<Developer> {

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) {
    super();
  }

  delete(developer: Developer): any {
    return this.http.delete<Developer>('api/developer/' + developer.url, this.httpOptions)
  }

  public get(filter: DeveloperFilterModel): Observable<CollectionResult<Developer>> {
    return this.http.get<CollectionResult<Developer>>('/api/developer?' + this.getQueryStringFromObject(filter), this.httpOptions);
  }


  public create(developer: Developer): Observable<Developer> {
    return this.http.post<Developer>("/api/developer", developer, this.httpOptions);
  }

  public single(nickname: string): Observable<Developer> {
    return this.http.get<Developer>('/api/developer/' + nickname);
  }

  public update(developer: Developer): Observable<Developer> {
    return this.http.put<Developer>("/api/developer/" + developer.url, developer, this.httpOptions);
  }
}
