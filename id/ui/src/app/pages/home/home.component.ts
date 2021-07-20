import { Component, OnInit } from '@angular/core';
import {BaseComponent} from '../../shared/components/base.component';
import {UserService} from "../../shared/services/user.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent extends BaseComponent implements OnInit {
  username = localStorage.getItem('fullName');
  data:any;
  constructor(private userService: UserService) { super() }

  ngOnInit(): void {
    this.userService.getMoney().subscribe(resp => {
      this.data = resp;
    });
  }

  getVIP(vip: number)
  {
    if(vip >= 6)
    {
      vip = 6;
    }
    return `/assets/images/vip/vip${vip}.png`;
  }
}
