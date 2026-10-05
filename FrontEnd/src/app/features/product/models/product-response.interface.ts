import { Product } from './product.interface';

export interface ProductResponse {
  data: Product[];
  currentPage: number;
  totalPages: number;
  totalCount: number;
  meta: unknown;
  pageSize: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  messages: string[];
  succeeded: boolean;
}