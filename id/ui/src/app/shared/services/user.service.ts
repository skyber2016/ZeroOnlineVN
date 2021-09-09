import { Injectable } from '@angular/core';
import {GeneralService} from './general.service';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class  UserService extends GeneralService{

  constructor(http: HttpClient) {
    super(http);
    this.apiName = 'User';
  }

  getMoney():Observable<any>{
    return this.httpGet<any>(this.apiName +'/Money');
  }

  coinToZP(coin): Observable<any>{
    return this.httpPost(this.apiName + '/CoinToZP', {money: coin});
  }
  changePassword(params): Observable<any>{
    return this.httpPost(this.apiName + '/ChangePassword', params);
  }
}
