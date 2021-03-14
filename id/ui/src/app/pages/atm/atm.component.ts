import { Component, OnInit } from '@angular/core';
import {BaseComponent} from '../../shared/components/base.component';
import {RouterConstant} from '../../core/infrastructure/router-constant';

@Component({
  selector: 'app-atm',
  templateUrl: './atm.component.html',
  styleUrls: ['./atm.component.css']
})
export class ATMComponent extends BaseComponent implements OnInit {
  routerContants = RouterConstant;
  username = localStorage.getItem('fullName');
  constructor() {
    super();
  }

  ngOnInit(): void {
  }

}
