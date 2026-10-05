import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-candidate-registration',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './candidate-registration.component.html',
  styleUrl: './candidate-registration.component.scss',
})
export class CandidateRegistrationComponent {
  readonly checklist = [
    'Join a growing community of skilled professionals.',
    'Build your profile',
    'Get matched with job opportunities',
    'Track your applications',
  ];
}
