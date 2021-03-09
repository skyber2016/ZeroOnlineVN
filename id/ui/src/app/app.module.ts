import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {ApiEndpointInterceptor} from './core/http-interceptor/api-endpoint.interceptor';
import {AuthGuard} from './shared/guard/auth.guard';
import { InLoginComponent } from './pages/layouts/in-login/in-login.component';

@NgModule({
  declarations: [
    AppComponent,
    InLoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    AuthGuard,
    {
    provide: HTTP_INTERCEPTORS,
    useClass: ApiEndpointInterceptor,
    multi: true,
  }],
  bootstrap: [AppComponent]
})
export class AppModule {
}
