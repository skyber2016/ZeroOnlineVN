import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GeneralService {
  apiName = '';
  constructor(protected http: HttpClient) { }

  Post(obj: any): Observable<any> {
    return this.http.post(environment.host + this.apiName, obj);
  }
}
