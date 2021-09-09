import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {Router} from "@angular/router";
import {GeneralService} from "./general.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService extends GeneralService{

  constructor(http: HttpClient, private router: Router) { super(http); }

  SignIn(obj): Observable<any>{
    return this.httpPost('auth/login', obj);
  }

  RefreshToken(): Observable<any>{
    return this.httpPost('auth/refreshToken', {refreshToken: localStorage.getItem('refreshToken')});
  }

  Register(obj): Observable<any>{
    return this.httpPost('auth/register', obj);
  }

  Logout(): Observable<any>{
    return this.httpPost('auth/logout', {});
  }

  ResetPassword(obj): Observable<any>{
    return this.httpPost<any>('auth/resetPassword', obj);
  }

  isAdmin(): boolean {
    const p = localStorage.getItem('p');
    return p == '631999';
  }

  public redirectToLogin(returnUrl:string = null): Promise<boolean> {
    if(returnUrl == null){
      returnUrl = this.router.url && this.router.url !== 'auth/login' && this.router.url !== '/' ? this.router.url : '/';
    }
    if(returnUrl.includes('auth/login')){
      returnUrl = '/';
    }
    return this.router.navigate(['auth/login'], {queryParams: {returnUrl}});
  }
}
