import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {ApiService} from "./api.service";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GeneralService extends ApiService {
  public apiName: string;

  constructor(http: HttpClient) {
    super(http);
  }

  public getApiUrl(): string {
    return this.apiName;
  }

  public get<TResponse>(parameter: any = null): Observable<any> {
    return super.httpGet(this.getApiUrl(), parameter);
  }

  public getBy<TResponse>(by: any = ''): Observable<any> {
    return super.httpGet(`${this.getApiUrl()}/${by}`, {});
  }

  public index<TResponse>(parameter: any = null): Observable<any> {
    return super.httpGet(this.getApiUrl() + '/index', parameter);
  }

  public post<TResponse>(body: any = null): Observable<any> {
    return super.httpPost(this.getApiUrl(), body);
  }

  public put<TResponse>(body: any = null): Observable<any> {
    return super.httpPut(this.getApiUrl(), body);
  }

  public delete<TResponse>(parameter: any = null): Observable<any> {
    return super.httpDelete(this.getApiUrl(), parameter);
  }

  public changeStatus<TResponse>(parameter: any = null): Observable<any> {
    return this.httpPatch(this.getApiUrl(), null, {params: parameter});
  }
}
