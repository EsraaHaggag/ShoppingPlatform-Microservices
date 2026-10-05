import {Component,OnInit,inject,signal} from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.interface';
import { CartService } from '../../../cart/services/cart.service';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [DecimalPipe],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.css'
})
export class ProductDetailsComponent implements OnInit {
  private readonly productService = inject(ProductService);
  private readonly route = inject(ActivatedRoute);
  private readonly cartService = inject(CartService);

  product = signal<Product | null>(null);
  selectedImage = signal('');
  isLoading = signal(true);
  errorMessage = signal('');
  quantity = signal(1);
  isAddingToCart = signal(false);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.errorMessage.set('Product ID was not found.');
      this.isLoading.set(false);
      return;
    }

    this.loadProduct(id);
  }

  loadProduct(id: string): void {
    this.isLoading.set(true);

    this.productService.getProductById(id).subscribe({
      next: (product) => {
        this.product.set(product);

        const images = product.images;

        if (images.length > 0) {
          const primaryImage = images.find(
            image => image.isPrimary
          );

          this.selectedImage.set(
            primaryImage?.imageUrl ?? images[0].imageUrl
          );
        }

        this.quantity.set(1);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error(
          'Failed to load product:',
          error
        );

        this.errorMessage.set(
          'Failed to load product.'
        );
        this.isLoading.set(false);
      }
    });
  }

  selectImage(imageUrl: string): void {
    this.selectedImage.set(imageUrl);
  }

  increaseQuantity(): void {
    const currentProduct = this.product();

    if (!currentProduct)
      return;

    if (this.quantity() < currentProduct.stockQuantity) {
      this.quantity.update(value => value + 1);
    }
  }

  decreaseQuantity(): void {
    if (this.quantity() > 1) {
      this.quantity.update(value => value - 1);
    }
  }

  addToCart(): void {
    const currentProduct = this.product();

    if (!currentProduct ||
      currentProduct.stockQuantity <= 0)
      return;

    this.isAddingToCart.set(true);

    this.cartService
      .addToCart(
        currentProduct.id,
        this.quantity()
      )
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