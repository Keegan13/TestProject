import { CollectionResult } from './collection-result';
import { FilterModel } from './models/FilterModel';
import { Observable } from 'rxjs';

export abstract class Repository<T> {
    public static getQueryStringFromObject(options: any) {
        var params = [];
        for (var key in options)
            if (options.hasOwnProperty(key)) {
                params.push(encodeURIComponent(key) + "=" + encodeURIComponent(options[key]));
            }
        return params.join("&");
    }
    public getQueryStringFromObject(obj: any) {
        return Repository.getQueryStringFromObject(obj);
    }
    abstract single(item: string): Observable<T>

    abstract create(entity: T): Observable<T>

    abstract get(filter: FilterModel): Observable<CollectionResult<T>>

    abstract update(entity:T):Observable<T>
    abstract delete(entity:T):any
}
