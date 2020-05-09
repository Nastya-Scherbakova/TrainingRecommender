import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { Muscle } from '../models/muscle';
import { IService } from './iservice';
import { PaginationBase } from '../models/search-models';

@Injectable({
  providedIn: 'root'
})
export class MusclesService implements IService<Muscle> {
  private readonly url = 'api/Muscles/';
  constructor(private api: ApiService) { }

  search(paging?: PaginationBase): Observable<Muscle[]> {
    if (!paging) {
      paging = new PaginationBase();
      paging.pageSize = 50;
    }
    return this.api.post(this.url + 'Search', paging);
  }

  post(obj: Muscle): Observable<Muscle> {
    return this.api.post(this.url, obj);
  }

  put(obj: Muscle) {
    return this.api.put(this.url + obj.id, obj);
  }

  delete(id: number) {
    return this.api.delete(this.url + id);
  }
}
