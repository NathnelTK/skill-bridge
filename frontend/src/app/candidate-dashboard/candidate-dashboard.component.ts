import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CandidatesService, CandidateProfileDto } from '../services/candidates.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-candidate-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './candidate-dashboard.component.html',
  styleUrl: './candidate-dashboard.component.scss'
})
export class CandidateDashboardComponent implements OnInit {
  private candidatesService = inject(CandidatesService);
  private authService = inject(AuthService);

  profile: CandidateProfileDto | null = null;
  loading = true;
  error: string | null = null;

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.loading = true;
    this.candidatesService.getProfile().subscribe({
      next: (profile) => {
        this.profile = profile;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load profile';
        this.loading = false;
        console.error(err);
      }
    });
  }

  logout(): void {
    this.authService.logout();
    window.location.href = '/login';
  }
}
