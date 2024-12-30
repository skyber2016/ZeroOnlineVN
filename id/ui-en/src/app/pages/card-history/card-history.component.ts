import { Component, OnInit } from '@angular/core';
import {BaseComponent} from '../../shared/components/base.component';
import {RouterConstant} from '../../core/infrastructure/router-constant';
import {CardService} from '../../shared/services/card.service';

@Component({
  selector: 'app-card-history',
  templateUrl: './card-history.component.html',
  styleUrls: ['./card-history.component.css']
})
export class CardHistoryComponent extends BaseComponent implements OnInit {
  routerContants = RouterConstant;
  cards = [];
  constructor(private cardService: CardService) { super() }

  ngOnInit(): void {
    this.getCards();
  }

  getCards(){
    this.cardService.get<any[]>({}).subscribe(cards =>{
      this.cards = cards;
    });
  }

}
