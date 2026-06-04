import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ApiService, Prestador, Proposta } from '../../core/services/api.service';

@Component({
  selector: 'app-detalhe',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './detalhe.html',
  styleUrl: './detalhe.css',
})
export class DetalheComponent implements OnInit {
  private route  = inject(ActivatedRoute);
  private api    = inject(ApiService);

  prestador  = signal<Prestador | null>(null);
  loading    = signal(true);
  activeTab  = signal<'desc' | 'proposta'>('desc');
  sucesso    = signal(false);
  enviando   = signal(false);

  proposta: Partial<Proposta> = { descricao: '', valor: 0, clienteId: 1 };

  get iniciais(): string {
    return (this.prestador()?.nome ?? '??')
      .split(' ').map((n) => n[0]).slice(0, 2).join('').toUpperCase();
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.api.buscarPrestador(id).subscribe({
      next: (p) => { this.prestador.set(p); this.loading.set(false); },
      error: () => this.loading.set(false),
    });
  }

  enviarProposta(): void {
    const p = this.prestador();
    if (!p?.id) return;
    this.enviando.set(true);
    const payload: Proposta = {
      clienteId:   this.proposta.clienteId ?? 1,
      prestadorId: p.id,
      descricao:   this.proposta.descricao ?? '',
      valor:       this.proposta.valor ?? 0,
      status:      'Pendente',
    };
    this.api.criarProposta(payload).subscribe({
      next: () => { this.sucesso.set(true); this.enviando.set(false); },
      error: () => this.enviando.set(false),
    });
  }
}
