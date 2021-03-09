import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ToastrService {

  constructor() {
  }

  public error(message: string, title: string = ''): void {
    alert(message);
  }

  public success(message: string = 'Thành công', title: string = ''): void {
    alert(message);
  }
}
