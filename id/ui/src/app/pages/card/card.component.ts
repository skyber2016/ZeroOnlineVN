import {Component, OnInit} from '@angular/core';
import {CardService} from '../../shared/services/card.service';
import {UserService} from '../../shared/services/user.service';
import {RouterConstant} from '../../core/infrastructure/router-constant';
import {Router} from '@angular/router';
import {BaseComponent} from '../../shared/components/base.component';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent extends BaseComponent implements OnInit {

  cards = [];
  cardType: any;
  money = 0;
  serial = '';
  code = '';
  value = 0;
  status = 0;
  routerContants = RouterConstant;

  constructor(private cardService: CardService, private userService: UserService, private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.cardService.cardInfo().subscribe(resp => {
      this.cards = resp;
      if (this.cards.length > 0) {
        this.cardType = this.cards[0].id;
      }
    });

    this.userService.getMoney().subscribe(resp => {
      this.money = resp.webMoney;
    });
  }
  statusClick(status){
    this.status = status;
  }
  onSubmit() {
    this.cardService.post({
      type: +this.cardType,
      seri: this.serial,
      code: this.code,
      value: +this.value,
      status: +this.status
    }).subscribe(resp => {
      alert(resp.message);
      this.router.navigate([this.routerContants.card.history]).then();
    });
  }


}
