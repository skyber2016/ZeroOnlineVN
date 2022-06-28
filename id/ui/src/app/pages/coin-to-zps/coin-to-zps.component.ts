import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/components/base.component';
import {RouterConstant} from '../../core/infrastructure/router-constant';
import {UserService} from '../../shared/services/user.service';

@Component({
  selector: 'app-coin-to-zps',
  templateUrl: './coin-to-zps.component.html',
  styleUrls: ['./coin-to-zps.component.css']
})
export class CoinToZpsComponent extends BaseComponent implements OnInit {
  routerContants = RouterConstant;
  money = 0;
  value = 0;

  constructor(private userService: UserService) {
    super();
  }

  ngOnInit(): void {
    this.userService.getMoney().subscribe(money => this.money = money.webMoney);
  }

  onSubmit() {
    alert('This feature is maintaining');
    return;
    this.userService.coinToZP(this.value).subscribe(resp => {
      alert('You have successfully converted to Zps, please re-login the game to receive it at NPC Receive Gift');
      if (resp.message) {
        alert(resp.message);
      }
      window.location.reload();
    });
  }
}
