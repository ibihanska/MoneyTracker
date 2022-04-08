import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountListComponent } from './account/account-list/account-list.component';
import { TransactionListComponent } from './transaction/transaction-list/transaction-list.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthHttpInterceptor } from './authentication/interceptor.service';
import { AuthGuard } from './authentication/auth.guard';
import { ChartComponent } from './chart/chart.component';


const routes: Routes = [

  {
    path: 'accounts',
    component: AccountListComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard]
  },
  {
    path: 'transactions',
    component: TransactionListComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard]
  },
  {
    path: 'reports',
    component: ChartComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard]
  },
  {
    path: '**', pathMatch: 'full',
    redirectTo:'/'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHttpInterceptor,
      multi: true
    }
  ]
})
export class AppRoutingModule { }
