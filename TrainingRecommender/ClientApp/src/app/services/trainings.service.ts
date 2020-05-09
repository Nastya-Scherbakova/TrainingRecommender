import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Training } from '../models/training';
import { Observable } from 'rxjs';
import { SearchTrainings } from '../models/search-models';
import { IService } from './iservice';

@Injectable({
  providedIn: 'root'
})
export class TrainingsService implements IService<Training> {
  private readonly url = 'api/Trainings/';
  constructor(private api: ApiService) { }

  search(search?: SearchTrainings): Observable<Training[]> {
    return this.api.post(this.url + 'Search', search);
  }

  recommend(userId: string): Observable<Training[]> {
    return this.api.get(this.url + 'Recommend/' + userId);
  }

  getById(id: number): Observable<Training> {
    return this.api.get(this.url + id);
  }

  post(obj: Training) {
    return this.api.post(this.url, obj);
  }

  put(obj: Training) {
    return this.api.put(this.url + obj.id, obj);
  }

  delete(id: number) {
    return this.api.delete(this.url + id);
  }
}
