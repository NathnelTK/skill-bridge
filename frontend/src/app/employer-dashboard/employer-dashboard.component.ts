import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { EmployerService, JobSummaryDto } from '../services/employer.service';

@Component({
  selector: 'app-employer-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './employer-dashboard.component.html',
  styleUrl: './employer-dashboard.component.scss',
})
export class EmployerDashboardComponent implements OnInit {
  private employerService = inject(EmployerService);
  private changeDetector = inject(ChangeDetectorRef);
  jobs: JobSummaryDto[] = [];
  loading = true;
  error: string | null = null;

  ngOnInit(): void {
    this.loadJobs();
  }

  loadJobs(): void {
    this.loading = true;
    this.employerService.listJobs().subscribe({
      next: (jobs) => {
        this.jobs = jobs;
        this.loading = false;
        this.changeDetector.markForCheck();
      },
      error: (err) => {
        this.error = 'Failed to load jobs';
        this.loading = false;
        console.error(err);
        this.changeDetector.markForCheck();
      },
    });
  }
}
