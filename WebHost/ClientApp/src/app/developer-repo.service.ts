import { Injectable } from '@angular/core';
import { Developer } from './models/Developer';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { FilterModel } from './models/FilterModel';
import { Repository } from './repository';
import { CollectionResult } from './collection-result';


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

  handlerError(errror: HttpErrorResponse): void {

  }
  public get(filter: FilterModel): Observable<CollectionResult<Developer>> {
    return this.http.get<CollectionResult<Developer>>('/api/developers/get?' + this.getQueryStringFromObject(filter), this.httpOptions);
  }
  public create(dev: Developer): Observable<Developer> {
    return this.http.post<Developer>("/api/developers/create", dev, this.httpOptions);
  }
  public single(idUrl: string): Observable<Developer> {
    return this.http.get<Developer>('/api/developer/' + idUrl);
  }
}
