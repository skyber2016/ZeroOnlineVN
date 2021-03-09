import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import {
  CanActivate,
  CanActivateChild,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from "@angular/router";
import { Observable } from "rxjs";
import { RouterConstant } from "src/app/core/infrastructure/router-constant";

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(private router: Router) { }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const token = localStorage.getItem("token");
    if (!token) {
      this.router.navigate([RouterConstant.auth.login]).then();
      return false;
    }
    return true;
  }

  canActivateChild(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    console.log("canActivateChild");
    return true;
  }
}
