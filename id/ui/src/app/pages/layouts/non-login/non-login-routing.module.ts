import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {NonLoginComponent} from './non-login.component';
import {SignInComponent} from '../../sign-in/sign-in.component';

const routes: Routes = [
  {
    path: '',
    component: NonLoginComponent,
    children: [
      {
        path: 'login',
        loadChildren: () => import('../../sign-in/sign-in.module').then(m => m.SignInModule)
      },
      {
        path: 'register',
        loadChildren: () => import('../../register/register.module').then(m => m.RegisterModule)
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
