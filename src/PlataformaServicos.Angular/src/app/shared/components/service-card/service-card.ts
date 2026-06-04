import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Prestador } from '../../../core/services/api.service';

@Component({
  selector: 'app-service-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './service-card.html',
  styleUrl: './service-card.css',
})
export class ServiceCardComponent {
  @Input() prestador!: Prestador;

  get iniciais(): string {
    return (this.prestador?.nome ?? '??')
      .split(' ')
      .map((n) => n[0])
      .slice(0, 2)
      .join('')
      .toUpperCase();
  }

  get estrelas(): number[] {
    return Array(5).fill(0);
  }

  get avaliacaoArredondada(): number {
    return Math.round((this.prestador?.avaliacao ?? 0) * 10) / 10;
  }
}
