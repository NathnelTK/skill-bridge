import { Routes } from '@angular/router';
import { roleGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./landing/landing.component').then((m) => m.LandingComponent),
  },
  {
    path: 'landing',
    loadComponent: () => import('./landing/landing.component').then((m) => m.LandingComponent),
  },
  {
    path: 'role-select',
    loadComponent: () =>
      import('./role-select/role-select.component').then((m) => m.RoleSelectComponent),
  },
  {
    path: 'login',
    loadComponent: () => import('./login/login.component').then((m) => m.LoginComponent),
  },
  {
    path: 'candidate-registration',
    loadComponent: () =>
      import('./candidate-registration/candidate-registration.component').then(
        (m) => m.CandidateRegistrationComponent,
      ),
  },
  {
    path: 'employer-registration',
    loadComponent: () =>
      import('./employer-registration/employer-registration.component').then(
        (m) => m.EmployerRegistrationComponent,
      ),
  },
  {
    path: 'cv-upload',
    canActivate: [roleGuard('Candidate')],
    loadComponent: () => import('./cv-upload/cv-upload.component').then((m) => m.CvUploadComponent),
  },
  {
    path: 'jobs',
    loadComponent: () => import('./jobs/jobs.component').then((m) => m.JobsComponent),
  },
  {
    path: 'employer/dashboard',
    canActivate: [roleGuard('Employer')],
    loadComponent: () =>
      import('./employer-dashboard/employer-dashboard.component').then(
        (m) => m.EmployerDashboardComponent,
      ),
  },
  {
    path: 'employer/jobs/:id/applicants',
    canActivate: [roleGuard('Employer')],
    loadComponent: () =>
      import('./job-applicants/job-applicants.component').then((m) => m.JobApplicantsComponent),
  },
  {
    path: 'jobs/create',
    canActivate: [roleGuard('Employer')],
    loadComponent: () => import('./job-form/job-form.component').then((m) => m.JobFormComponent),
  },
  {
    path: 'jobs/:id/edit',
    canActivate: [roleGuard('Employer')],
    loadComponent: () => import('./job-form/job-form.component').then((m) => m.JobFormComponent),
  },
  {
    path: 'candidate/dashboard',
    canActivate: [roleGuard('Candidate')],
    loadComponent: () =>
      import('./candidate-dashboard/candidate-dashboard.component').then(
        (m) => m.CandidateDashboardComponent,
      ),
  },
  {
    path: 'candidate/profile/edit',
    canActivate: [roleGuard('Candidate')],
    loadComponent: () =>
      import('./candidate-profile-edit/candidate-profile-edit.component').then(
        (m) => m.CandidateProfileEditComponent,
      ),
  },
  {
    path: 'candidate/cv-upload',
    canActivate: [roleGuard('Candidate')],
    loadComponent: () => import('./cv-upload/cv-upload.component').then((m) => m.CvUploadComponent),
  },
  {
    path: 'candidate/applications',
    canActivate: [roleGuard('Candidate')],
    loadComponent: () =>
      import('./candidate-applications/candidate-applications.component').then(
        (m) => m.CandidateApplicationsComponent,
      ),
  },
  { path: '**', redirectTo: '' },
];
