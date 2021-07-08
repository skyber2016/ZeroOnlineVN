import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {SyndicateRankingComponent} from "./syndicate-ranking.component";

const routes: Routes = [
  {
    path: '',
    component: SyndicateRankingComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SyndicateRankingRoutingModule { }
