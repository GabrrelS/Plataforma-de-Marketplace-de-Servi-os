import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContratoService } from '../../core/services/contrato.service';
import { ClienteService } from '../../core/services/cliente.service';
import { PrestadorService } from '../../core/services/prestador.service';
import { ToastService } from '../../core/services/toast.service';
import { Contrato, Cliente, Prestador } from '../../core/models';

@Component({
  selector: 'app-contratos',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './contratos.component.html',
  styleUrl: './contratos.component.css'
})
export class ContratosComponent implements OnInit {
  private svc = inject(ContratoService);
  private clienteSvc = inject(ClienteService);
  private prestadorSvc = inject(PrestadorService);
  private toast = inject(ToastService);

  contratos = signal<Contrato[]>([]);
  clientes = signal<Cliente[]>([]);
  prestadores = signal<Prestador[]>([]);
  loading = signal(false);

  ngOnInit() {
    this.loading.set(true);
    this.svc.listar().subscribe({
      next: (d) => { this.contratos.set(d); this.loading.set(false); },
      error: () => { this.toast.show('Erro ao carregar contratos', 'error'); this.loading.set(false); }
    });
    this.clienteSvc.listar().subscribe(d => this.clientes.set(d));
    this.prestadorSvc.listar().subscribe(d => this.prestadores.set(d));
  }

  nomeCliente(id: number): string {
    return this.clientes().find(c => c.id === id)?.nome ?? '—';
  }

  nomePrestador(id: number): string {
    return this.prestadores().find(p => p.id === id)?.nome ?? '—';
  }

  encerrar(contrato: Contrato) {
    if (!confirm('Encerrar este contrato?')) return;
    const atualizado: Contrato = { ...contrato, status: 'Encerrado' };
    this.svc.atualizar(contrato.id!, atualizado).subscribe({
      next: () => { this.toast.show('Contrato encerrado.'); this.ngOnInit(); },
      error: () => this.toast.show('Erro ao encerrar', 'error')
    });
  }

  badgeClass(status: string = ''): string {
    return 'badge badge-' + status.toLowerCase();
  }
}
