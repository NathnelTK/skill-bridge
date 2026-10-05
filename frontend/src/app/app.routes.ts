import { Routes } from '@angular/router';
import { CandidateRegistrationComponent } from './candidate-registration/candidate-registration.component';
import { LandingComponent } from './landing/landing.component';
import { RoleSelectComponent } from './role-select/role-select.component';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'landing', component: LandingComponent },
  { path: 'role-select', component: RoleSelectComponent },
  { path: 'candidate-registration', component: CandidateRegistrationComponent },
  { path: '**', redirectTo: '' },
];
