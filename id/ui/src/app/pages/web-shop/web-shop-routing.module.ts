import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {ShopComponent} from "./shop/shop.component";
import {ShopManageComponent} from "./shop-manage/shop-manage.component";

const routes: Routes = [
  {
    path: '',
    component: ShopComponent
  },
  {
    path: 'quan-ly',
    component: ShopManageComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WebShopRoutingModule { }
