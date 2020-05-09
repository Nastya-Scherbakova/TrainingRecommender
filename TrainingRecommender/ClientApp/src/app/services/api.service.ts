import { Injectable } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly apiRoute: string = '/';
  constructor(private http: HttpClient) {
  }

  getArg(route: string, args: Array<any>): Observable<any> {
    let params = new HttpParams();
    args.forEach(element => {
      params = params.set(element.name, element.value);
    });
    return this.http.get(this.apiRoute + route, { params});
  }

  postArg(route: string, args: Array<any>, obj: any): Observable<any> {
    let params = new HttpParams();
    args.forEach(element => {
      params = params.set(element.name, element.value);
    });
    return this.http.post(this.apiRoute + route, obj, { params});
  }

  get(route: string): Observable<any> {
    return this.http.get(this.apiRoute + route);
  }

  post(route: string, obj: any): Observable<any> {
    return this.http.post(this.apiRoute + route, obj);
  }

  put(route: string, obj: any): Observable<any> {
    return this.http.put(this.apiRoute + route, obj);
  }

  delete(route: string): Observable<any> {
    return this.http.delete(this.apiRoute + route);
  }
}
