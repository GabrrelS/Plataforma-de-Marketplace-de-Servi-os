import { Component, OnInit, inject, signal } from '@angular/core';
import { ApiService, Proposta, Prestador, Cliente } from '../../core/services/api.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class DashboardComponent implements OnInit {
  private api = inject(ApiService);

  propostas  = signal<Proposta[]>([]);
  prestadores = signal<Prestador[]>([]);
  clientes   = signal<Cliente[]>([]);
  loading    = signal(true);

  get totalPropostas()  { return this.propostas().length; }
  get totalPrestadores(){ return this.prestadores().length; }
  get totalClientes()   { return this.clientes().length; }
  get propostasPendentes() {
    return this.propostas().filter((p) => p.status === 'Pendente').length;
  }

  ngOnInit(): void {
    this.api.listarPropostas().subscribe({ next: (d) => this.propostas.set(d) });
    this.api.listarPrestadores().subscribe({ next: (d) => this.prestadores.set(d) });
    this.api.listarClientes().subscribe({
      next: (d) => { this.clientes.set(d); this.loading.set(false); },
      error: () => this.loading.set(false),
    });
  }

  statusClass(status?: string): string {
    const map: Record<string, string> = {
      Pendente: 'badge-warn',
      Aceita:   'badge-success',
      Recusada: 'badge-danger',
      Concluida:'badge-accent',
    };
    return map[status ?? ''] ?? 'badge-warn';
  }
}
