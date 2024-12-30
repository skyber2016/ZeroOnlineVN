import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {CoinToZpsComponent} from './coin-to-zps.component';

const routes: Routes = [
  {
    path: '',
    component: CoinToZpsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoinToZpsRoutingModule { }
