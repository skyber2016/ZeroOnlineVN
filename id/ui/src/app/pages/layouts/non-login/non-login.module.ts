import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {NonLoginRoutingModule} from './non-login-routing.module';
import {NonLoginComponent} from './non-login.component';


@NgModule({
  declarations: [NonLoginComponent],
  imports: [
    CommonModule,
    NonLoginRoutingModule
  ]
})
export class NonLoginModule {
}
