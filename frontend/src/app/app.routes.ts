import { Routes } from '@angular/router';
import { CandidateRegistrationComponent } from './candidate-registration/candidate-registration.component';
import { LandingComponent } from './landing/landing.component';
import { RoleSelectComponent } from './role-select/role-select.component';
import { CvUploadComponent } from './cv-upload/cv-upload.component';
import { JobsComponent } from './jobs/jobs.component';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'landing', component: LandingComponent },
  { path: 'role-select', component: RoleSelectComponent },
  { path: 'candidate-registration', component: CandidateRegistrationComponent },
  { path: 'cv-upload', component: CvUploadComponent },
  { path: 'jobs', component: JobsComponent },
  { path: '**', redirectTo: '' },
];
