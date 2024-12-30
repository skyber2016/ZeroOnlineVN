import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AtmRoutingModule } from './atm-routing.module';
import {ATMComponent} from './atm.component';
import {FormsModule} from '@angular/forms';


@NgModule({
  declarations: [ATMComponent],
    imports: [
        CommonModule,
        AtmRoutingModule,
        FormsModule
    ]
})
export class AtmModule { }
