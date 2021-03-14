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
      },
      {
        path: 'nap-the',
        loadChildren: () => import('../../card/card.module').then(m => m.CardModule)
      },
      {
        path: 'nap-the/lich-su',
        loadChildren: () => import('../../card-history/card-history.module').then(m => m.CardHistoryModule)
      },
      {
        path: 'nap-the/atm',
        loadChildren: () => import('../../atm/atm.module').then(m => m.AtmModule)
      },
      {
        path: 'doi-tien',
        loadChildren: () => import('../../coin-to-zps/coin-to-zps.module').then(m => m.CoinToZpsModule)
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InLoginRoutingModule { }
