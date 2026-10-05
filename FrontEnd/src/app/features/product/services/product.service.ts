import { Service, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product.interface';
import { ProductResponse } from '../models/product-response.interface';

@Service()
export class ProductService {

  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'https://localhost:7296/api/products';

  getProducts(
    search?: string,
    minPrice?: number,
    maxPrice?: number,
    sortBy?: string,
    sortDirection?: string,
    pageNumber: number = 1,
    pageSize: number = 12
  ): Observable<ProductResponse> {

    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (search?.trim()) {
      params = params.set(
        'search',
        search.trim()
      );
    }

    if (minPrice !== undefined) {
      params = params.set(
        'minPrice',
        minPrice
      );
    }

    if (maxPrice !== undefined) {
      params = params.set(
        'maxPrice',
        maxPrice
      );
    }

    if (sortBy) {
      params = params.set(
        'sortBy',
        sortBy
      );
    }

    if (sortDirection) {
      params = params.set(
        'sortDirection',
        sortDirection
      );
    }

    return this.http.get<ProductResponse>(
      this.apiUrl,
      { params }
    );
  }
    getProductById( id: string ): Observable<Product>
     { return this.http.get<Product>
      ( `${this.apiUrl}/${id}` );
     }
}

