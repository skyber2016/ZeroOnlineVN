import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {CardHistoryComponent} from './card-history.component';

const routes: Routes = [
  {
    path: '',
    component: CardHistoryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CardHistoryRoutingModule { }
