import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { IService } from './iservice';
import { PaginationBase } from '../models/search-models';

@Injectable({
  providedIn: 'root'
})
export class UsersService implements IService<User> {
  private readonly url = 'api/Users/';
  constructor(private api: ApiService) { }

  search(paging: PaginationBase): Observable<User[]> {
    return this.api.post(this.url + 'Search', paging);
  }

  current(): Observable<User> {
    return this.api.get(this.url + 'Current');
  }

  put(user: User): Observable<User> {
    return this.api.put(this.url + user.id, user);
  }

  post(obj: User) {
    return this.api.post(this.url, obj);
  }

  delete(id: string) {
    return this.api.delete(this.url + id);
  }
}
