import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  SignIn(obj): Observable<any>{
    return this.http.post('auth/login', obj);
  }

  RefreshToken(): Observable<any>{
    return this.http.post('auth/refreshToken', {refreshToken: localStorage.getItem('refreshToken')});
  }

  Register(obj): Observable<any>{
    return this.http.post('auth/register', obj);
  }

  Logout(): Observable<any>{
    return this.http.post('auth/logout', {});
  }

  ResetPassword(obj): Observable<any>{
    return this.http.post<any>('auth/resetPassword', obj);
  }
}
