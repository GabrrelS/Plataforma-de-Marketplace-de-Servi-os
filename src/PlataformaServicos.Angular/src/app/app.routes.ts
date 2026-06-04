import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/home/home').then((m) => m.HomeComponent),
  },
  {
    path: 'explorar',
    loadComponent: () => import('./pages/explorar/explorar').then((m) => m.ExplorarComponent),
  },
  {
    path: 'prestadores/:id',
    loadComponent: () => import('./pages/detalhe/detalhe').then((m) => m.DetalheComponent),
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./pages/dashboard/dashboard').then((m) => m.DashboardComponent),
  },
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login').then((m) => m.LoginComponent),
  },
  {
    path: 'cadastro',
    loadComponent: () => import('./pages/cadastro/cadastro').then((m) => m.CadastroComponent),
  },
  { path: '**', redirectTo: '' },
];