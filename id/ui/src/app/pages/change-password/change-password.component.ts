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
      this.message.error('Please enter full information');
      return;
    }
    if (this.newPassword != this.rePassword) {
      this.message.error('Re-enter new password does not match');
      return;
    }
    if (this.oldPassword == this.newPassword) {
      this.message.error('New password and old password cannot be the same');
      return;
    }
    this.userService.changePassword({
      oldPassword: this.oldPassword,
      newPassword: this.newPassword
    }).subscribe(async ()=>{
      alert('Change password successfully, please login again');
      await this.router.navigate(['/auth/login']);
    })
  }

}
