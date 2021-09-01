import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;
  constructor(private spinService: NgxSpinnerService) { }

  busy(): void {
      this.busyRequestCount++;
      this.spinService.show(undefined, {
        type : 'timer',
        bdColor: 'rgba(255,255,255, 0.7)',
        color: '#333333',
        size: 'default'
      });
  }

 idle(): void {
   this.busyRequestCount--;
   if(this.busyRequestCount <= 0){
     this.spinService.hide();
   }
 }
}
