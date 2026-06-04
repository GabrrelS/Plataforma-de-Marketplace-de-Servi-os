import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: 'dashboard',
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  {
    path: 'clientes',
    loadComponent: () => import('./features/clientes/clientes.component').then(m => m.ClientesComponent)
  },
  {
    path: 'prestadores',
    loadComponent: () => import('./features/prestadores/prestadores.component').then(m => m.PrestadoresComponent)
  },
  {
    path: 'propostas',
    loadComponent: () => import('./features/propostas/propostas.component').then(m => m.PropostasComponent)
  },
  {
    path: 'contratos',
    loadComponent: () => import('./features/contratos/contratos.component').then(m => m.ContratosComponent)
  },
  { path: '**', redirectTo: 'dashboard' }
];
