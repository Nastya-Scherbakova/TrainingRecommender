import { Observable } from 'rxjs';
import { PaginationBase } from '../models/search-models';

export interface IService<T> {
  search(paging?: PaginationBase): Observable<T[]>;
  post(obj: T): Observable<T>;
  put(obj: T);
  delete(id: any);
}
