import { Component, OnInit } from '@angular/core';
import { ChartOptions, ChartType } from 'chart.js';
import { Transaction } from 'src/lib/transaction.model';
import { TransactionService } from 'src/lib/transaction.service';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {
  constructor(public service: TransactionService) { }

  transactions?: Transaction[];
  income: number[] = [];
  expense: number[] = [];
  incomeTransactions: Transaction[] = [];
  expenseTransactions: Transaction[] = [];
  incomeMonthT: Transaction[] = [];
  expenseMonthT: Transaction[] = [];
  tags: Set<string> = new Set<string>();
  year: number = 2022;
  tag: string = 'Products';
  option: string = 'Income&Expense';

  ngOnInit(): void {
    this.getTransactions();

  }

  onChangeOption($event: any) {
    this.option = $event;
    if (this.option == 'Income&Expense') {
      this.getYearReport(this.year);
    }
    else {
      this.getYearReportwithTag(this.year, this.tag);
    }
  }

  onChange($event: any) {
    this.tag = $event;
    this.getYearReportwithTag(this.year, this.tag);
  }

  public barChartOptions: ChartOptions = {
    responsive: true,
  };
  public barChartLabels: string[] = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartData: any[] | undefined;

  getYearReport(year: number) {
    this.year = year;
    let totalSum: number = 0;
    let chartData: any[];
    chartData = new Array<any>();
    this.expenseTransactions = this.transactions!.filter(x => x.transactionType == "Expense" && x.transactionDate.getFullYear() == year);
    this.incomeTransactions = this.transactions!.filter(x => x.transactionType == "Income" && x.transactionDate.getFullYear() == year);
    for (var counter: number = 0; counter < 12; counter++) {
      this.incomeMonthT = this.incomeTransactions.filter(t => t.transactionDate.getMonth() == counter);
      this.incomeMonthT.forEach(t => totalSum += t.amount);
      this.income.push(totalSum);
      totalSum = 0;
      this.expenseMonthT = this.expenseTransactions.filter(t => t.transactionDate.getMonth() == counter);
      this.expenseMonthT.forEach(t => totalSum += t.amount);
      this.expense.push(totalSum);
      totalSum = 0;
    }
    chartData.push({
      data: this.expense,
      label: 'Expense'
    });
    chartData.push({
      data: this.income,
      label: 'Income'
    });
    this.barChartData = chartData;
    this.expense = [];
    this.income = [];
  }

  getYearReportwithTag(year: number, tag: string) {
    this.tag = tag;
    this.year = year;
    let totalSum: number = 0;
    let chartData: any[];
    chartData = new Array<any>();
    this.expenseTransactions = this.transactions!.filter(x => x.transactionDate.getFullYear() == year);
    for (var counter: number = 0; counter < 12; counter++) {
      this.expenseMonthT = this.expenseTransactions.filter(t => t.transactionDate.getMonth() == counter && t.tagName == tag);
      this.expenseMonthT.forEach(t => totalSum += t.amount);
      this.expense.push(totalSum);
      totalSum = 0;
    }
    chartData.push({
      data: this.expense,
      label: tag
    });
    this.barChartData = chartData;
    this.expense = [];
  }

  getTags(year: number) {
    this.expenseTransactions = this.transactions!.filter(x => x.transactionDate.getFullYear() == year && x.transactionType != "Transfer");
    this.expenseTransactions.map(t => this.tags.add(t.tagName));
    return new Set(this.tags);
  }

  getTransactions() {
    this.service.get().subscribe
      (res => {
        this.transactions = res.map((x) => {
          return { ...x, transactionDate: new Date(x.transactionDate) };
        });
        this.getYearReport(2022);
      });
  }
}
