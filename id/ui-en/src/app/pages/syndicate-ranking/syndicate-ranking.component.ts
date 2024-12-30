import { Component, OnInit } from '@angular/core';
import { RankingService } from "../../shared/services/ranking.service";
declare const DataTable: any;
declare const $: any;
@Component({
  selector: 'app-syndicate-ranking',
  templateUrl: './syndicate-ranking.component.html',
  styleUrls: []
})
export class SyndicateRankingComponent implements OnInit {

  rankings = [];
  constructor(private rankingService: RankingService) { }

  ngOnInit(): void {
    this.rankingService.syndicate().subscribe(resp => {
      this.rankings = resp;
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
