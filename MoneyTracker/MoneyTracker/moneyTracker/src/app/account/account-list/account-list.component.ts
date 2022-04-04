import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountType } from 'src/lib/account-type';
import { Account } from 'src/lib/account.model';
import { AccountService } from 'src/lib/account.service';
import { ToastService } from 'src/app/shared/toast.service';
import { AccountModalComponent } from '../account-modal/account-modal.component';



@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.css']
})
export class AccountListComponent implements OnInit {
  constructor(public service: AccountService,
    public toastService: ToastService,
    private modalService: NgbModal) { }
  totalsum: number = 0;
  accounts: Account[] = [];
  updateFormData: Account = new Account();

  ngOnInit(): void {
    this.getAccounts();
  }

  populateForm(selectedRecord: Account) {
    this.updateFormData = Object.assign({}, selectedRecord);
  }

  showSum() {
    this.totalsum = 0;
    this.accounts.forEach(t => this.totalsum += t.balance);
  }

  onDelete(id: string) {
    if (confirm('Are you sure to delete this account?')) {
      this.service.deleteAccount(id)
        .subscribe(
          res => {
            this.accounts = this.accounts.filter(item => item.id !== id);
            this.toastService.show("Deleted successfully", { classname: 'bg-danger text-light', delay: 15000 });
          }
        )
    }
  }

  getAccountName(index: number): string {
    return AccountType[index];
  }
  getAccounts() {
    this.service.getAccountsList().subscribe(
      res => this.accounts = res
    );
  }

  openAccountModal(account?: Account) {
    let ref = this.modalService.open(AccountModalComponent, {
      ariaLabelledBy: 'modal-basic-title',
    });
    const component = ref.componentInstance as AccountModalComponent;
    component.account = account ? { ...account } : ({} as Account);
    component.accountSubmit.subscribe((a) => {
      component.close();
      let ac = { ...a };
      const changedAccount = !!this.accounts.find((x) => x.id == a.id);
      this.accounts = changedAccount
        ? this.accounts.map((x) => {
          return x.id == a.id ? { ...x, ...a } : x;
        })
        : [...this.accounts, ac];
    }
    );
  }
}
