import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {ATMComponent} from './atm.component';

const routes: Routes = [
  {
    path: '',
    component: ATMComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AtmRoutingModule { }
