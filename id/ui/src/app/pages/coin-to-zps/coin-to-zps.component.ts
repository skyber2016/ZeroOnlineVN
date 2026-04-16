import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/components/base.component';
import {RouterConstant} from '../../core/infrastructure/router-constant';
import {UserService} from '../../shared/services/user.service';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-coin-to-zps',
  templateUrl: './coin-to-zps.component.html',
  styleUrls: ['./coin-to-zps.component.css']
})
export class CoinToZpsComponent extends BaseComponent implements OnInit {
  routerContants = RouterConstant;
  money = 0;
  value = 0;

  constructor(private userService: UserService, private translate: TranslateService) {
    super();
  }

  ngOnInit(): void {
    this.userService.getMoney().subscribe(money => this.money = money.webMoney);
  }

  onSubmit() {
    this.userService.coinToZP(this.value).subscribe(resp => {
      alert(this.translate.instant('ALERT.ZPS_SUCCESS'));
      if (resp.message) {
        alert(resp.message);
      }
      window.location.reload();
    });
  }
}
