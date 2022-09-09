import { Component, OnInit } from '@angular/core';
import {RewardService} from "../../../shared/services/reward.service";
import {ToastrService} from "ngx-toastr";
import {RewardShopItemService} from "../../../shared/services/reward-shop-item.service";
import {RewardShopService} from "../../../shared/services/reward-shop.service";
import {RewardShop} from "../../../shared/contants/reward-shop.constants";
import {environment} from "../../../../environments/environment";

@Component({
  selector: 'app-reward-shop',
  templateUrl: './reward-shop.component.html',
  styleUrls: ['./reward-shop.component.css']
})
export class RewardShopComponent implements OnInit {
  rewardShop = RewardShop;
  host = environment.image_upload;
  current: number = 0;
  rewards = [];

  constructor(private rewardShopItemService: RewardShopItemService, private rewardShopService: RewardShopService, private toastrService: ToastrService) {
  }

  ngOnInit(): void {
    this.onInitData();
  }
  onInitData(){
    this.rewardShopService.get<any>({}).subscribe(result => {
      this.rewards = result.rewards;
      this.current = result.current;
    });
  }
  onSubmit(reward: any) {
    if (reward.status === 2) {
      this.rewardShopService.post({group: reward.group}).subscribe(resp => {
        this.onInitData();
        this.toastrService.success('Receiving gifts successful');
      });
    }
  }

}
