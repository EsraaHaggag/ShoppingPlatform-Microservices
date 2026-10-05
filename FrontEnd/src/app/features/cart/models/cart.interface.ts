export interface CartItem {
  productId: string;
  productName: string;
  productImageUrl: string | null;
  unitPrice: number;
  quantity: number;
  totalPrice: number;
}

export interface Cart {
  id: string;
  customerId: string;
  items: CartItem[];
}