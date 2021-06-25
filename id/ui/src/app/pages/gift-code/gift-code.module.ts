import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GiftCodeRoutingModule } from './gift-code-routing.module';
import { ReceiveGiftCodeComponent } from './receive-gift-code/receive-gift-code.component';
import {FormsModule} from "@angular/forms";
import { GiftCodeCreateComponent } from './gift-code-create/gift-code-create.component';


@NgModule({
  declarations: [ReceiveGiftCodeComponent, GiftCodeCreateComponent],
  imports: [
    CommonModule,
    GiftCodeRoutingModule,
    FormsModule
  ]
})
export class GiftCodeModule { }
