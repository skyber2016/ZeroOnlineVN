import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SyndicateRankingRoutingModule } from './syndicate-ranking-routing.module';
import { SyndicateRankingComponent } from './syndicate-ranking.component';


@NgModule({
  declarations: [SyndicateRankingComponent],
  imports: [
    CommonModule,
    SyndicateRankingRoutingModule
  ]
})
export class SyndicateRankingModule { }
