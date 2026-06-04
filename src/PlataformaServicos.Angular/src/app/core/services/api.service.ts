import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Cliente {
  id?: number;
  nome: string;
  email: string;
  telefone?: string;
}

export interface Prestador {
  id?: number;
  nome: string;
  email: string;
  especialidade: string;
  descricao?: string;
  valorHora?: number;
  avaliacao?: number;
  totalAvaliacoes?: number;
}

export interface Proposta {
  id?: number;
  clienteId: number;
  prestadorId: number;
  descricao: string;
  valor: number;
  status?: 'Pendente' | 'Aceita' | 'Recusada' | 'Concluida';
  dataCriacao?: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  
  // AQUI ESTÁ A CORREÇÃO: Definimos a URL do container/porta do backend
  private readonly API_URL = 'http://localhost:5006/api';

  // Clientes
  listarClientes(): Observable<Cliente[]> {
    return this.http.get<Cliente[]>(`${this.API_URL}/clientes`);
  }
  buscarCliente(id: number): Observable<Cliente> {
    return this.http.get<Cliente>(`${this.API_URL}/clientes/${id}`);
  }
  criarCliente(cliente: Cliente): Observable<Cliente> {
    return this.http.post<Cliente>(`${this.API_URL}/clientes`, cliente);
  }
  atualizarCliente(id: number, cliente: Cliente): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/clientes/${id}`, cliente);
  }
  deletarCliente(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/clientes/${id}`);
  }

  // Prestadores
  listarPrestadores(): Observable<Prestador[]> {
    return this.http.get<Prestador[]>(`${this.API_URL}/prestadores`);
  }
  buscarPrestador(id: number): Observable<Prestador> {
    return this.http.get<Prestador>(`${this.API_URL}/prestadores/${id}`);
  }
  criarPrestador(prestador: Prestador): Observable<Prestador> {
    return this.http.post<Prestador>(`${this.API_URL}/prestadores`, prestador);
  }
  atualizarPrestador(id: number, prestador: Prestador): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/prestadores/${id}`, prestador);
  }
  deletarPrestador(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/prestadores/${id}`);
  }

  // Propostas
  listarPropostas(): Observable<Proposta[]> {
    return this.http.get<Proposta[]>(`${this.API_URL}/propostas`);
  }
  buscarProposta(id: number): Observable<Proposta> {
    return this.http.get<Proposta>(`${this.API_URL}/propostas/${id}`);
  }
  criarProposta(proposta: Proposta): Observable<Proposta> {
    return this.http.post<Proposta>(`${this.API_URL}/propostas`, proposta);
  }
  atualizarProposta(id: number, proposta: Proposta): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/propostas/${id}`, proposta);
  }
  deletarProposta(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/propostas/${id}`);
  }
}