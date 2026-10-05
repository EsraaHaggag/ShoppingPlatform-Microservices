import {
  Component,
  OnInit,
  inject,
  signal
} from '@angular/core';

import { DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CheckoutService } from '../../services/checkout.service';
import { CartService } from '../../../cart/services/cart.service';
import { Cart } from '../../../cart/models/cart.interface';
import { CheckoutResponse, StockReservationItemStatus } from '../../models/checkout-response.interface';
@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [DecimalPipe, RouterLink],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent implements OnInit {
  private readonly cartService = inject(CartService);
  private readonly checkoutService = inject(CheckoutService);

  isProcessing = signal(false);
  cart = signal<Cart | null>(null);
  isLoading = signal(true);
  errorMessage = signal('');
  checkoutIssues = signal<StockReservationItemStatus[]>([]);

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.isLoading.set(true);

    this.cartService.getCart().subscribe({
      next: (cart) => {
        this.cart.set(cart);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error(
          'Failed to load cart:',
          error
        );

        this.errorMessage.set(
          'Failed to load your cart.'
        );

        this.isLoading.set(false);
      }
    });
  }

  getCartTotal(): number {
    const currentCart = this.cart();

    if (!currentCart)
      return 0;

    return currentCart.items.reduce(
      (total, item) => total + item.totalPrice,
      0
    );
  }

  proceedToPayment(): void {
    this.isProcessing.set(true);
    this.errorMessage.set('');
    this.checkoutIssues.set([]);

    this.checkoutService.checkout().subscribe({
      next: (response) => {
        const invalidItems = response.itemsStatus.filter(
          item => item.status !== 0
        );

        if (invalidItems.length > 0) {
          this.checkoutIssues.set(invalidItems);
          this.isProcessing.set(false);
          return;
        }

        if (response.success && response.payment?.checkoutUrl) {
          window.location.href = response.payment.checkoutUrl;
          return;
        }

        this.errorMessage.set(
          response.message ?? 'Unable to proceed to payment.'
        );
        this.isProcessing.set(false);
      },
      error: (error) => {
        console.error('Checkout failed:', error);
        this.errorMessage.set('Unable to proceed to payment.');
        this.isProcessing.set(false);
      }
    });
  }

  getProductName(productId: string): string {
    const item = this.cart()?.items.find(
      item => item.productId === productId
    );
    return item?.productName ?? 'Product';
  }

  getIssueMessage(item: StockReservationItemStatus): string {
    switch (item.status) {
      case 1:
        return `Only ${item.available} item(s) are available, but you requested ${item.requested}.`;
      case 2:
        return 'This product is currently out of stock.';
      case 3:
        return `The price has changed from $${item.requestedPrice} to $${item.currentPrice}.`;
      case 4:
        return 'This product is no longer available.';
      default:
        return 'There is an issue with this product.';
    }
  }


}