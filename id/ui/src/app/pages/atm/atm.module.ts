import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AtmRoutingModule } from './atm-routing.module';
import {ATMComponent} from './atm.component';


@NgModule({
  declarations: [ATMComponent],
  imports: [
    CommonModule,
    AtmRoutingModule
  ]
})
export class AtmModule { }
