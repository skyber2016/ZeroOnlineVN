import {Component, OnInit} from '@angular/core';
import {ShopService} from "../../../shared/services/shop.service";
import {ShopItemService} from "../../../shared/services/shop-item.service";
import {MessageService} from "../../../shared/services/message.service";
import {environment} from "../../../../environments/environment";

@Component({
  selector: 'app-shop-manage',
  templateUrl: './shop-manage.component.html',
  styleUrls: ['./shop-manage.component.css']
})
export class ShopManageComponent implements OnInit {
  name: string;
  price: number;
  qty: number;
  image: File;
  actionId: number;
  items = [];
  host = environment.image_upload;
  isUpdate = false;
  imageUrl: string;

  constructor(private shopService: ShopService, private shopItemService: ShopItemService, private messageService: MessageService) {
  }

  ngOnInit(): void {
    this.initItem();
  }

  initItem(): void {
    this.shopItemService.get<any[]>({}).subscribe(items => {
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
    if (!this.actionId || !this.name || !this.price || !this.qty || !this.imageUrl) {
      return;
    }
    const formData = new FormData();
    formData.append('image', this.imageUrl.trim());
    formData.append('id', this.actionId.toString().trim());
    formData.append('name', this.name.trim());
    formData.append('qty', this.qty.toString().trim());
    formData.append('price', this.price.toString().trim());

    this.shopItemService.post(formData).subscribe(() => {
      this.messageService.success();
      this.initItem();
    })
  }

  onUpload(): void {
    if (!this.actionId || !this.name || !this.price || !this.qty || !this.imageUrl) {
      return;
    }
    if (!this.isUpdate) {
      return;
    }
    const formData = new FormData();
    formData.append('image', this.imageUrl.trim());
    formData.append('id', this.actionId.toString().trim());
    formData.append('name', this.name.trim());
    formData.append('qty', this.qty.toString().trim());
    formData.append('price', this.price.toString().trim());

    this.shopItemService.put(formData).subscribe(() => {
      this.messageService.success();
      this.initItem();
    })
  }

  onEdit(item): void {
    this.isUpdate = true;
    this.actionId = item.id;
    this.name = item.name;
    this.price = item.price;
    this.qty = item.qty;
    this.image = null;
    this.imageUrl = item.image;
  }

}
