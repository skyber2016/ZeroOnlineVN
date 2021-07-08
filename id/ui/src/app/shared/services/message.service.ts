import {Injectable} from '@angular/core';
import {ToastrService} from 'ngx-toastr';
import * as Process from "process";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private toastr: ToastrService) {
  }

  public error(message: string, title: string = ''): void {
    this.toastr.error(message, title);
  }

  public success(message: string = 'Thành công', title: string = ''): void {
    this.toastr.success(message, title);

  }
  public confirm(message: string): Promise<void> {
    return new Promise((res, rej)=>{
      const isOk = confirm(message);
      if(isOk){
        res();
      }
      else{
        rej();
      }
    });
  }
}
