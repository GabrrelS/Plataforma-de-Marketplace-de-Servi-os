import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClienteService } from '../../core/services/cliente.service';
import { ToastService } from '../../core/services/toast.service';
import { Cliente } from '../../core/models';

@Component({
  selector: 'app-clientes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './clientes.component.html',
  styleUrl: './clientes.component.css'
})
export class ClientesComponent implements OnInit {
  private svc = inject(ClienteService);
  private toast = inject(ToastService);

  clientes = signal<Cliente[]>([]);
  loading = signal(false);
  modalAberto = signal(false);
  editando = signal<Cliente | null>(null);

  form: Cliente = { nome: '', email: '', telefone: '' };

  ngOnInit() {
    this.carregar();
  }

  carregar() {
    this.loading.set(true);
    this.svc.listar().subscribe({
      next: (data) => { this.clientes.set(data); this.loading.set(false); },
      error: () => { this.toast.show('Erro ao carregar clientes', 'error'); this.loading.set(false); }
    });
  }

  abrirModal(cliente?: Cliente) {
    if (cliente) {
      this.editando.set(cliente);
      this.form = { ...cliente };
    } else {
      this.editando.set(null);
      this.form = { nome: '', email: '', telefone: '' };
    }
    this.modalAberto.set(true);
  }

  fecharModal() {
    this.modalAberto.set(false);
  }

  salvar() {
    const ed = this.editando();
    if (ed?.id) {
      this.svc.atualizar(ed.id, this.form).subscribe({
        next: () => { this.toast.show('Cliente atualizado!'); this.fecharModal(); this.carregar(); },
        error: () => this.toast.show('Erro ao atualizar', 'error')
      });
    } else {
      this.svc.criar(this.form).subscribe({
        next: () => { this.toast.show('Cliente criado!'); this.fecharModal(); this.carregar(); },
        error: () => this.toast.show('Erro ao criar', 'error')
      });
    }
  }

  deletar(id: number) {
    if (!confirm('Remover este cliente?')) return;
    this.svc.deletar(id).subscribe({
      next: () => { this.toast.show('Cliente removido.'); this.carregar(); },
      error: () => this.toast.show('Erro ao remover', 'error')
    });
  }

  iniciais(nome: string): string {
    return nome.split(' ').slice(0, 2).map(w => w[0]).join('').toUpperCase();
  }
}
