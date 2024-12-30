import { Component, OnInit } from '@angular/core';
import { RankingService } from '../../shared/services/ranking.service';
declare const $: any;
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
      const { rankings, columns } = resp;
      this.rankings = rankings;
      this.columns = columns;
      setTimeout(() => {
        $('#myTable').DataTable({
          responsive: true,
          paging: false,
          ordering: true,
          searching: false,
        })
      }, 500);
    })
  }

}
