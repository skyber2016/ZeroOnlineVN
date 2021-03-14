import {AfterViewInit, Component, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from '../../../shared/services/auth.service';
import {Router} from '@angular/router';
import {RouterConstant} from '../../../core/infrastructure/router-constant';
import {UserService} from '../../../shared/services/user.service';

// @ts-ignore
import {version} from '../../../../../package.json';
@Component({
  selector: 'app-in-login',
  templateUrl: './in-login.component.html',
  styleUrls: ['./in-login.component.css']
})
export class InLoginComponent implements OnInit, AfterViewInit, OnDestroy {
  fullName = '';
  routerContant = RouterConstant;
  money = 0;
  constructor(private authService: AuthService, private router: Router, private userService: UserService) { }

  ngOnInit(): void {
    this.fullName = localStorage.getItem('fullName');
    this.getMoney();
  }

  getMoney(){
    this.userService.getMoney().subscribe(resp =>{
      this.money = resp.webMoney;
    })
  }
  css: string[] = [
    '/assets/HT/images/in-login.css',
    '/assets/HT/css/cssmin.css',
    '/assets/HT/css/all.css',
    '/assets/HT/css/cssx.css',
    'https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css',
    '/assets/HT/css/jquery-ui-1.8.16.custom.css',
    '/assets/HT/css/jquery.css',
    'https://fonts.googleapis.com/css?family=Montserrat:300,300i,400,400i,500,500i,600,600i,700,700i',
    'https://maxcdn.bootstrapcdn.com/font-awesome/4.1.0/css/font-awesome.min.css',
    'https://netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css',
    '/assets/HT/css/tooltipster.bundle.min.css',
    '/assets/HT/css/tooltipster-sideTip-punk.min.css',
    '/assets/HT/css/owl.carousel.min.css',
    '/assets/HT/css/owl.theme.default.min.css',
    '/assets/HT/css/style.css',
    '/assets/HT/css/cs.css',
    '/assets/HT/css/style1.css',
    '/assets/HT/css/additions.css',
    '/assets/HT/show/tooltip.css',
  ];

  addCss(source, allSource: string[]): void {
    if (!source) {
      return;
    }
    // add and remove
    const css = document.createElement('link');
    css.rel = 'stylesheet';
    css.href = source + `?version=${version}`;
    css.id = source.replace(/[/:.]/gi, '_');
    css.onload = ()=>{
      this.idloaded.push(css.id);
    }
    document.head.append(css);
    this.addCss(allSource.shift(), allSource);

  }
  idloaded:string[] = [];
  ngAfterViewInit(): void {
    this.addCss(this.css.shift(), this.css);
  }

  ngOnDestroy(): void {
    this.idloaded.forEach(item => document.getElementById(item).remove());
  }

  onLogout(){
    this.authService.Logout().toPromise().finally(() => {
      localStorage.clear();
      this.router.navigate([RouterConstant.auth.login]).then();
    });
  }
}
