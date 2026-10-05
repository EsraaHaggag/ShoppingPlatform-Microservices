import {Component,OnInit,inject,input,signal} from '@angular/core';

import { DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import { Product } from '../../models/product.interface';
import { CartService } from '../../../cart/services/cart.service';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [DecimalPipe, RouterLink],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.css'
})
export class ProductCardComponent {

  private readonly cartService =
    inject(CartService);

  product = input.required<Product>();

  selectedImage = signal('');

  isAddingToCart = signal(false);

  ngOnInit(): void {

    const images = this.product().images;

    if (images.length > 0) {

      const primaryImage =
        images.find(image => image.isPrimary);

      this.selectedImage.set(
        primaryImage?.imageUrl ??
        images[0].imageUrl
      );
    }
  }

  selectImage(imageUrl: string): void {
    this.selectedImage.set(imageUrl);
  }


  addToCart(): void {

    if (this.product().stockQuantity <= 0)
      return;

    this.isAddingToCart.set(true);

    this.cartService
      .addToCart(this.product().id, 1)
      .subscribe({
        next: () => {
          this.isAddingToCart.set(false);
        },

        error: (error) => {
          console.error(
            'Failed to add product to cart:',
            error
          );
          this.isAddingToCart.set(false);

        }
      });
  }
}