import { Component, OnInit } from '@angular/core';
import {BaseComponent} from '../../shared/components/base.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent extends BaseComponent implements OnInit {

  constructor() { super() }

  ngOnInit(): void {
  }

}
