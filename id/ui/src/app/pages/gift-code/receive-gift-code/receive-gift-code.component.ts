import {Component, OnInit} from '@angular/core';
import {GiftCodeService} from "../../../shared/services/gift-code.service";
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-receive-gift-code',
  templateUrl: './receive-gift-code.component.html',
  styleUrls: ['./receive-gift-code.component.css']
})
export class ReceiveGiftCodeComponent implements OnInit {
  giftCode: string;

  constructor(private giftCodeService: GiftCodeService, private translate: TranslateService) {
  }

  ngOnInit(): void {
  }

  receive(): void {
    if (!this.giftCode) {
      return;
    }
    this.giftCodeService.receive({
      giftCode: this.giftCode
    }).subscribe(() => {
      alert(this.translate.instant('ALERT.GIFTCODE_SUCCESS'));
      this.giftCode = '';
    })
  }

}
