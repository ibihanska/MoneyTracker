import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorHandler, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AccountModule } from './account/account.module';
import { TransactionModule } from './transaction/transaction.module';
import { NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AuthModule } from '@auth0/auth0-angular';

import { AppComponent } from './app.component';
import { ToastsContainer } from './shared/toasts-container.component';
import { AuthButtonComponent } from './authentication/auth-button.component';
import { AuthHttpInterceptor } from './authentication/interceptor.service';
import { AuthGuard } from './authentication/auth.guard';
import { ChartComponent } from './chart/chart.component';
import { NgChartsModule } from 'ng2-charts';
import { GlobalErrorHandler } from './shared/global-error-handler';
import { environment } from 'src/environments/environment';



@NgModule({
  declarations: [
    AppComponent,
    ToastsContainer,
    AuthButtonComponent,
    ChartComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    AccountModule,
    TransactionModule,
    NgbToastModule,
    AuthModule.forRoot({
      domain: environment.domain,
      clientId: environment.clientId,
      audience: environment.audience
    }),
    NgChartsModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    AuthGuard,
    { provide: ErrorHandler, useClass: GlobalErrorHandler }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
