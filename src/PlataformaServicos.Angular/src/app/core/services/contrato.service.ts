import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Contrato } from '../models';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ContratoService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/contratos`;

  listar(): Observable<Contrato[]> {
    return this.http.get<Contrato[]>(this.url);
  }

  buscarPorId(id: number): Observable<Contrato> {
    return this.http.get<Contrato>(`${this.url}/${id}`);
  }

  criar(contrato: Contrato): Observable<Contrato> {
    return this.http.post<Contrato>(this.url, contrato);
  }

  atualizar(id: number, contrato: Contrato): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, contrato);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
