import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AuthGuard} from './shared/guard/auth.guard';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./pages/layouts/non-login/non-login.module').then(m => m.NonLoginModule)
  },
  {
    path: '',
    canActivate: [AuthGuard],
    loadChildren: () => import('./pages/layouts/in-login/in-login.module').then(m => m.InLoginModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
