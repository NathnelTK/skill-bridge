import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CandidatesService, CandidateApplicationDto } from '../services/candidates.service';

@Component({
  selector: 'app-candidate-applications',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './candidate-applications.component.html',
  styleUrl: './candidate-applications.component.scss',
})
export class CandidateApplicationsComponent implements OnInit {
  private candidatesService = inject(CandidatesService);
  private changeDetector = inject(ChangeDetectorRef);

  applications: CandidateApplicationDto[] = [];
  loading = true;
  error: string | null = null;

  ngOnInit(): void {
    this.loadApplications();
  }

  loadApplications(): void {
    this.loading = true;
    this.candidatesService.listApplications().subscribe({
      next: (applications) => {
        this.applications = applications;
        this.loading = false;
        this.changeDetector.markForCheck();
      },
      error: (err) => {
        this.error = 'Failed to load applications';
        this.loading = false;
        console.error(err);
        this.changeDetector.markForCheck();
      },
    });
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'pending':
        return 'status-pending';
      case 'accepted':
        return 'status-accepted';
      case 'rejected':
        return 'status-rejected';
      case 'reviewed':
        return 'status-reviewed';
      default:
        return 'status-default';
    }
  }
}
