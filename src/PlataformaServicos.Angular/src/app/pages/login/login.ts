import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class LoginComponent {
  private router = inject(Router);

  email    = '';
  senha    = '';
  loading  = signal(false);
  erro     = signal('');

  constructor() {}

  entrar(): void {
    if (!this.email || !this.senha) {
      this.erro.set('Preencha todos os campos.');
      return;
    }
    this.loading.set(true);
    setTimeout(() => {
      this.loading.set(false);
      this.router.navigate(['/dashboard']);
    }, 800);
  }
}