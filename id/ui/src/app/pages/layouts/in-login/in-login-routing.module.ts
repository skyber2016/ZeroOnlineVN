import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {InLoginComponent} from './in-login.component';

const routes: Routes = [
  {
    path: '',
    component: InLoginComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('../../home/home.module').then(m => m.HomeModule)
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InLoginRoutingModule { }
