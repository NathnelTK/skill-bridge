import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../services/auth.service';
import { EmployerService, JobSummaryDto } from '../services/employer.service';

@Component({
  selector: 'app-employer-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './employer-dashboard.component.html',
  styleUrl: './employer-dashboard.component.scss',
})
export class EmployerDashboardComponent implements OnInit {
  private employerService = inject(EmployerService);
  private authService = inject(AuthService);
  private changeDetector = inject(ChangeDetectorRef);
  jobs: JobSummaryDto[] = [];
  loading = true;
  error: string | null = null;

  get companyName(): string {
    return this.jobs[0]?.companyName || 'Your hiring dashboard';
  }

  get locationCount(): number {
    return new Set(this.jobs.map((job) => job.location.trim()).filter(Boolean)).size;
  }

  get skillCount(): number {
    return new Set(
      this.jobs.flatMap((job) => job.requiredSkills.map((skill) => skill.name.trim().toLowerCase())),
    ).size;
  }

  ngOnInit(): void {
    this.loadJobs();
  }

  loadJobs(): void {
    this.loading = true;
    this.error = null;
    this.employerService.listJobs().subscribe({
      next: (jobs) => {
        this.jobs = (jobs ?? []).map((job) => ({
          ...job,
          requiredSkills: job.requiredSkills ?? [],
        }));
        this.loading = false;
        this.changeDetector.markForCheck();
      },
      error: (err) => {
        this.error = err.error?.detail ?? 'Please check your connection and try again.';
        this.loading = false;
        console.error(err);
        this.changeDetector.markForCheck();
      },
    });
  }

  logout(): void {
    this.authService.logout();
    window.location.assign('/login');
  }
}
