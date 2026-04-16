import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WheelRoutingModule } from './wheel-routing.module';
import { WheelComponent } from './wheel.component';
import {TranslateModule} from '@ngx-translate/core';

@NgModule({
  declarations: [WheelComponent],
  imports: [
    CommonModule,
    WheelRoutingModule,
    TranslateModule
  ]
})
export class WheelModule { }
