export interface ProductImage {
  id: string;
  imageUrl: string;
  displayOrder: number;
  isPrimary: boolean;
}

export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
  images: ProductImage[];
}


