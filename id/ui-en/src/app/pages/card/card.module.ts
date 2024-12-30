import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {CardRoutingModule} from './card-routing.module';
import {CardComponent} from '../card/card.component';
import {FormsModule} from '@angular/forms';


@NgModule({
  declarations: [CardComponent],
  imports: [
    CommonModule,
    CardRoutingModule,
    FormsModule
  ]
})
export class CardModule {
}
