import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {CardRoutingModule} from './card-routing.module';
import {CardComponent} from '../card/card.component';
import {FormsModule} from '@angular/forms';
import {TranslateModule} from '@ngx-translate/core';



@NgModule({
  declarations: [CardComponent],
  imports: [
    CommonModule,
    CardRoutingModule,
    FormsModule,
    TranslateModule
  ]
})
export class CardModule {
}
