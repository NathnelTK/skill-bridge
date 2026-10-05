import { Routes } from '@angular/router';
import { candidateGuard, employerGuard } from './core/auth/auth.guards';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then((m) => m.Login)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register').then((m) => m.Register)
  },
  {
    path: 'jobs',
    canActivate: [candidateGuard],
    loadComponent: () =>
      import('./features/placeholder/placeholder').then((m) => m.Placeholder),
    data: { title: 'Browse jobs' }
  },
  {
    path: 'my-applications',
    canActivate: [candidateGuard],
    loadComponent: () =>
      import('./features/placeholder/placeholder').then((m) => m.Placeholder),
    data: { title: 'My applications' }
  },
  {
    path: 'create-job',
    canActivate: [employerGuard],
    loadComponent: () =>
      import('./features/placeholder/placeholder').then((m) => m.Placeholder),
    data: { title: 'Create a job' }
  },
  {
    path: 'applicants',
    canActivate: [employerGuard],
    loadComponent: () =>
      import('./features/placeholder/placeholder').then((m) => m.Placeholder),
    data: { title: 'Applicants' }
  },
  { path: '**', redirectTo: 'login' }
];
