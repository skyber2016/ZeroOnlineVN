import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {from, Observable, Subject, throwError} from "rxjs";
import {catchError, finalize} from "rxjs/operators";

export enum HttpMethod {
  GET,
  POST,
  PUT,
  PATCH,
  DELETE,
  OPTIONS
}

export class PendingRequest {
  url: string;
  method: HttpMethod; // GET, POST, PUT, PATCH
  options: any;
  params: any;
  subscription: Subject<any>;

  constructor(method: HttpMethod, url: string, params: any, options: any, subscription: Subject<any>) {
    this.url = url;
    this.method = method;
    this.options = options || {};
    this.params = params || {};
    this.subscription = subscription;
  }
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private requests$ = new Subject<any>();
  private static queue: PendingRequest[] = [];

  constructor(protected http: HttpClient) {
    this.requests$.subscribe(request => {
      debugger;
      this.execute(request);
    });
  }

  /** Call this method to add your http request to queue */
  invoke(method: HttpMethod, url, params, options): Observable<any> {
    const promise = new Promise(async (res, rej) => {
      const response = this.addRequestToQueue(method, url, params, options);
      response.subscribe(datas => {
        res(datas);
      }, err => {
        rej(err);
      });
    });
    return from(promise);

  }

  private execute(requestData: PendingRequest) {
    let httpCall = this.http.get(requestData.url, {params: requestData.params, ...requestData.options});
    if (requestData.method == HttpMethod.POST) {
      httpCall = this.http.post(requestData.url, requestData.params, requestData.options);
    }
    if (requestData.method == HttpMethod.PUT) {
      httpCall = this.http.put(requestData.url, requestData.params, requestData.options);
    }
    if (requestData.method == HttpMethod.DELETE) {
      httpCall = this.http.delete(requestData.url, {params: requestData.params, ...requestData.options});
    }
    if (requestData.method == HttpMethod.PATCH) {
      httpCall = this.http.patch(requestData.url, requestData.params, requestData.options);
    }
    httpCall
      .pipe(
        catchError(err => {
          return throwError({});
        }),
        finalize(() => {
          ApiService.queue.shift();
          this.startNextRequest();
        })
      )
      .subscribe(res => {
        const sub = requestData.subscription;
        sub.next(res);
      });

  }

  private addRequestToQueue(method: HttpMethod, url, params, options) {
    const sub = new Subject<any>();
    const request = new PendingRequest(method, url, params, options, sub);

    ApiService.queue.push(request);
    if (ApiService.queue.length === 1) {
      this.startNextRequest();
    }
    return sub;
  }

  private startNextRequest() {
    // get next request, if any.
    if (ApiService.queue.length > 0) {
      this.execute(ApiService.queue[0]);
    }
  }

  public httpGet<TResponse>(url: string, parameter: any = {}): Observable<TResponse> {
    let params = new HttpParams();
    if (parameter != null) {
      Object.keys(parameter).forEach(key => {
        if (parameter[key] != null)
          params = params.set(key, parameter[key]);
      });
    }
    return this.invoke(HttpMethod.GET, url, params, {});
  }

  public httpPost<TResponse>(url: string, body: any = null, params = {}): Observable<TResponse> {
    return this.invoke(HttpMethod.POST, url, body, params);
  }

  public httpPut<TResponse>(url: string, body: any = null, params = {}): Observable<TResponse> {
    return this.invoke(HttpMethod.PUT, url, body, params);
  }

  public httpPatch<TResponse>(url: string, body: any = null, params = {}): Observable<TResponse> {
    return this.invoke(HttpMethod.PATCH, url, body, params);
  }

  public httpDelete<TResponse>(url: string, parameter: any = {}): Observable<TResponse> {
    let params = new HttpParams();
    Object.keys(parameter).forEach(key => {
      params = params.set(key, parameter[key]);
    });
    return this.invoke(HttpMethod.DELETE, url, params, {});
  }
}
