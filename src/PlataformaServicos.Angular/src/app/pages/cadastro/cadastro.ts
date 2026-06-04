import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ApiService } from '../../core/services/api.service';

@Component({
  selector: 'app-cadastro',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './cadastro.html',
  styleUrl: './cadastro.css',
})
export class CadastroComponent {
  private api    = inject(ApiService);
  private router = inject(Router);

  tipo       = signal<'cliente' | 'prestador'>('cliente');
  loading    = signal(false);
  erro       = signal('');
  sucesso    = signal(false);

  nome         = '';
  email        = '';
  senha        = '';
  especialidade = '';

  setTipo(t: 'cliente' | 'prestador') { this.tipo.set(t); }

  cadastrar(): void {
    this.erro.set('');
    if (!this.nome || !this.email || !this.senha) {
      this.erro.set('Preencha todos os campos obrigatórios.');
      return;
    }
    if (this.tipo() === 'prestador' && !this.especialidade) {
      this.erro.set('Informe sua especialidade.');
      return;
    }

    this.loading.set(true);

    if (this.tipo() === 'cliente') {
      this.api.criarCliente({ nome: this.nome, email: this.email }).subscribe({
        next: () => { this.sucesso.set(true); this.loading.set(false); setTimeout(() => this.router.navigate(['/login']), 1500); },
        error: () => { this.erro.set('Erro ao cadastrar. Tente novamente.'); this.loading.set(false); },
      });
    } else {
      this.api.criarPrestador({ nome: this.nome, email: this.email, especialidade: this.especialidade }).subscribe({
        next: () => { this.sucesso.set(true); this.loading.set(false); setTimeout(() => this.router.navigate(['/login']), 1500); },
        error: () => { this.erro.set('Erro ao cadastrar. Tente novamente.'); this.loading.set(false); },
      });
    }
  }
}
