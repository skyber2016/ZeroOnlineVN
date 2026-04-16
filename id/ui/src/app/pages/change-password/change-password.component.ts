import {Component, OnInit} from '@angular/core';
import {MessageService} from "../../shared/services/message.service";
import {UserService} from "../../shared/services/user.service";
import {Router} from "@angular/router";
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  oldPassword: string;
  newPassword: string;
  rePassword: string;

  constructor(private message: MessageService, private userService: UserService, private router: Router, private translate: TranslateService) {
  }

  ngOnInit(): void {
  }

  changePassword(): void {
    if (!this.oldPassword || !this.newPassword || !this.rePassword) {
      this.message.error(this.translate.instant('ALERT.FILL_INFO'));
      return;
    }
    if (this.newPassword != this.rePassword) {
      this.message.error(this.translate.instant('ALERT.PW_MISMATCH'));
      return;
    }
    if (this.oldPassword == this.newPassword) {
      this.message.error(this.translate.instant('ALERT.PW_SAME'));
      return;
    }
    this.userService.changePassword({
      oldPassword: this.oldPassword,
      newPassword: this.newPassword
    }).subscribe(async ()=>{
      alert(this.translate.instant('ALERT.PW_CHANGE_SUCCESS'));
      await this.router.navigate(['/auth/login']);
    })
  }

}
