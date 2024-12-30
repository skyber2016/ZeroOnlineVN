import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WheelRoutingModule } from './wheel-routing.module';
import { WheelComponent } from './wheel.component';


@NgModule({
  declarations: [WheelComponent],
  imports: [
    CommonModule,
    WheelRoutingModule
  ]
})
export class WheelModule { }
