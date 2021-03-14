import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {catchError, finalize, switchMap} from 'rxjs/operators';
import {Observable, throwError} from 'rxjs';
import {Router} from '@angular/router';
import {RouterConstant} from '../infrastructure/router-constant';
import {AuthService} from 'src/app/shared/services/auth.service';
import {MessageConstants} from '../infrastructure/message-constant';
import {LoaderService} from 'src/app/shared/services/loader.service';
import {MessageService} from '../../shared/services/message.service';

@Injectable()
export class ApiEndpointInterceptor implements HttpInterceptor {
  static counter = 0;

  constructor(
    private router: Router,
    private toastr: MessageService,
    private authService: AuthService,
    private loaderService: LoaderService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    ApiEndpointInterceptor.counter += 1;
    if (ApiEndpointInterceptor.counter === 1) {
      this.loaderService.isLoading.next(true);
    }
    const token = localStorage.getItem('token');
    let request = req.clone({
      url: environment.host + req.url,
    });
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }
    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {
        switch (err.status) {
          case 0:
            this.toastr.error(MessageConstants.ConnectServerFail);
            break;
          case 400:
            this.toastr.error((err.error && err.error.message) || MessageConstants.ConnectServerFail);
            break;
          case 401:
            if (err.url.toLowerCase().endsWith('refreshtoken')) {
              localStorage.clear();
              this.router.navigate([RouterConstant.auth.login]).then();
              break;
            }
            return this.refreshTokenAndRetry(request, next);
          case 404:
            this.router.navigate([RouterConstant.error._404]).then();
            break;
          case 403:
            this.router.navigate([RouterConstant.error._403]).then();
            break;
          default:
            this.toastr.error((err.error && err.error.message) || err.message);
            break;
        }
        return throwError(err);
      }), finalize(() => {
        ApiEndpointInterceptor.counter -= 1;
        if (ApiEndpointInterceptor.counter === 0) {
          this.loaderService.isLoading.next(false);
        }
      })
    );
  }

  refreshTokenAndRetry(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.authService.RefreshToken()
      .pipe(
        switchMap(result => {
          localStorage.setItem('token', result.token);
          return next.handle(this.mappingToken(req));
        }), catchError(err => {
          if (err.status === 401) {
            if(err.url.toLowerCase().endsWith('refreshtoken')){
              localStorage.clear();
              this.router.navigate([RouterConstant.auth.login]).then();
            }
            else{
              return this.refreshTokenAndRetry(req, next);
            }
          }
          throw err;
        }));
  }

  mappingToken(req: HttpRequest<any>) {
    const token = localStorage.getItem('token');
    return req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }
}
