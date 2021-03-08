import { Component, OnInit } from '@angular/core';
import {BaseComponent} from "../../../shared/components/base.component";

@Component({
  selector: 'app-in-login',
  templateUrl: './in-login.component.html',
  styleUrls: ['./in-login.component.css']
})
export class InLoginComponent extends BaseComponent implements OnInit {

  constructor() { super() }

  ngOnInit(): void {
  }

}
