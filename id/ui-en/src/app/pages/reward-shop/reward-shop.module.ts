import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RewardShopRoutingModule } from './reward-shop-routing.module';
import { RewardShopItemComponent } from './reward-shop-item/reward-shop-item.component';
import { RewardShopComponent } from './reward-shop/reward-shop.component';
import {FormsModule} from "@angular/forms";


@NgModule({
  declarations: [RewardShopItemComponent, RewardShopComponent],
  imports: [
    CommonModule,
    RewardShopRoutingModule,
    FormsModule
  ]
})
export class RewardShopModule { }
