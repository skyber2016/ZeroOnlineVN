import { Component, OnInit } from '@angular/core';
import {RouterConstant} from '../../core/infrastructure/router-constant';
import {AuthService} from '../../shared/services/auth.service';
import {Router} from '@angular/router';
import {BaseComponent} from '../../shared/components/base.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent extends BaseComponent implements OnInit {

  routerContants = RouterConstant;
  username = '';
  password = '';
  sdt = '';
  question = '1';
  answer = '';
  email = '';

  constructor(private authService: AuthService, private router: Router) {
    super();
  }

  ngOnInit(): void {
  }

  onRegister(){
    this.authService.Register({
      username: this.username,
      password: this.password,
      sdt: this.sdt,
      question: +this.question,
      answer: this.answer,
      email: this.email
    }).subscribe( resp => {
      alert('Successful account registration');
      this.router.navigate([RouterConstant.auth.login]).then();
    })
  }

}
