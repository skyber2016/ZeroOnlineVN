import { Component, OnInit } from '@angular/core';
import {GiftCodeService} from "../../../shared/services/gift-code.service";

@Component({
  selector: 'app-gift-code-history',
  templateUrl: './gift-code-history.component.html',
  styleUrls: ['./gift-code-history.component.css']
})
export class GiftCodeHistoryComponent implements OnInit {
  histories: any[] = [];
  constructor(private giftCode: GiftCodeService) { }

  ngOnInit(): void {
    this.giftCode.history().subscribe(resp => {
      this.histories = resp;
    })
  }

}
