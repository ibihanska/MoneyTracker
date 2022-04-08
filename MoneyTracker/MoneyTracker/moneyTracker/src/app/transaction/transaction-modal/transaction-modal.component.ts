import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Form, NgForm } from '@angular/forms';
import { AccountService } from 'src/lib/account.service';
import { Transaction } from 'src/lib/transaction.model';
import { TransactionService } from 'src/lib/transaction.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastService } from 'src/app/shared/toast.service';
import { Account } from 'src/lib/account.model';
import { AccountInfo } from 'src/lib/account-info';

@Component({
  selector: 'app-transaction-modal',
  templateUrl: './transaction-modal.component.html',
  styleUrls: ['./transaction-modal.component.css']
})
export class TransactionModalComponent implements OnInit {
  @Output() transactionSubmit = new EventEmitter<Transaction>();
  @Input() transaction: Transaction = new Transaction();
  @Input() income: Set<string> = new Set<string>();
  @Input() expense: Set<string> = new Set<string>();
  @ViewChild("form") form?: Form;
  acc: Account = new Account;
  constructor(public transactionService: TransactionService,
    public accountService: AccountService,
    public toastService: ToastService,
    public activeModal: NgbActiveModal) { }

  transactionType: string = 'Expense';
  accounts: AccountInfo[] = [];
  transactions: Transaction[] = [];
  newOption = '';

  ngOnInit(): void {
    this.getAccounts();
    this.type();
  }

  get isUpdateOperation() {
    return !!this.transaction.id;
  }

  close() {
    this.activeModal.close();
  }

  selectTransactionType(type: string) {
    this.transactionType = type;
  }

  onSubmit(form: NgForm) {
    this.isUpdateOperation ? this.updateTransaction(form) : this.postTransaction(form);
  }

  updateTransaction(form: NgForm) {
    this.transactionService.putTransaction(this.transaction).subscribe(
      res => {
        this.transactionSubmit.emit(this.transaction);
        this.resetForm(form);
        this.toastService.show('Updated successfully');
      }
    );
  }

  getAccountName(id: string) {
    return this.accounts.find(x => x.id == id)?.name;
  }

  postTransaction(form: NgForm) {
    if (this.transactionType != 'Income') {
      var accountName = this.getAccountName(this.transaction.fromAccountId);
      if (typeof accountName === 'string') {
        this.transaction.fromAccountName = accountName;
      }
    }
    if (this.transactionType != 'Expense') {
      var toaccountName = this.getAccountName(this.transaction.toAccountId);
      if (typeof toaccountName === 'string') {
        this.transaction.toAccountName = toaccountName;
      }
    }
    this.transaction.transactionType = this.transactionType;
    this.transactionService.postTransaction(this.transaction).subscribe(
      res => {
        this.transaction.id = res;
        this.transactionSubmit.emit(this.transaction);
        this.resetForm(form);
        this.toastService.show('Transaction added', { classname: 'bg-success text-light', delay: 10000 });
      }
    );
  }

  resetForm(form: NgForm) {
    form.form.reset();
  }

  addExpenseTag() {
    this.expense.add(this.newOption);
  }

  addIncomeTag() {
    this.income.add(this.newOption);
  }

  getAccounts() {
    this.accountService.getAccountInfo().subscribe
      (res => this.accounts = res);
  }

  type() {
    if (this.transaction.fromAccountId == null) {
      this.transactionType = "Income";
    }
    else if (this.transaction.toAccountId == null) {
      this.transactionType = "Expense";
    }
    else {
      this.transactionType = "Transfer";
    }
  }

  setTransactionDate(date: string) {
    this.transaction.transactionDate = new Date(date);
  }
}


