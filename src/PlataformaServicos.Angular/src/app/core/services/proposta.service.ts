import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Proposta } from '../models';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PropostaService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/propostas`;

  listar(): Observable<Proposta[]> {
    return this.http.get<Proposta[]>(this.url);
  }

  buscarPorId(id: number): Observable<Proposta> {
    return this.http.get<Proposta>(`${this.url}/${id}`);
  }

  criar(proposta: Proposta): Observable<Proposta> {
    return this.http.post<Proposta>(this.url, proposta);
  }

  atualizar(id: number, proposta: Proposta): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, proposta);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
