import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class LoginComponent {
  private http   = inject(HttpClient);
  private router = inject(Router);

  email   = '';
  senha   = '';
  loading = signal(false);
  erro    = signal('');

  entrar(): void {
    this.erro.set('');

    if (!this.email || !this.senha) {
      this.erro.set('Preencha todos os campos.');
      return;
    }

    this.loading.set(true);

    this.http
      .post<{ token: string; perfil: string; nome: string }>(
        'http://localhost:5006/api/auth/login',
        { email: this.email, senha: this.senha }
      )
      .subscribe({
        next: (res) => {
          localStorage.setItem('token', res.token);
          localStorage.setItem('perfil', res.perfil);
          localStorage.setItem('nome', res.nome);
          this.loading.set(false);
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          this.loading.set(false);
          if (err.status === 401) {
            this.erro.set('E-mail ou senha inválidos.');
          } else {
            this.erro.set('Erro ao conectar. Tente novamente.');
          }
        },
      });
  }
}
