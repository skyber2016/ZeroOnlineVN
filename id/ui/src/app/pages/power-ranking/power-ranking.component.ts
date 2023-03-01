import { Component, OnInit } from '@angular/core';
import {RankingService} from '../../shared/services/ranking.service';

@Component({
  selector: 'app-power-ranking',
  templateUrl: './power-ranking.component.html',
  styleUrls: []
})
export class PowerRankingComponent implements OnInit {
  rankings = [];
  columns = [];
  constructor(private rankingService: RankingService) { }

  ngOnInit(): void {
    this.rankingService.power().subscribe(resp => {
      const {rankings, columns} = resp;
      this.rankings = rankings;
      this.columns = columns;
    })
  }

}
