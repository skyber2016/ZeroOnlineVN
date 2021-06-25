import {Component, OnInit} from '@angular/core';
import {GiftCodeService} from "../../../shared/services/gift-code.service";

@Component({
  selector: 'app-receive-gift-code',
  templateUrl: './receive-gift-code.component.html',
  styleUrls: ['./receive-gift-code.component.css']
})
export class ReceiveGiftCodeComponent implements OnInit {
  giftCode: string;

  constructor(private giftCodeService: GiftCodeService) {
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
      alert('Nhận vật phẩm thành công, vui lòng đến NPC để nhận');
      this.giftCode = '';
    })
  }

}
