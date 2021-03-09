import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {InLoginComponent} from "./in-login.component";

const routes: Routes = [
  {
    path: '',
    component: InLoginComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InLoginRoutingModule { }
