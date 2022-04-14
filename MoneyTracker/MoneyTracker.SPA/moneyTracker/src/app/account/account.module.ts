import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { AccountListComponent } from './account-list/account-list.component';
import { AppRoutingModule } from '../app-routing.module';
import { AccountModalComponent } from './account-modal/account-modal.component';


@NgModule({
  declarations: [
    AccountModalComponent,
    AccountListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    AppRoutingModule
  ],
  exports: [
    AccountListComponent
  ]
})
export class AccountModule { }
