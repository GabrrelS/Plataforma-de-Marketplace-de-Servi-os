import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface VersionInfo {
  version: string;
  environment: string;
  buildDate: string;
}

@Injectable({
  providedIn: 'root'
})
export class VersionService {

  private http = inject(HttpClient);

  getVersion(): Observable<VersionInfo> {
    return this.http.get<VersionInfo>('/api/v1/version');
  }
}