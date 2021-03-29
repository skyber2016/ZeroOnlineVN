import {Injectable} from '@angular/core';
import {GeneralService} from './general.service';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RewardService extends GeneralService {

  constructor(http: HttpClient) {
    super(http);
    this.apiName = 'RewardVip';
  }
}
