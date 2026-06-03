import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface VersionInfo {
  version: string;
  environment: string;
  buildDate: string;
}

@Injectable({
  providedIn: 'root',
})
export class Version {
  private http = inject(HttpClient);

  getVersion(): Observable<VersionInfo> {
    return this.http.get<VersionInfo>(
      'http://localhost:5006/api/v1/version'
    );
  }
}