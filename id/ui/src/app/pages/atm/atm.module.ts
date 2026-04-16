import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AtmRoutingModule } from './atm-routing.module';
import {ATMComponent} from './atm.component';
import {FormsModule} from '@angular/forms';
import {TranslateModule} from '@ngx-translate/core';



@NgModule({
  declarations: [ATMComponent],
    imports: [
        CommonModule,
        AtmRoutingModule,
        FormsModule,
        TranslateModule
    ]
})
export class AtmModule { }
