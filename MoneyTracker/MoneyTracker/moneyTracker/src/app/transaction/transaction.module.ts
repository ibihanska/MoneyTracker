import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TransactionModalComponent } from './transaction-modal/transaction-modal.component';
import { FormsModule } from '@angular/forms';
import { TransactionListComponent } from './transaction-list/transaction-list.component';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from '../app-routing.module';


@NgModule({
  declarations: [
    TransactionModalComponent,
    TransactionListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbPaginationModule,
    AppRoutingModule
  ],
  exports: [
    TransactionListComponent
  ]

})
export class TransactionModule { }
