import {Component, OnInit} from '@angular/core';
import {GiftCodeService} from "../../../shared/services/gift-code.service";
import {MessageService} from "../../../shared/services/message.service";
import {AuthService} from "../../../shared/services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-gift-code-create',
  templateUrl: './gift-code-create.component.html',
  styleUrls: ['./gift-code-create.component.css']
})
export class GiftCodeCreateComponent implements OnInit {
  giftCode: string;
  type = 2;
  itemId: number;
  giftCodes = [];
  types = [
    {
      key: 1,
      value: '1 cho 1'
    },
    {
      key: 2,
      value: '1 cho tất cả'
    }
  ]

  constructor(private giftCodeService: GiftCodeService, private message: MessageService, private authService: AuthService, private router: Router) {
    if (!this.authService.isAdmin()) {
      this.router.navigate(['/gift-code']).then();
    }
  }

  ngOnInit(): void {
    this.giftCode = this.randomString(6, 'a');
    this.initData();
  }

  initData(): void {
    this.giftCodeService.get<any[]>({}).subscribe(res => {
      this.giftCodes = res;
    });
  }

  create(): void {
    if (!this.giftCode || !this.itemId) {
      return;
    }
    this.giftCodeService.post({
      giftCode: this.giftCode,
      itemId: +this.itemId,
      type: +this.type
    }).subscribe(() => {
      this.message.success('Tạo Gift Code thành công');
      this.giftCode = this.randomString(6, 'a');
      this.itemId = null;
      this.initData();
    });
  }

  randomString(len, an) {
    an = an && an.toLowerCase();
    let str = "",
      i = 0,
      min = an == "a" ? 10 : 0,
      max = an == "n" ? 10 : 62;
    for (; i++ < len;) {
      let r = Math.random() * (max - min) + min << 0;
      str += String.fromCharCode(r += r > 9 ? r < 36 ? 55 : 61 : 48);
    }
    return str;
  }
}
