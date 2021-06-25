import {Component, OnInit} from '@angular/core';
import {AuthService} from '../../shared/services/auth.service';
import {Router} from '@angular/router';
import {RouterConstant} from '../../core/infrastructure/router-constant';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {
  username = '';
  password = '';
  routerContants = RouterConstant;
  constructor(private authService: AuthService, private router: Router) {
  }

  ngOnInit(): void {
  }

  onSignIn() {
    this.authService.SignIn({username:this.username,password:this.password}).subscribe(resp => {
      localStorage.setItem('token', resp.token);
      localStorage.setItem('refreshToken', resp.refreshToken);
      localStorage.setItem('isAdmin', resp.isAdmin);
      localStorage.setItem('fullName', resp.fullName);
      localStorage.setItem('p', resp.p);
      window.location.href = '/';
    })
  }

}
