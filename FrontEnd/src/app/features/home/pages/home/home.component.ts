import { Component } from '@angular/core';
import {RouterLink,RouterLinkActive} from '@angular/router';
@Component({
  imports: [RouterLink, RouterLinkActive],
  selector: 'app-home',
  styleUrl: './home.component.css',
  templateUrl: './home.component.html',
})
export class HomeComponent {}
