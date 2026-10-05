import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

type Role = 'candidate' | 'employer';

@Component({
  selector: 'app-role-select',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './role-select.component.html',
  styleUrl: './role-select.component.scss',
})
export class RoleSelectComponent {
  private readonly router = inject(Router);
  selected = signal<Role>('candidate');

  roles: { id: Role; title: string; desc: string; icon: string }[] = [
    {
      id: 'candidate',
      title: 'Candidate',
      desc: 'Find internships and jobs, build your profile.',
      icon: '👤',
    },
    {
      id: 'employer',
      title: 'Employer',
      desc: 'Post jobs and find the best talent for your team.',
      icon: '🏢',
    },
  ];

  select(role: Role) {
    this.selected.set(role);
  }

  continue() {
    if (this.selected() === 'candidate') {
      this.router.navigateByUrl('/candidate-registration');
      return;
    }

    this.router.navigateByUrl('/landing');
  }
}