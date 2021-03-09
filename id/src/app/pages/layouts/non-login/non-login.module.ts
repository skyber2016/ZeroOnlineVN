import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {NonLoginRoutingModule} from './non-login-routing.module';
import {SignInComponent} from './sign-in/sign-in.component';
import {NonLoginComponent} from './non-login.component';
import { RegisterComponent } from './register/register.component';


@NgModule({
  declarations: [NonLoginComponent, SignInComponent, RegisterComponent],
  imports: [
    CommonModule,
    NonLoginRoutingModule
  ]
})
export class NonLoginModule {
}
