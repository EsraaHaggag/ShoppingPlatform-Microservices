import {
  Component,
  OnInit,
  inject
} from '@angular/core';

import {
  RouterLink,
  RouterLinkActive
} from '@angular/router';

import { CartService } from '../../../features/cart/services/cart.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {

  private readonly cartService =
    inject(CartService);

  cartCount = this.cartService.cartCount;

  ngOnInit(): void {
    this.cartService.getCart().subscribe({
      error: (error) => {
        console.error(
          'Failed to load cart:',
          error
        );
      }
    });
  }
}