import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SyndicateRankingRoutingModule } from './syndicate-ranking-routing.module';
import { SyndicateRankingComponent } from './syndicate-ranking.component';
import {TranslateModule} from '@ngx-translate/core';

@NgModule({
  declarations: [SyndicateRankingComponent],
  imports: [
    CommonModule,
    SyndicateRankingRoutingModule,
    TranslateModule
  ]
})
export class SyndicateRankingModule { }
