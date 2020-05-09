import { Injectable } from '@angular/core';
import { UserTraining } from '../models/user-training';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { PaginationBase } from '../models/search-models';
import { IService } from './iservice';

@Injectable({
  providedIn: 'root'
})
export class UserTrainingsService implements IService<UserTraining> {
  private readonly url = 'api/UserTrainings/';
  constructor(private api: ApiService) { }

  search(paging?: PaginationBase): Observable<UserTraining[]> {
    if (!paging) {
      paging = new PaginationBase();
      paging.pageSize = 50;
    }
    return this.api.post(this.url + 'Search', paging);
  }

  post(obj: UserTraining): Observable<UserTraining> {
    return this.api.post(this.url, obj);
  }

  calculate(obj: UserTraining): Observable<UserTraining> {
    return this.api.post(this.url + 'Calculate', obj);
  }

  put(obj: UserTraining) {
    return this.api.put(this.url + obj.id, obj);
  }

  delete(id: number) {
    return this.api.delete(this.url + id);
  }
}
