import {Injectable,inject,signal} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { Cart } from '../models/cart.interface';
import { AddCartItemRequest } from '../models/add-cart-item.interface';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'https://localhost:7207/api/cart';

  cart = signal<Cart | null>(null);

  cartCount = signal(0);

  getCart(): Observable<Cart> {

    return this.http
      .get<Cart>(this.apiUrl)
      .pipe(
        tap(cart => {
          this.cart.set(cart);
          this.updateCartCount(cart);
        })
      );
  }

  addToCart(
    productId: string,
    quantity: number = 1
  ): Observable<void> {

    const request: AddCartItemRequest = {
      productId,
      quantity
    };

    return this.http
      .post<void>(
        this.apiUrl,
        request
      )
      .pipe(
        tap(() => {
          this.getCart().subscribe();
        })
      );
  }

  updateQuantity(
    productId: string,
    quantity: number
  ): Observable<void> {

    return this.http
      .put<void>(
        `${this.apiUrl}/items/${productId}?quantity=${quantity}`,
        {}
      )
      .pipe(
        tap(() => {
          this.getCart().subscribe();
        })
      );
  }

  removeItem(
    productId: string
  ): Observable<void> {

    return this.http
      .delete<void>(
        `${this.apiUrl}/items/${productId}`
      )
      .pipe(
        tap(() => {
          this.getCart().subscribe();
        })
      );
  }

  clearCart(): Observable<void> {

    return this.http
      .delete<void>(this.apiUrl)
      .pipe(
        tap(() => {
          this.cart.set(null);
          this.cartCount.set(0);
        })
      );
  }

  private updateCartCount(cart: Cart): void {

    const count = cart.items.reduce(
      (total, item) =>
        total + item.quantity,
      0
    );

    this.cartCount.set(count);
  }
}