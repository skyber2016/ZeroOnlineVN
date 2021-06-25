import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {ReceiveGiftCodeComponent} from "./receive-gift-code/receive-gift-code.component";
import {GiftCodeCreateComponent} from "./gift-code-create/gift-code-create.component";

const routes: Routes = [
  {
    path: '',
    component: ReceiveGiftCodeComponent
  },
  {
    path: 'create',
    component: GiftCodeCreateComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GiftCodeRoutingModule { }
