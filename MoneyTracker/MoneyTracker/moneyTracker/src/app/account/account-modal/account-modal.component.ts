import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Form, NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Account } from 'src/lib/account.model';
import { AccountService } from 'src/lib/account.service';
import { ToastService } from 'src/app/shared/toast.service';

@Component({
  selector: 'app-account-modal',
  templateUrl: './account-modal.component.html',
  styleUrls: ['./account-modal.component.css']
})
export class AccountModalComponent implements OnInit {
  @Output() accountSubmit = new EventEmitter<Account>();
  @Input() account: Account = new Account();
  @ViewChild("form") form?: Form;
  constructor(public service: AccountService,
    public toastService: ToastService,
    public activeModal: NgbActiveModal
  ) { }

  accounts: Account[] = [];
  accountId:string='';

  ngOnInit(): void {
    this.getAccounts();
  }

  get isUpdateOperation() {
    return !!this.account.id;
  }

  onSubmit(form: NgForm) {
    if (this.isUpdateOperation) {
      this.updateAccount(form);
    }
    else {
      this.postAccount(form);
    }
  }

  getAccounts() {
    this.service.getAccountsList().subscribe(
      res => this.accounts = res
    );
  }

  postAccount(form: NgForm) {
    this.service.postAccount(this.account).subscribe(
      res => {
        this.account.id=res;
        this.accountSubmit.emit(this.account);
        form.form.reset();
        this.toastService.show('Account added successfully!', { classname: 'bg-success text-light', delay: 10000 });
      },
      err => {
        console.log(err);
      }
    );
  }

  updateAccount(form: NgForm) {
    this.service.putAccount(this.account).subscribe(
      res => {
        this.accountSubmit.emit(this.account);
        form.form.reset();
        this.toastService.show('Account updated successfully!');
      },
      err => {
        console.log(err);
      }
    );
  }
}
