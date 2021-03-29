import { Component, OnInit } from '@angular/core';
import {RankingService} from '../../shared/services/ranking.service';

@Component({
  selector: 'app-power-ranking',
  templateUrl: './power-ranking.component.html',
  styleUrls: []
})
export class PowerRankingComponent implements OnInit {
  rankings = [];
  constructor(private rankingService: RankingService) { }

  ngOnInit(): void {
    this.rankingService.power().subscribe(resp => {
      this.rankings = resp;
    })
  }

}
