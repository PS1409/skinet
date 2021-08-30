import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent implements OnInit {

  baseUrl: string = environment.apiUrl;
  validationErrors: any;
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  onGet404Error(): void {
     this.http.get(this.baseUrl + 'products/42').subscribe(response => {
       console.log(response);
     }, error => {
       console.log(error);
     });
  }

  onGet500Error(): void {
     this.http.get(this.baseUrl + 'bug/servererror').subscribe(response => {
       console.log(response);
     }, error => {
       console.log(error);
     });
  }

  onGet400Error(): void {
     this.http.get(this.baseUrl + 'bug/badrequest').subscribe(response => {
       console.log(response);
     }, error => {
       console.log(error);
     });
  }

  onGet400ValidationError(): void {
     this.http.get(this.baseUrl + 'products/fortytwo').subscribe(response => {
       console.log(response);
     }, error => {
       console.log(error);
       this.validationErrors = error.errors;
     });
  }
}
