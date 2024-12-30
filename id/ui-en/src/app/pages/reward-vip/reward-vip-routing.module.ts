import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {RewardVipComponent} from './reward-vip.component';

const routes: Routes = [
  {
    path: '',
    component: RewardVipComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RewardVipRoutingModule { }
