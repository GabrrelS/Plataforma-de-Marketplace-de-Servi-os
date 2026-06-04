import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PropostaService } from '../../core/services/proposta.service';
import { ClienteService } from '../../core/services/cliente.service';
import { PrestadorService } from '../../core/services/prestador.service';
import { ToastService } from '../../core/services/toast.service';
import { Proposta, Cliente, Prestador } from '../../core/models';

@Component({
  selector: 'app-propostas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './propostas.component.html',
  styleUrl: './propostas.component.css'
})
export class PropostasComponent implements OnInit {
  private svc = inject(PropostaService);
  private clienteSvc = inject(ClienteService);
  private prestadorSvc = inject(PrestadorService);
  private toast = inject(ToastService);

  propostas = signal<Proposta[]>([]);
  clientes = signal<Cliente[]>([]);
  prestadores = signal<Prestador[]>([]);
  loading = signal(false);
  modalAberto = signal(false);

  form: Proposta = { titulo: '', descricao: '', valor: 0, clienteId: 0, prestadorId: 0 };

  ngOnInit() {
    this.carregar();
    this.clienteSvc.listar().subscribe(d => this.clientes.set(d));
    this.prestadorSvc.listar().subscribe(d => this.prestadores.set(d));
  }

  carregar() {
    this.loading.set(true);
    this.svc.listar().subscribe({
      next: (data) => { this.propostas.set(data); this.loading.set(false); },
      error: () => { this.toast.show('Erro ao carregar propostas', 'error'); this.loading.set(false); }
    });
  }

  nomeCliente(id: number): string {
    return this.clientes().find(c => c.id === id)?.nome ?? '—';
  }

  nomePrestador(id: number): string {
    return this.prestadores().find(p => p.id === id)?.nome ?? '—';
  }

  abrirModal() {
    this.form = { titulo: '', descricao: '', valor: 0, clienteId: this.clientes()[0]?.id ?? 0, prestadorId: this.prestadores()[0]?.id ?? 0 };
    this.modalAberto.set(true);
  }

  fecharModal() { this.modalAberto.set(false); }

  salvar() {
    this.svc.criar(this.form).subscribe({
      next: () => { this.toast.show('Proposta criada!'); this.fecharModal(); this.carregar(); },
      error: () => this.toast.show('Erro ao criar proposta', 'error')
    });
  }

  aceitar(proposta: Proposta) {
    const atualizada: Proposta = { ...proposta, status: 'Aceita' };
    this.svc.atualizar(proposta.id!, atualizada).subscribe({
      next: () => { this.toast.show('Proposta aceita!'); this.carregar(); },
      error: () => this.toast.show('Erro ao aceitar', 'error')
    });
  }

  recusar(proposta: Proposta) {
    const atualizada: Proposta = { ...proposta, status: 'Recusada' };
    this.svc.atualizar(proposta.id!, atualizada).subscribe({
      next: () => { this.toast.show('Proposta recusada.'); this.carregar(); },
      error: () => this.toast.show('Erro ao recusar', 'error')
    });
  }

  deletar(id: number) {
    if (!confirm('Remover esta proposta?')) return;
    this.svc.deletar(id).subscribe({
      next: () => { this.toast.show('Proposta removida.'); this.carregar(); },
      error: () => this.toast.show('Erro ao remover', 'error')
    });
  }

  badgeClass(status: string = ''): string {
    return 'badge badge-' + status.toLowerCase();
  }
}
