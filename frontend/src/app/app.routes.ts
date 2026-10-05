import { Routes } from '@angular/router';
import { CandidateDashboardComponent } from './candidate-dashboard/candidate-dashboard.component';
import { CandidateRegistrationComponent } from './candidate-registration/candidate-registration.component';
import { EmployerDashboardComponent } from './employer-dashboard/employer-dashboard.component';
import { EmployerLoginComponent } from './employer-login/employer-login.component';
import { EmployerRegistrationComponent } from './employer-registration/employer-registration.component';
import { LandingComponent } from './landing/landing.component';
import { LoginComponent } from './login/login.component';
import { RoleSelectComponent } from './role-select/role-select.component';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'landing', component: LandingComponent },
  { path: 'role-select', component: RoleSelectComponent },
  { path: 'candidate-registration', component: CandidateRegistrationComponent },
  { path: 'employer-registration', component: EmployerRegistrationComponent },
  { path: 'login', component: LoginComponent },
  { path: 'employer-login', component: EmployerLoginComponent },
  { path: 'employer-dashboard', component: EmployerDashboardComponent },
  { path: 'candidate-dashboard', component: CandidateDashboardComponent },
  { path: 'candidate-dasboard', redirectTo: 'candidate-dashboard' },
  { path: '**', redirectTo: '' },
];
