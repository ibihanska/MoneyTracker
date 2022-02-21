import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountListComponent } from './account/account-list/account-list.component';
import { TransactionListComponent } from './transaction/transaction-list/transaction-list.component';

const routes: Routes = [

  {
    path: 'account',
    component: AccountListComponent
  },
  {
    path: 'transaction',
    component: TransactionListComponent
  },
  {
    path: '**', pathMatch: 'full',
    component: AccountListComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
