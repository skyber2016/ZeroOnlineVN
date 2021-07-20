import {Component, OnInit} from '@angular/core';
import {environment} from "../../../../environments/environment";
import {ShopService} from "../../../shared/services/shop.service";
import {ShopItemService} from "../../../shared/services/shop-item.service";
import {MessageService} from "../../../shared/services/message.service";
import {RewardShopItemService} from "../../../shared/services/reward-shop-item.service";
import {RewardShop} from "../../../shared/contants/reward-shop.constants";

@Component({
  selector: 'app-reward-shop-item',
  templateUrl: './reward-shop-item.component.html',
  styleUrls: ['./reward-shop-item.component.css']
})
export class RewardShopItemComponent implements OnInit {
  name: string;
  group: number = 1;
  id: number;
  image: File;
  actionId: number;
  items = [];
  host = environment.image_upload;
  isUpdate = false;
  imageUrl: string;
  reward = RewardShop;
  constructor(
    private shopService: ShopService,
    private rewardShopItemService: RewardShopItemService,
    private shopItemService: ShopItemService,
    private messageService: MessageService
  ) {
  }

  ngOnInit(): void {
    this.initItem();
  }

  initItem(): void {
    this.rewardShopItemService.get<any[]>({}).subscribe(items => {
      this.items = items;
    });
  }

  selectImage(e): void {
    const image = e.target.files[0];
    const formData = new FormData();
    formData.append('file', image);
    this.shopItemService.upload(formData).subscribe(url => {
      this.imageUrl = url;
    })
  }

  onCreate(): void {
    if (!this.actionId || !this.name || !this.group || !this.imageUrl) {
      return;
    }
    this.rewardShopItemService.post(
      {
        image: this.imageUrl,
        actionId: +this.actionId,
        name: this.name,
        group: +this.group
      }
    ).subscribe(() => {
      this.messageService.success();
      this.initItem();
    })
  }

  updateItem(): void {
    if (!this.actionId || !this.name || !this.group || !this.imageUrl) {
      return;
    }
    if (!this.isUpdate) {
      return;
    }

    this.rewardShopItemService.put({
      image: this.imageUrl.trim(),
      id: +this.id,
      actionId: +this.actionId,
      name: this.name.trim(),
      group: +this.group
    }).subscribe(() => {
      this.messageService.success();
      this.initItem();
    })
  }

  onEdit(item): void {
    this.id = item.id;
    this.isUpdate = true;
    this.actionId = item.actionId;
    this.name = item.name;
    this.group = item.group;
    this.image = null;
    this.imageUrl = item.image;
  }

  onDelete(id): void {
    const isOk = confirm('Bạn chắc chắn xóa');
    if(!isOk)
    {
      return;
    }
    this.rewardShopItemService.delete({id}).subscribe(() => {
      this.messageService.success();
      this.initItem();
    });
  }

}
