import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
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
      },
      {
        path: 'nhan-thuong-vip',
        loadChildren: () => import('../../reward-vip/reward-vip.module').then(m => m.RewardVipModule)
      },
      {
        path: 'xep-hang/luc-chien',
        loadChildren: () => import('../../power-ranking/power-ranking.module').then(m => m.PowerRankingModule)
      },
      {
        path: 'doi-mat-khau',
        loadChildren: () => import('../../../pages/change-password/change-password.module').then(m => m.ChangePasswordModule)
      },
      {
        path: 'gift-code',
        loadChildren: () => import('../../../pages/gift-code/gift-code.module').then(m => m.GiftCodeModule)
      },
      {
        path: 'vong-quay',
        loadChildren: () => import('../../wheel/wheel.module').then(m => m.WheelModule)
      }

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InLoginRoutingModule {
}
