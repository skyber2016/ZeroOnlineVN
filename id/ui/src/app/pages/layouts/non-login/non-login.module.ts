import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {NonLoginRoutingModule} from './non-login-routing.module';
import {NonLoginComponent} from './non-login.component';
import {TranslateModule} from '@ngx-translate/core';



@NgModule({
  declarations: [NonLoginComponent],
  imports: [
    CommonModule,
    NonLoginRoutingModule,
    TranslateModule
  ]
})
export class NonLoginModule {
}
