import { Component } from '@angular/core';
import { OrderService } from '../services/order.service';

@Component({
  selector: 'app-order-summary',
  templateUrl: './order-summary.component.html',
  styleUrls: ['./order-summary.component.scss']
})
export class OrderSummaryComponent {
  orderId: number = 0;
  orderSummary: any;

  constructor(private orderService: OrderService) {}

  loadSummary() {
    if (this.orderId <= 0) {
      alert('Please enter a valid Order ID');
      return;
    }

    this.orderService.getOrderSummary(this.orderId).subscribe({
      next: (res) => {
        this.orderSummary = res;
      },
      error: (err) => {
        console.error(err);
        alert('Order not found or server error!');
      }
    });
  }
}
