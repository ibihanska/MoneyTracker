import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Transaction } from './transaction.model';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  constructor(private http: HttpClient) { }
  readonly baseURL = environment.apiUrl + 'Transactions';

  get(): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(this.baseURL);
  }

  deleteTransaction(id: string) {
    return this.http.delete(`${this.baseURL}/${id}`);
  }

  postTransaction(formData: Transaction):Observable<string>{
    return this.http.post<string>(this.baseURL, formData);
  }
  putTransaction(updateFormData: Transaction) {
    return this.http.put(`${this.baseURL}/${updateFormData.id}`, updateFormData);
  }

  filterTransaction(accountName: string, tagName: string, date: string): Observable<Transaction[]> {
    const queryParams = new HttpParams().set('accountName', accountName).set('tagName', tagName).set('date', date);
    return this.http.get<Transaction[]>(this.baseURL, { params: queryParams })
  }
}
