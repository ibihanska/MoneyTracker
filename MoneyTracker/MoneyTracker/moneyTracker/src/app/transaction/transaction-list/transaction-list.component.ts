import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastService } from 'src/app/shared/toast.service';
import { Transaction } from 'src/lib/transaction.model';
import { TransactionService } from 'src/lib/transaction.service';
import { TransactionModalComponent } from '../transaction-modal/transaction-modal.component';

@Component({
  selector: 'app-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css']
})
export class TransactionListComponent implements OnInit {

  constructor(public service: TransactionService,
    public toastService: ToastService,
    private modalService: NgbModal) { }

  public page = 1;
  public pageSize = 10;
  incomeTags: Transaction[] = [];
  expenseTags: Transaction[] = [];
  income: Set<string> = new Set<string>();
  expense: Set<string> = new Set<string>();
  transactions: Transaction[] = [];
  updateFormData: Transaction = new Transaction();
  accountName: string = "";
  tagName: string = "";
  date: string = "";

  ngOnInit(): void {
    this.getFilterTransactions();
  }
  onDelete(id: string) {
    if (confirm('Are you sure to delete this transaction?')) {
      this.service.deleteTransaction(id)
        .subscribe(
          res => {
            this.transactions = this.transactions.filter(item => item.id !== id);
            this.toastService.show("Deleted successfully", { classname: 'bg-danger text-light', delay: 15000 });
          }
        )
    }
  }
  populateForm(selectedRecord: Transaction) {
    this.updateFormData = Object.assign({}, selectedRecord);
  }
  onSubmit(form: NgForm) {
    this.getFilterTransactions();
  }

  getFilterTransactions() {
    this.service.filterTransaction(this.accountName, this.tagName, this.date).subscribe
      (res => {
        this.transactions = res.map((x) => {
          return { ...x, transactionDate: new Date(x.transactionDate) };
        })
      });
  }

  getIncomeTags() {
    this.incomeTags = this.transactions.filter(x => x.transactionType == "Income");
    this.incomeTags.map(t => this.income.add(t.tagName));
    return new Set(this.income);
  }

  getExpenseTags() {
    this.expenseTags = this.transactions.filter(x => x.transactionType == "Expense");
    this.expenseTags.map(t => this.expense.add(t.tagName));
    return new Set(this.expense);
  }

  reset() {
    this.accountName = '';
    this.tagName = '';
    this.date = '';
  }

  openTransactionModal(transaction?: Transaction) {
    let ref = this.modalService.open(TransactionModalComponent, {
      ariaLabelledBy: 'modal-basic-title',
    });
    const component = ref.componentInstance as TransactionModalComponent;
    component.expense = this.getExpenseTags();
    component.income = this.getIncomeTags();
    component.transaction = transaction ? { ...transaction } : ({} as Transaction);
    component.transactionSubmit.subscribe((a) => {
      component.close();
      let tr = { ...a };
      const changedTransaction = !!this.transactions.find((x) => x.id == a.id);
      this.transactions = changedTransaction
        ? this.transactions.map((x) => {
          return x.id == a.id ? { ...x, ...a } : x;
        })
        : [...this.transactions, tr];
    });
  }

}
