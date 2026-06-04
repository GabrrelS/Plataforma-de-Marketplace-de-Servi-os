import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Prestador } from '../models';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PrestadorService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/prestadores`;

  listar(): Observable<Prestador[]> {
    return this.http.get<Prestador[]>(this.url);
  }

  buscarPorId(id: number): Observable<Prestador> {
    return this.http.get<Prestador>(`${this.url}/${id}`);
  }

  criar(prestador: Prestador): Observable<Prestador> {
    return this.http.post<Prestador>(this.url, prestador);
  }

  atualizar(id: number, prestador: Prestador): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, prestador);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
