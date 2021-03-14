import { Component, OnInit } from '@angular/core';
import {AuthService} from '../../shared/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  name = '';
  answer = '';
  question = '1';
  newPassword = '';
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
  }

  onResetPassword(){
    this.authService.ResetPassword({
      username: this.name,
      answer: this.answer,
      question: +this.question
    }).subscribe(resp => {
      this.newPassword = resp.password;
    })
  }
}
