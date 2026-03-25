import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TableService {
  private apiUrl = 'https://localhost:7269/api/Table';

  constructor(private http: HttpClient) {}

  getDashboardOverview(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/dashboard`);
  }
}
