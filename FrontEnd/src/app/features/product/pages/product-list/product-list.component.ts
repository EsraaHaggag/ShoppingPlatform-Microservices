import {Component,OnInit,inject,signal} from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.interface';
import { ProductCardComponent } from '../../components/product-card/product-card.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [ProductCardComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit {

  private readonly productService =
    inject(ProductService);

  products = signal<Product[]>([]);

  search = signal('');

  minPrice = signal<number | undefined>(undefined);

  maxPrice = signal<number | undefined>(undefined);

  sortBy = signal('');

  sortDirection = signal('');

  currentPage = signal(1);

  totalPages = signal(1);

  totalCount = signal(0);

  pageSize = 6;

  isLoading = signal(false);

  ngOnInit(): void {
    this.loadProducts();
  }


  loadProducts(): void {
    this.isLoading.set(true);
    this.productService.getProducts(
      this.search(),
      this.minPrice(),
      this.maxPrice(),
      this.sortBy(),
      this.sortDirection(),
      this.currentPage(),
      this.pageSize
    ).subscribe({
      next: (response) => {
        this.products.set(response.data);
        this.totalPages.set(
          response.totalPages
        );
        this.totalCount.set(
          response.totalCount
        );
        this.isLoading.set(false);
      },

      error: (error) => {
        console.error(
          'Failed to load products:',
          error );
        this.isLoading.set(false);
      }

    });
  }


onSearchInput(event: Event): void {
  const input = event.target as HTMLInputElement;

  this.search.set(input.value);
}

onSearch(): void {
  this.currentPage.set(1);
  this.loadProducts();
}

  onSortChange(
    event: Event
  ): void {
    const value =
      (event.target as HTMLSelectElement).value;
    if (!value) {
      this.sortBy.set('');
      this.sortDirection.set('');
    } else {

      const [sortBy, sortDirection] =
        value.split('-');

      this.sortBy.set(sortBy);
      this.sortDirection.set(sortDirection);
    }

    this.currentPage.set(1);
    this.loadProducts();
  }


  nextPage(): void {
    if (
      this.currentPage() <this.totalPages()
    ) {
      this.currentPage.update(
        page => page + 1
      );
      this.loadProducts();
    }
  }


  previousPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.update(
        page => page - 1
      );
      this.loadProducts();
    }
  }
}

