import {Component, OnInit} from '@angular/core';
import {MessageService} from "../../shared/services/message.service";
import {UserService} from "../../shared/services/user.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  oldPassword: string;
  newPassword: string;
  rePassword: string;

  constructor(private message: MessageService, private userService: UserService, private router: Router) {
  }

  ngOnInit(): void {
  }

  changePassword(): void {
    if (!this.oldPassword || !this.newPassword || !this.rePassword) {
      this.message.error('Vui lòng nhập đầy đủ thông tin');
      return;
    }
    if (this.newPassword != this.rePassword) {
      this.message.error('Nhập lại mật khẩu mới không khớp');
      return;
    }
    if (this.oldPassword == this.newPassword) {
      this.message.error('Mật khẩu mới và mật khẩu cũ không được trùng nhau');
      return;
    }
    this.userService.changePassword({
      oldPassword: this.oldPassword,
      newPassword: this.newPassword
    }).subscribe(async ()=>{
      alert('Đổi mật khẩu thành công, vui lòng đăng nhập lại');
      await this.router.navigate(['/auth/login']);
    })
  }

}
