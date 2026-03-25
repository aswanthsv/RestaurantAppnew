import { Component, OnInit } from '@angular/core';

import { TableService } from 'src/app/services/table.service';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  tables: any[] = [];

  constructor(private tableService: TableService) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard() {
    this.tableService.getDashboardOverview().subscribe({
      next: (data : any[]) => this.tables = data,
      error: (err : any) => {
        console.error(err);
        alert('Error loading dashboard');
      }
    });
  }
}
