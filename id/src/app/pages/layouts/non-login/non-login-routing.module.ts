import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {NonLoginComponent} from './non-login.component';
import {SignInComponent} from './sign-in/sign-in.component';
import {RegisterComponent} from "./register/register.component";

const routes: Routes = [
  {
    path: '',
    component: NonLoginComponent,
    children: [
      {
        path: 'login',
        component: SignInComponent
      },
      {
        path: 'register',
        component: RegisterComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NonLoginRoutingModule {
}
