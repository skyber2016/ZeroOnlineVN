import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {HTTP_INTERCEPTORS, HttpClientModule, HttpClient} from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
import {ApiEndpointInterceptor} from './core/http-interceptor/api-endpoint.interceptor';
import {AuthGuard} from './shared/guard/auth.guard';
import {InLoginComponent} from './pages/layouts/in-login/in-login.component';
import {ForgotPasswordComponent} from './pages/forgot-password/forgot-password.component';
import {FormsModule} from '@angular/forms';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

import {ToastrModule} from 'ngx-toastr';
import { CoinToZpsComponent } from './pages/coin-to-zps/coin-to-zps.component';
import { RewardVipComponent } from './pages/reward-vip/reward-vip.component';
import { PowerRankingComponent } from './pages/power-ranking/power-ranking.component';

@NgModule({
  declarations: [
    AppComponent,
    InLoginComponent,
    ForgotPasswordComponent,
    CoinToZpsComponent,
    RewardVipComponent,
    PowerRankingComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
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
