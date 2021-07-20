import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {RewardShopComponent} from "./reward-shop/reward-shop.component";
import {RewardShopItemComponent} from "./reward-shop-item/reward-shop-item.component";

const routes: Routes = [
  {
    path: '',
    component: RewardShopComponent
  },
  {
    path: 'vat-pham',
    component: RewardShopItemComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RewardShopRoutingModule { }
