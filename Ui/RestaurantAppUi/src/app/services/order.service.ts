import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private apiUrl= 'https://localhost:7269/api/Order';

  constructor(private http: HttpClient) { }

  getOrderSummary(orderId: number): Observable<any>{
    return this.http.get('${this.apiUrl}/${orderId}/summary');
  }
}
