import {Component,OnInit,inject,signal} from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-payment-result',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './payment-result.component.html',
  styleUrl: './payment-result.component.css'
})
export class PaymentResultComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);

  isSuccess = signal(false);

  ngOnInit(): void {
    const success =
      this.route.snapshot.queryParamMap.get('success');

    this.isSuccess.set(success === 'true');
  }
}