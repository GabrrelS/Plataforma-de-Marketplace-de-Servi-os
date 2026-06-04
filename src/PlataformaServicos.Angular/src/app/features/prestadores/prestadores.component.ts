import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PrestadorService } from '../../core/services/prestador.service';
import { ToastService } from '../../core/services/toast.service';
import { Prestador } from '../../core/models';

@Component({
  selector: 'app-prestadores',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './prestadores.component.html',
  styleUrl: './prestadores.component.css'
})
export class PrestadoresComponent implements OnInit {
  private svc = inject(PrestadorService);
  private toast = inject(ToastService);

  prestadores = signal<Prestador[]>([]);
  loading = signal(false);
  modalAberto = signal(false);
  editando = signal<Prestador | null>(null);

  form: Prestador = { nome: '', email: '', especialidade: '' };

  ngOnInit() { this.carregar(); }

  carregar() {
    this.loading.set(true);
    this.svc.listar().subscribe({
      next: (data) => { this.prestadores.set(data); this.loading.set(false); },
      error: () => { this.toast.show('Erro ao carregar prestadores', 'error'); this.loading.set(false); }
    });
  }

  abrirModal(prestador?: Prestador) {
    if (prestador) {
      this.editando.set(prestador);
      this.form = { ...prestador };
    } else {
      this.editando.set(null);
      this.form = { nome: '', email: '', especialidade: '' };
    }
    this.modalAberto.set(true);
  }

  fecharModal() { this.modalAberto.set(false); }

  salvar() {
    const ed = this.editando();
    if (ed?.id) {
      this.svc.atualizar(ed.id, this.form).subscribe({
        next: () => { this.toast.show('Prestador atualizado!'); this.fecharModal(); this.carregar(); },
        error: () => this.toast.show('Erro ao atualizar', 'error')
      });
    } else {
      this.svc.criar(this.form).subscribe({
        next: () => { this.toast.show('Prestador criado!'); this.fecharModal(); this.carregar(); },
        error: () => this.toast.show('Erro ao criar', 'error')
      });
    }
  }

  deletar(id: number) {
    if (!confirm('Remover este prestador?')) return;
    this.svc.deletar(id).subscribe({
      next: () => { this.toast.show('Prestador removido.'); this.carregar(); },
      error: () => this.toast.show('Erro ao remover', 'error')
    });
  }

  iniciais(nome: string): string {
    return nome.split(' ').slice(0, 2).map(w => w[0]).join('').toUpperCase();
  }

  estrelas(nota: number): string {
    const n = Math.round(nota);
    return '★'.repeat(n) + '☆'.repeat(5 - n);
  }
}
