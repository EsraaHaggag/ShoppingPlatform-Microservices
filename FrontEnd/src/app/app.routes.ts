import { Routes } from '@angular/router';

import { HomeComponent } from './features/home/pages/home/home.component';
import { ProductListComponent } from './features/product/pages/product-list/product-list.component';
import { ProductDetailsComponent } from './features/product/pages/product-details/product-details.component';
import { CartComponent } from './features/cart/pages/cart/cart.component';
import { CheckoutComponent } from './features/checkout/pages/checkout/checkout.component';
import { PaymentResultComponent } from './features/payment/pages/payment-result/payment-result.component';
export const routes: Routes = [

  {
    path: '',
    component: HomeComponent
  },

  {
    path: 'products',
    component: ProductListComponent
  },

  {
    path: 'products/:id',
    component: ProductDetailsComponent
  },

  {
    path: 'cart',
    component: CartComponent
  },

  {
  path: 'checkout',
  component: CheckoutComponent
  },

  {
  path: 'payment-result',
  component: PaymentResultComponent
  }

];

