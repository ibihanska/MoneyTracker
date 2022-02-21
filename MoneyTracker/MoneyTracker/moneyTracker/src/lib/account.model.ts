import { AccountType } from "./account-type";

export class Account {
    id:string='';
    name:string='';
    balance:number=0;
    accountType: AccountType=AccountType.Cash;
}
