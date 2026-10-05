import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-candidate-registration',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './candidate-registration.component.html',
  styleUrl: './candidate-registration.component.scss',
})
export class CandidateRegistrationComponent {
  private readonly router = inject(Router);

  fullName = '';

  readonly checklist = [
    'Join a growing community of skilled professionals.',
    'Build your profile',
    'Get matched with job opportunities',
    'Track your applications',
  ];

  submit(): void {
    const name = this.fullName.trim();
    if (name) {
      localStorage.setItem('skillbridgeCandidateName', name);
    }
    this.router.navigateByUrl('/login');
  }
}
