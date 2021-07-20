import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {GeneralService} from "./general.service";

@Injectable({
  providedIn: 'root'
})
export class RewardShopItemService extends GeneralService {

  constructor(http: HttpClient) {
    super(http);
    this.apiName = 'RewardShopItem';
  }
}
