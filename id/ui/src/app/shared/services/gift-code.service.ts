import {Injectable} from '@angular/core';
import {GeneralService} from "./general.service";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class GiftCodeService extends GeneralService {

  constructor(http: HttpClient) {
    super(http);
    this.apiName = 'GiftCode';
  }

  public receive(params): Observable<any>{
    return this.http.post(this.apiName + '/Receive', params);
  }
  public history(): Observable<any[]> {
    return this.http.get<any[]>(this.apiName + '/History');
  }
}
