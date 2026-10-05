import {Component,OnInit,inject,signal} from '@angular/core';

import { DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import { CartService } from '../../services/cart.service';
import { Cart } from '../../models/cart.interface';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [DecimalPipe, RouterLink],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {

  private readonly cartService =
    inject(CartService);

  cart = signal<Cart | null>(null);

  isLoading = signal(true);

  errorMessage = signal('');
  ngOnInit(): void {
    this.loadCart();
  }


  loadCart(): void {

    this.isLoading.set(true);

    this.cartService
      .getCart()
      .subscribe({

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
            'Failed to load cart.'
          );

          this.isLoading.set(false);

        }

      });
  }


  getCartTotal(): number {

    const currentCart =
      this.cart();

    if (!currentCart)
      return 0;

    return currentCart.items.reduce(
      (total, item) =>
        total + item.totalPrice,
      0
    );
  }


  increaseQuantity(
    productId: string,
    currentQuantity: number
  ): void {

    this.updateQuantity(
      productId,
      currentQuantity + 1
    );
  }


  decreaseQuantity(
    productId: string,
    currentQuantity: number
  ): void {

    if (currentQuantity <= 1) {
      this.removeItem(productId);
      return;
    }

    this.updateQuantity(
      productId,
      currentQuantity - 1
    );
  }


  updateQuantity(
    productId: string,
    quantity: number
  ): void {

    this.cartService
      .updateQuantity(
        productId,
        quantity
      )
      .subscribe({
        next: () => {
          this.loadCart();
        },
        error: (error) => {
          console.error(
            'Failed to update quantity:',
            error
          );
        }
      });
  }


  removeItem(productId: string): void {

    this.cartService
      .removeItem(productId)
      .subscribe({
        next: () => {
          this.loadCart();
        },
        error: (error) => {
          console.error(
            'Failed to remove item:',
            error
          );
        }
      });
  }


  clearCart(): void {

    this.cartService
      .clearCart()
      .subscribe({
        next: () => {
          this.cart.set(null);
        },
        error: (error) => {
          console.error(
            'Failed to clear cart:',
            error
          );
        }
      });
  }
}