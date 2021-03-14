import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CardHistoryRoutingModule } from './card-history-routing.module';
import {CardHistoryComponent} from './card-history.component';


@NgModule({
  declarations: [CardHistoryComponent],
  imports: [
    CommonModule,
    CardHistoryRoutingModule
  ]
})
export class CardHistoryModule { }
