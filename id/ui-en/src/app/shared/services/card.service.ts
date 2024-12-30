import { Injectable } from '@angular/core';
import {GeneralService} from './general.service';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CardService extends GeneralService{

  constructor(http: HttpClient) {
    super(http);
    this.apiName = 'Card';
  }

  cardInfo(): Observable<any[]>{
    return this.httpGet<any[]>(this.apiName + '/CardInfo');
  }

  confirmATM(file){
    const formData= new FormData();
    formData.append('file', file);
    return this.httpPost(this.apiName + '/ConfirmBanking', formData);
  }
}
