import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ApiService, Prestador } from '../../core/services/api.service';
import { ServiceCardComponent } from '../../shared/components/service-card/service-card';

@Component({
  selector: 'app-explorar',
  standalone: true,
  imports: [FormsModule, ServiceCardComponent],
  templateUrl: './explorar.html',
  styleUrl: './explorar.css',
})
export class ExplorarComponent implements OnInit {
  private api = inject(ApiService);

  todos = signal<Prestador[]>([]);
  loading = signal(true);
  busca = signal('');
  especialidade = signal('');

  especialidades = ['Design', 'Desenvolvimento', 'Marketing', 'Redação', 'IA & Automação', 'Vídeo'];

  filtrados = computed(() => {
    const termo = this.busca().toLowerCase();
    const esp   = this.especialidade().toLowerCase();
    return this.todos().filter((p) => {
      const matchBusca = !termo ||
        p.nome.toLowerCase().includes(termo) ||
        (p.especialidade ?? '').toLowerCase().includes(termo) ||
        (p.descricao ?? '').toLowerCase().includes(termo);
      const matchEsp = !esp || (p.especialidade ?? '').toLowerCase().includes(esp);
      return matchBusca && matchEsp;
    });
  });

  ngOnInit(): void {
    this.api.listarPrestadores().subscribe({
      next: (data) => { this.todos.set(data); this.loading.set(false); },
      error: () => this.loading.set(false),
    });
  }

  setBusca(v: string): void { this.busca.set(v); }
  setEsp(v: string): void   { this.especialidade.set(v); }
}
