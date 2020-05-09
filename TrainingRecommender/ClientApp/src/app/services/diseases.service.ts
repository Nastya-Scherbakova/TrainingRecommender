import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { Disease } from '../models/disease';
import { IService } from './iservice';
import { PaginationBase } from '../models/search-models';

@Injectable({
  providedIn: 'root'
})
export class DiseasesService implements IService<Disease> {
  private readonly url = 'api/Diseases/';
  constructor(private api: ApiService) { }

  search(paging?: PaginationBase): Observable<Disease[]> {
    if (!paging) {
      paging = new PaginationBase();
      paging.pageSize = 50;
    }

    return this.api.post(this.url + 'Search', paging);
  }

  post(obj: Disease): Observable<Disease> {
    return this.api.post(this.url, obj);
  }

  put(obj: Disease) {
    return this.api.put(this.url + obj.id, obj);
  }

  delete(id: number) {
    return this.api.delete(this.url + id);
  }
}
