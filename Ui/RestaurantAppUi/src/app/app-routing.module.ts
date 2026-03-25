import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderSummaryComponent } from './order-summary/order-summary.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'order-summary', component: OrderSummaryComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { 
}
