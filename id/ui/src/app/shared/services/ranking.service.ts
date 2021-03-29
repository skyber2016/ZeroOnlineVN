import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RankingService {

  constructor(private http:HttpClient) { }

  power():Observable<any[]>{
    return this.http.get<any[]>('Ranking/Power');
  }
}
