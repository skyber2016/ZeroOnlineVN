import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/components/base.component';
import {RouterConstant} from '../../core/infrastructure/router-constant';
import {CardService} from '../../shared/services/card.service';
import {MessageService} from '../../shared/services/message.service';

@Component({
  selector: 'app-atm',
  templateUrl: './atm.component.html',
  styleUrls: ['./atm.component.css']
})
export class ATMComponent extends BaseComponent implements OnInit {
  routerContants = RouterConstant;
  username = localStorage.getItem('fullName');

  constructor(private cardService: CardService, private toastrService: MessageService) {
    super();
  }

  ngOnInit(): void {
  }

  async onConfirm() {
    try {
      debugger;
      const file: any = document.getElementById('file-upload');
      if (file.files.length > 0) {
        const fileByte = file.files[0];
        this.cardService.confirmATM(fileByte).toPromise().then(resp => {
          this.toastrService.success('Đã gửi yêu cầu thành công, vui lòng đợi 5-10p');
        }).finally(() =>{
          const element:any = document.getElementById('file-upload');
          element.value = '';
        })
      }
    }
    catch (Exception){

    }
  }
}
