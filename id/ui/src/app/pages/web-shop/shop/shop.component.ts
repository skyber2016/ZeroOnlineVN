import {Component, OnInit} from '@angular/core';
import {ShopService} from "../../../shared/services/shop.service";
import {ShopItemService} from "../../../shared/services/shop-item.service";
import {environment} from "../../../../environments/environment";
import {MessageService} from "../../../shared/services/message.service";
import * as moment from 'moment';
@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit {
  histories = [];
  items = [];
  host = environment.image_upload;

  constructor(private shopService: ShopService, private shopItemService: ShopItemService, private messageService: MessageService) {
  }

  ngOnInit(): void {
    this.initData();
  }

  initData(): void {
    this.shopService.get<any[]>({}).subscribe(histories => {
      this.histories = histories;
    });
    this.shopItemService.get<any[]>({}).subscribe(items => {
      this.items = items;
    })
  }

  buyItem(item): void {
    this.shopService.post({
      itemId: item.id
    }).subscribe(()=>{
      this.messageService.success('Mua thành công');
      this.initData();
    })
  }

  fromNow(time): string {
    moment.locale('vi');
    return moment(time, 'yyyy-MM-DD HH:mm:ss').fromNow();
  }
}
