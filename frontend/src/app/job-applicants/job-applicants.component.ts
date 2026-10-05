import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { EmployerService, ApplicantDto } from '../services/employer.service';
import { ApplicationsService, ApplicationDto } from '../services/applications.service';

@Component({
  selector: 'app-job-applicants',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './job-applicants.component.html',
  styleUrl: './job-applicants.component.scss'
})
export class JobApplicantsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private employerService = inject(EmployerService);
  private applicationsService = inject(ApplicationsService);

  jobId: string = '';
  applicants: ApplicantDto[] = [];
  loading = true;
  error: string | null = null;

  ngOnInit(): void {
    this.jobId = this.route.snapshot.paramMap.get('id') || '';
    if (this.jobId) {
      this.loadApplicants();
    } else {
      this.error = 'Invalid job ID';
      this.loading = false;
    }
  }

  loadApplicants(): void {
    this.loading = true;
    this.employerService.getApplicants(this.jobId).subscribe({
      next: (applicants) => {
        this.applicants = applicants;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load applicants';
        this.loading = false;
        console.error(err);
      }
    });
  }

  updateApplicationStatus(applicationId: string, newStatus: string): void {
    this.applicationsService.updateStatus(applicationId, { status: newStatus }).subscribe({
      next: () => {
        alert(`Application status updated to ${newStatus}`);
      },
      error: (err) => {
        alert('Failed to update application status');
        console.error(err);
      }
    });
  }
}
