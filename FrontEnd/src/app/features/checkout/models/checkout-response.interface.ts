export interface CheckoutResponse {
  success: boolean;
  payment: PaymentResponse | null;
  itemsStatus: StockReservationItemStatus[];
  message: string | null;
}

export interface PaymentResponse {
  paymentId: string;
  checkoutUrl: string;
}

export interface StockReservationItemStatus {
  productId: string;
  requested: number;
  available: number;
  requestedPrice: number | null;
  currentPrice: number | null;
  status: number;
}