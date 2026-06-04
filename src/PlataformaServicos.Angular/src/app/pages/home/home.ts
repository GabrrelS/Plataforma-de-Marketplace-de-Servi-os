import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ApiService, Prestador } from '../../core/services/api.service';
import { ServiceCardComponent } from '../../shared/components/service-card/service-card';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, ServiceCardComponent],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class HomeComponent implements OnInit {
  private api = inject(ApiService);

  prestadores = signal<Prestador[]>([]);
  loading = signal(true);

  categorias = [
    { icon: '💻', label: 'Desenvolvimento' },
    { icon: '🎨', label: 'Design' },
    { icon: '📈', label: 'Marketing Digital' },
    { icon: '✍️', label: 'Redação' },
    { icon: '🎬', label: 'Vídeo & Motion' },
    { icon: '🤖', label: 'IA & Automação' },
  ];

  ngOnInit(): void {
    this.api.listarPrestadores().subscribe({
      next: (data) => {
        this.prestadores.set(data.slice(0, 6));
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }
}
