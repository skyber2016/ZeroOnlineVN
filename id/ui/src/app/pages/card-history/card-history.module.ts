import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CardHistoryRoutingModule } from './card-history-routing.module';
import {CardHistoryComponent} from './card-history.component';
import {TranslateModule} from '@ngx-translate/core';



@NgModule({
  declarations: [CardHistoryComponent],
  imports: [
    CommonModule,
    CardHistoryRoutingModule,
    TranslateModule
  ]
})
export class CardHistoryModule { }
