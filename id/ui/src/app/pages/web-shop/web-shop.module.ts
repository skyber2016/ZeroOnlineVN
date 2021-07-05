import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WebShopRoutingModule } from './web-shop-routing.module';
import { ShopComponent } from './shop/shop.component';
import { ShopManageComponent } from './shop-manage/shop-manage.component';
import {FormsModule} from "@angular/forms";


@NgModule({
  declarations: [ShopComponent, ShopManageComponent],
  imports: [
    CommonModule,
    WebShopRoutingModule,
    FormsModule
  ]
})
export class WebShopModule { }
