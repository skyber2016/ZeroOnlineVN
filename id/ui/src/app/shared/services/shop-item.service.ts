import {Injectable} from '@angular/core';
import {GeneralService} from "./general.service";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ShopItemService extends GeneralService {

  constructor(http: HttpClient) {
    super(http);
    this.apiName = 'WebShopItem';
  }

  public upload(formData: FormData): Observable<any> {
    return this.httpPost<any>(this.apiName + '/upload', formData);
  }
}
