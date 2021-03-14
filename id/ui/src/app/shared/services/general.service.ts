import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GeneralService {
  apiName = '';
  constructor(public http: HttpClient) { }

  post(obj: any): Observable<any> {
    return this.http.post(this.apiName, obj);
  }

  get<T>(obj: any): Observable<T>{
    let params = new HttpParams();
    Object.keys(obj).forEach(item =>{
      if(obj[item] != null){
        params = params.set(item, obj[item]);
      }
    });
    return this.http.get<T>(this.apiName, {params});
  }
}
