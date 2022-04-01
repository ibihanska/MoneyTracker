import { Injectable } from '@angular/core';
import { Account } from './account.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { AccountInfo } from './account-info';


@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }
  readonly baseURL = environment.apiUrl + 'Accounts';

  postAccount(formData: Account): Observable<string> {
    return this.http.post<string>(this.baseURL, formData);
  }
  putAccount(updateFormData: Account) {
    return this.http.put(`${this.baseURL}/${updateFormData.id}`, updateFormData);
  }

  deleteAccount(id: string) {
    return this.http.delete(`${this.baseURL}/${id}`);
  }

  getAccountInfo(): Observable<AccountInfo[]> {
    return this.http.get<AccountInfo[]>(this.baseURL);
  }
  getAccountsList(): Observable<Account[]> {
    return this.http.get<Account[]>(this.baseURL);
  }
}
