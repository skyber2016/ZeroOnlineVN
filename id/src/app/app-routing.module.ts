import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./pages/layouts/non-login/non-login.module').then(m => m.NonLoginModule)
  },
  {
    path: '',
    loadChildren: () => import('./pages/layouts/in-login/in-login.module').then(m => m.InLoginModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
