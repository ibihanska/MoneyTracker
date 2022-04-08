import { ErrorHandler, Injectable } from '@angular/core';
import { ToastService } from './toast.service';

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandler implements ErrorHandler {
  constructor(public toastService: ToastService) { }
  handleError(err: any): void {
    console.log(err);
    if (err.status == '404') {
      this.toastService.show('Not Found', { classname: 'bg-danger text-light', delay: 15000, headertext: "Error:" + err.status });
    }
    else {
      this.toastService.show(
        err.error.detail || err.name,
        {
          classname: 'bg-danger text-light',
          delay: 15000,
          headertext: (err.error.status == undefined) ? "" : (err.error.status + " " + err.error.title)
        });
    }
  }
}