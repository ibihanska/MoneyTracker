export class Transaction {
    id:string='';
    fromAccountId:any=null;
    fromAccountName:string='';
    toAccountId:any=null;
    toAccountName:string='';
    transactionDate:Date= new Date();
    tagName:string='';
    transactionType:string='';
    amount:number=0;
    note:string='';
}