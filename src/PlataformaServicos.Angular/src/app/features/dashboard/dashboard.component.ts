import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ClienteService } from '../../core/services/cliente.service';
import { PrestadorService } from '../../core/services/prestador.service';
import { PropostaService } from '../../core/services/proposta.service';
import { ContratoService } from '../../core/services/contrato.service';
import { Proposta, Prestador } from '../../core/models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  private clienteSvc = inject(ClienteService);
  private prestadorSvc = inject(PrestadorService);
  private propostaSvc = inject(PropostaService);
  private contratoSvc = inject(ContratoService);

  totalClientes = signal(0);
  totalPrestadores = signal(0);
  totalPropostas = signal(0);
  contratosAtivos = signal(0);
  propostasRecentes = signal<Proposta[]>([]);
  topPrestadores = signal<Prestador[]>([]);
  loading = signal(true);

  ngOnInit() {
    forkJoin({
      clientes: this.clienteSvc.listar(),
      prestadores: this.prestadorSvc.listar(),
      propostas: this.propostaSvc.listar(),
      contratos: this.contratoSvc.listar()
    }).subscribe({
      next: ({ clientes, prestadores, propostas, contratos }) => {
        this.totalClientes.set(clientes.length);
        this.totalPrestadores.set(prestadores.length);
        this.totalPropostas.set(propostas.length);
        this.contratosAtivos.set(contratos.filter(c => c.status === 'Ativo').length);
        this.propostasRecentes.set(propostas.slice(-5).reverse());
        this.topPrestadores.set([...prestadores].sort((a, b) => (b.notaMedia ?? 0) - (a.notaMedia ?? 0)).slice(0, 5));
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  badgeClass(status: string = ''): string {
    return 'badge badge-' + status.toLowerCase();
  }

  estrelas(nota: number = 0): string {
    const n = Math.round(nota);
    return '★'.repeat(n) + '☆'.repeat(5 - n);
  }

  iniciais(nome: string): string {
    return nome.split(' ').slice(0, 2).map(w => w[0]).join('').toUpperCase();
  }
}
